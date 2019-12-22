using System.Windows.Controls;
using System.Windows.Input;

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

        /// <summary>
        /// Constructor for the KeyControl class
        /// </summary>
        public KeyControl()
        {
            InitializeComponent();

            this.CommandBindings.Add(
                new CommandBinding(CopyToClipboardCommand, CommandExecution.Execute_CopyKeyToCliboardCommand, CommandExecution.CanExecute_CopytKeyToCliboardCommand)
            );
        }
    }
}
