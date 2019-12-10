using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProductKeyManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Exit Command
        /// </summary>
        public static RoutedCommand ExitCommand = new RoutedCommand("ExitCommand", typeof(MainWindow));

        /// <summary>
        /// Constructor for the <see cref="MainWindow"/> class
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            this.CommandBindings.Add(new CommandBinding(ExitCommand, ExecuteExitCommand));
        }

        /// <summary>
        /// Executes the <see cref="ExitCommand"/>
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="args">Arguments for this event</param>
        private void ExecuteExitCommand(object sender, ExecutedRoutedEventArgs args)
        {
            this.Close();
        }
    }
}