using System.Windows.Controls;
using System.Windows.Input;

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

            this.CommandBindings.Add(new CommandBinding(EditCommand, CommandExecution.Execute_EditProductCommand, CommandExecution.CanExecute_ModifyProductCommand));
            this.CommandBindings.Add(new CommandBinding(DeleteCommand, CommandExecution.Execute_DeleteProductCommand, CommandExecution.CanExecute_ModifyProductCommand));
        }
    }
}
