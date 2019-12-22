using Neis.ProductKeyManager.Data;
using System.Windows;
using System.Windows.Input;

namespace Neis.ProductKeyManager.Controls
{
    /// <summary>
    /// Interaction logic for EditProduct.xaml
    /// </summary>
    public partial class EditProductControl : Window
    {

        /// <summary>
        /// AddKey Command
        /// </summary>
        public static RoutedCommand AddKeyCommand = new RoutedCommand("AddKeyCommand", typeof(EditProductControl));
        /// <summary>
        /// OK Command
        /// </summary>
        public static RoutedCommand CancelCommand = new RoutedCommand("CancelCommand", typeof(EditProductControl));        
        /// <summary>
        /// OK Command
        /// </summary>
        public static RoutedCommand OkCommand = new RoutedCommand("OkCommand", typeof(EditProductControl));
        
        /// <summary>
        /// Constructor for the EditProductControl class
        /// </summary>
        public EditProductControl()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(AddKeyCommand, CommandExecution.Execute_AddKeyCommand, CommandExecution.CanExecute_ModifyProductCommand));
            CommandBindings.Add(new CommandBinding(CancelCommand, Execute_CancelCommand));
            CommandBindings.Add(new CommandBinding(OkCommand, Execute_OkCommand, CommandExecution.CanExecute_SaveProductCommand));
        }

        /// <summary>
        /// Occurs when the OK command is executed
        /// </summary>
        /// <param name="sender">Sender of this event.</param>
        /// <param name="args">Arguments for this event.</param>
        private void Execute_OkCommand(object sender, ExecutedRoutedEventArgs args)
        {
            this.DialogResult = true;
            this.Close();
        }
        /// <summary>
        /// Occurs when the Cancel command is executed
        /// </summary>
        /// <param name="sender">Sender of this event.</param>
        /// <param name="args">Arguments for this event.</param>
        private void Execute_CancelCommand(object sender, ExecutedRoutedEventArgs args)
        {
            var product = DataContext as GenericProduct;
            if (product != null && product.IsDirty)
            {
                var res = MessageBox.Show(Application.Current.MainWindow, "Are you sure you want to cancel and discard your changes?", "Confirm cancel", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.No)
                {
                    return;
                }
            }
            this.DialogResult = false;
            this.Close();
        }
    }
}
