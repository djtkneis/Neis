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

        private void CanExecute_ModifyCommand(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = GetDataContext(sender) != null;
        }
        private void Execute_EditCommand(object sender, ExecutedRoutedEventArgs args)
        {
            var product = GetDataContext(sender);
            if (product == null)
            {
                return;
            }

            MessageBox.Show(string.Format("Edit Product clicked for {0}", product.Name));
        }
        private void Execute_DeleteCommand(object sender, ExecutedRoutedEventArgs args)
        {
            var product = GetDataContext(sender);
            if (product == null)
            {
                return;
            }

            MessageBox.Show(string.Format("Delete Product clicked for {0}", product.Name));
        }
    }
}
