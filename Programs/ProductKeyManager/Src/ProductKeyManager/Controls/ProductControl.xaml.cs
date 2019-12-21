using Neis.ProductKeyManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Neis.ProductKeyManager.Controls
{
    /// <summary>
    /// Interaction logic for ProductControl.xaml
    /// </summary>
    public partial class ProductControl : UserControl
    {
        /// <summary>
        /// Edit Command
        /// </summary>
        public static RoutedCommand EditCommand = new RoutedCommand("EditCommand", typeof(ProductControl));
        /// <summary>
        /// Delete Command
        /// </summary>
        public static RoutedCommand DeleteCommand = new RoutedCommand("DeleteCommand", typeof(ProductControl));

        /// <summary>
        /// Constructor for the <see cref="ProductControl"/> class
        /// </summary>
        public ProductControl()
        {
            InitializeComponent();

            this.CommandBindings.Add(new CommandBinding(EditCommand, Execute_EditCommand, CanExecute_ModifyCommand));
            this.CommandBindings.Add(new CommandBinding(DeleteCommand, Execute_DeleteCommand, CanExecute_ModifyCommand));
        }

        private GenericProduct GetDataContext(object sender)
        {
            var control = sender as ProductControl;
            if (control == null)
            {
                return null;
            }
            var product = control.DataContext as GenericProduct;
            return product;
        }

        /// <summary>
        /// Determines whether or not any modify commands can be executed
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="args">Arguments for the event.</param>
        private void CanExecute_ModifyCommand(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = GetDataContext(sender) != null;
        }
        /// <summary>
        /// Occurs when the <see cref="EditCommand"/> is executed
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="args">Arguments for the event.</param>
        private void Execute_EditCommand(object sender, ExecutedRoutedEventArgs args)
        {
            var product = GetDataContext(sender);
            if (product == null)
            {
                return;
            }

            var dlg = new EditProductControl();
            dlg.DataContext = product;
            dlg.Owner = Window.GetWindow(this);
            dlg.ShowDialog();
        }
        /// <summary>
        /// Occurs when the <see cref="DeleteCommand"/> is executed
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="args">Arguments for the event.</param>
        private void Execute_DeleteCommand(object sender, ExecutedRoutedEventArgs args)
        {
            var product = GetDataContext(sender);
            if (product == null)
            {
                return;
            }

            var result = MessageBox.Show(
                Application.Current.MainWindow, 
                string.Format("Are you sure you want to delete {0} and all associated keys?", product.Name), 
                "Confirm delete product", 
                MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                product.IsMarkedDeleted = true;
            }
        }
    }
}
