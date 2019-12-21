using Neis.ProductKeyManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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
    /// Interaction logic for EditKeyControl.xaml
    /// </summary>
    public partial class EditKeyControl : UserControl
    {
        /// <summary>
        /// DeleteKey Command
        /// </summary>
        public static RoutedCommand DeleteKeyCommand = new RoutedCommand("DeleteKeyCommand", typeof(EditKeyControl));

        public EditKeyControl()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(DeleteKeyCommand, Execute_DeleteKeyCommand));
        }

        /// <summary>
        /// Occurs when the <see cref="DeleteKeyCommand"/> is executed
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="args">Arguments for the event.</param>
        private void Execute_DeleteKeyCommand(object sender, ExecutedRoutedEventArgs args)
        {
            var key = DataContext as GenericKey;
            if (key == null)
            {
                return;
            }

            if (!key.IsDirty && string.IsNullOrWhiteSpace(key.Value))
            {
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
    }
}
