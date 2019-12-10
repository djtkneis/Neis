using Neis.FileCleanup.Configuration;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Neis.FileCleanupTool
{
    /// <summary>
    /// Interaction logic for DirectoryConfigControl.xaml
    /// </summary>
    public partial class DirectoryConfigControl : UserControl
    {
        /// <summary>
        /// Property name for the CleanupActions property
        /// </summary>
        public static readonly string CleanupActionsPropertyName = "CleanupActions";
        /// <summary>
        /// Dependency property for the CleanupActions property
        /// </summary>
        public static DependencyProperty CleanupActionsProperty = DependencyProperty.Register(CleanupActionsPropertyName, typeof(List<CleanupAction>), typeof(DirectoryConfigControl));
        /// <summary>
        /// Gets or sets the CleanupActions
        /// </summary>
        public List<CleanupAction> CleanupActions
        {
            get
            {
                return (List<CleanupAction>)this.GetValue(CleanupActionsProperty);
            }
            set { this.SetValue(CleanupActionsProperty, value); }
        }

        /// <summary>
        /// Constructor for the <see cref="DirectoryConfigControl"/> class
        /// </summary>
        public DirectoryConfigControl()
        {
            CleanupActions = new List<CleanupAction>();

            Array actions = Enum.GetValues(typeof(CleanupAction));
            for (int i = 0; i < actions.Length; i++)
            {
                CleanupActions.Add((CleanupAction)actions.GetValue(i));
            }

            InitializeComponent();
        }
    }
}