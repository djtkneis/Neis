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
using System.Windows.Shapes;

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

        public EditProductControl()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(AddKeyCommand, Execute_AddKeyCommand));
        }

        private GenericProduct GetDataContext()
        {
            return DataContext as GenericProduct;
        }

        
        /// <summary>
        /// Occurs when the <see cref="AddKeyCommand"/> is executed
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="args">Arguments for the event.</param>
        private void Execute_AddKeyCommand(object sender, ExecutedRoutedEventArgs args)
        {
            var p = GetDataContext();
            if (p == null)
            {
                return;
            }

            p.Keys.Add(new GenericKey());
        }
    }
}
