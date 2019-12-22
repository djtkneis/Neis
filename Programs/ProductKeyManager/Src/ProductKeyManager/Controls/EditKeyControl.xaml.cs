using System.Windows.Controls;
using System.Windows.Input;

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

        /// <summary>
        /// Constructor for the EditKeyControl class
        /// </summary>
        public EditKeyControl()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(DeleteKeyCommand, CommandExecution.Execute_DeleteKeyCommand, CommandExecution.CanExecute_ModifyKeyCommand));
        }
    }
}
