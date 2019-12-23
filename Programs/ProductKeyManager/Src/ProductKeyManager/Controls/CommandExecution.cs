using Neis.ProductKeyManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Neis.ProductKeyManager.Controls
{
    internal class CommandExecution
    {
        private static T GetContextObject<T>(object sender) 
            where T : NotifiableBase
        {
            var control = sender as FrameworkElement;
            if (control == null)
            {
                return null;
            }

            return control.DataContext as T;
        }

        /// <summary>
        /// Determines whether or not the command to copy a key to the clipboard can execute
        /// </summary>
        /// <param name="sender">Sender of this event.</param>
        /// <param name="args">Arguments for this event.</param>
        internal static void CanExecute_CopytKeyToCliboardCommand(object sender, CanExecuteRoutedEventArgs args)
        {
            var val = GetContextObject<GenericKey>(sender);
            args.CanExecute = val != null && !string.IsNullOrWhiteSpace(val.Value);
        }
        /// <summary>
        /// Determines whether or not any commands that modify a key can be executed
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="args">Arguments for the event.</param>
        internal static void CanExecute_ModifyKeyCommand(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = GetContextObject<GenericKey>(sender) != null;
        }
        /// <summary>
        /// Determines whether or not any commands that modify a product can be executed
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="args">Arguments for the event.</param>
        internal static void CanExecute_ModifyProductCommand(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = GetContextObject<GenericProduct>(sender) != null;
        }
        /// <summary>
        /// Determines whether or not the command to save changes made to a product can be executed
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="args">Arguments for the event.</param>
        internal static void CanExecute_SaveProductCommand(object sender, CanExecuteRoutedEventArgs args)
        {
            var product = GetContextObject<GenericProduct>(sender);
            args.CanExecute = product != null && product.Keys.Count > 0 &&
                (product.IsDirty || product.Keys.Any(k => k.IsDirty)) &&
                product.Keys.All(k => !string.IsNullOrWhiteSpace(k.Value));
        }

        /// <summary>
        /// Occurs when command to add a product key is executed
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="args">Arguments for the event.</param>
        internal static void Execute_AddKeyCommand(object sender, ExecutedRoutedEventArgs args)
        {
            var p = GetContextObject<GenericProduct>(sender);
            if (p == null)
            {
                App.LogWriter.ShowError("Cannot execute 'Add Key' command because the 'product' object is null");
                return;
            }

            p.Keys.Add(new GenericKey());
        }
        /// <summary>
        /// Occurs when an add command is executed for a product
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="args">Arguments for the event.</param>
        internal static void Execute_AddProductCommand(object sender, ExecutedRoutedEventArgs args)
        {
            var keyFile = args.Parameter as GenericKeyFile;
            if (keyFile == null)
            {
                App.LogWriter.ShowError("Cannot execute 'Add Product' command because 'key file' is null");
                return;
            }

            var newProduct = new GenericProduct();

            var dlg = new EditProductControl();
            dlg.DataContext = newProduct;
            dlg.Owner = Application.Current.MainWindow;

            var res = dlg.ShowDialog();
            if (res != null && res.HasValue && res.Value &&
                !string.IsNullOrWhiteSpace(newProduct.Name) &&
                newProduct.Keys.Count > 0)
            {
                int removedCount = 0;
                for (int i = 0; i < newProduct.Keys.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(newProduct.Keys[i].Value))
                    {
                        newProduct.Keys.RemoveAt(i--);
                        removedCount++;
                    }
                }

                if (newProduct.Keys.Count > 0)
                {
                    keyFile.Products.Add(newProduct);
                }
            }
        }
        /// <summary>
        /// Occurs when a command to copy a product key to the clipboard has been executed
        /// </summary>
        /// <param name="sender">Sender of this event.</param>
        /// <param name="args">Arguments for this event.</param>
        internal static void Execute_CopyKeyToCliboardCommand(object sender, ExecutedRoutedEventArgs args)
        {
            var key = GetContextObject<GenericKey>(sender);
            if (key == null)
            {
                App.LogWriter.ShowError("Cannot execute 'Copy Key To Clipboard' command because 'key' is null");
                return;
            }

            Clipboard.SetText(key.Value);

            MessageBox.Show(
                Application.Current.MainWindow,
                string.Format("Key '{0}' copied to clipboard.", key.Value),
                "Key copied",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        /// <summary>
        /// Occurs when a command to edit a product is executed
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="args">Arguments for the event.</param>
        internal static void Execute_EditProductCommand(object sender, ExecutedRoutedEventArgs args)
        {
            var product = GetContextObject<GenericProduct>(sender);
            if (product == null)
            {
                App.LogWriter.ShowError("Cannot execute 'Edit Product' command because 'product' is null");
                return;
            }

            var dlg = new EditProductControl();
            dlg.DataContext = product;
            dlg.Owner = Application.Current.MainWindow;
            dlg.ShowDialog();
        }
        /// <summary>
        /// Occurs when an Exit command is executed
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="args">Arguments for this event</param>
        internal static void Execute_ExitCommand(object sender, ExecutedRoutedEventArgs args)
        {
            App.Current.MainWindow.Close();
        }
        /// <summary>
        /// Occurs when a command to delete a product key is executed
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="args">Arguments for the event.</param>
        internal static void Execute_DeleteKeyCommand(object sender, ExecutedRoutedEventArgs args)
        {
            var key = GetContextObject<GenericKey>(sender);
            if (key == null)
            {
                App.LogWriter.ShowError("Cannot execute 'Delete Key' command because the 'key' object is null");
                return;
            }

            if (!key.IsDirty && string.IsNullOrWhiteSpace(key.Value))
            {
                App.LogWriter.ShowRawText("Deleting key without confirmation because no changes have been made and the 'Value' is null");
                key.MarkForDeletion();
            }
            else
            {
                var result = MessageBox.Show(
                    Application.Current.MainWindow,
                    string.Format("Are you sure you want to delete {0}?", key.Value),
                    "Confirm delete key",
                    MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    key.MarkForDeletion();
                }
            }
        }
        /// <summary>
        /// Occurs when a delete command is executed for a product
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="args">Arguments for the event.</param>
        internal static void Execute_DeleteProductCommand(object sender, ExecutedRoutedEventArgs args)
        {
            var obj = GetContextObject<GenericProduct>(sender);
            if (obj == null)
            {
                App.LogWriter.ShowError("Cannot execute 'Delete Product' command because 'product' is null");
                return;
            }

            var result = MessageBox.Show(
                Application.Current.MainWindow,
                string.Format("Are you sure you want to delete {0} and all associated keys?", obj.Name),
                "Confirm delete product",
                MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                obj.IsMarkedDeleted = true;
            }
        }
    }
}
