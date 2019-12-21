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
    /// Interaction logic for KeyControl.xaml
    /// </summary>
    public partial class KeyControl : UserControl
    {
        /// <summary>
        /// CopyToClipboard Command
        /// </summary>
        public static RoutedCommand CopyToClipboardCommand = new RoutedCommand("CopyToClipboardCommand", typeof(KeyControl));

        public KeyControl()
        {
            InitializeComponent();

            this.CommandBindings.Add(new CommandBinding(CopyToClipboardCommand, Execute_CopyToCliboardCommand, CanExecute_CopytToCliboardCommand));
        }

        private GenericKey GetDataContext(object sender)
        {
            var control = sender as KeyControl;
            if (control == null)
            {
                return null;
            }

            var key = control.DataContext as GenericKey;
            return key;
        }
        

        private void CanExecute_CopytToCliboardCommand(object sender, CanExecuteRoutedEventArgs args)
        {
            GenericKey val = GetDataContext(sender);
            args.CanExecute = val != null && !string.IsNullOrWhiteSpace(val.Value);
        }

        private void Execute_CopyToCliboardCommand(object sender, ExecutedRoutedEventArgs args)
        {
            var key = GetDataContext(sender);
            if (key == null)
            {
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
    }
}
