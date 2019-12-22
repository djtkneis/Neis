using Microsoft.Win32;
using Neis.ProductKeyManager.Controls;
using Neis.ProductKeyManager.Data;
using Neis.ProductKeyManager.Data.Microsoft;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Neis.ProductKeyManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// AddProduct Command
        /// </summary>
        public static RoutedCommand AddProductCommand = new RoutedCommand("AddProductCommand", typeof(MainWindow));
        /// <summary>
        /// Exit Command
        /// </summary>
        public static RoutedCommand ExitCommand = new RoutedCommand("ExitCommand", typeof(MainWindow));
        /// <summary>
        /// Import Command
        /// </summary>
        public static RoutedCommand ImportCommand = new RoutedCommand("ImportCommand", typeof(MainWindow));
        /// <summary>
        /// Save Command
        /// </summary>
        public static RoutedCommand SaveCommand = new RoutedCommand("SaveCommand", typeof(MainWindow));

        #region KeyFile
        /// <summary>
        /// Dependency property declaration for the KeyFile property
        /// </summary>
        public static readonly DependencyProperty KeyFileProperty =
            DependencyProperty.Register("KeyFile", typeof(GenericKeyFile), typeof(MainWindow), new UIPropertyMetadata(new GenericKeyFile()));

        /// <summary>
        /// Gets or sets the KeyFile property
        /// </summary>
        public GenericKeyFile KeyFile
        {
            get { return GetKeyFile(); }
            set { this.SetKeyFile(value); }
        }
        private GenericKeyFile GetKeyFile()
        {
            if (!Dispatcher.CheckAccess())
            {
                return Dispatcher.Invoke(new Func<GenericKeyFile>(this.GetKeyFile)) as GenericKeyFile;
            }
            return (GenericKeyFile)this.GetValue(KeyFileProperty);
        }
        private void SetKeyFile(GenericKeyFile value)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<GenericKeyFile>(this.SetKeyFile), value);
                return;
            }
            this.SetValue(KeyFileProperty, value);
        }
        #endregion

        #region ProductsView
        /// <summary>
        /// Dependency property declaration for the ProductsView property
        /// </summary>
        public static readonly DependencyProperty ProductsViewProperty =
            DependencyProperty.Register("ProductsView", typeof(ICollectionView), typeof(MainWindow), new UIPropertyMetadata());
        
        /// <summary>
        /// Gets or sets the ProductsView property
        /// </summary>        
        public ICollectionView ProductsView
        {
            get { return GetProductsView(); }
            set { this.SetProductsView(value); }
        }
        private ICollectionView GetProductsView()
        {
            if (!Dispatcher.CheckAccess())
            {
                return Dispatcher.Invoke(new Func<ICollectionView>(this.GetProductsView)) as ICollectionView;
            }
            return (ICollectionView)this.GetValue(ProductsViewProperty);
        }
        private void SetProductsView(ICollectionView value)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<ICollectionView>(this.SetProductsView), value);
                return;
            }
            this.SetValue(ProductsViewProperty, value);
        }
        #endregion

        private delegate int MergeKeysDelegate(string product, ObservableCollection<GenericKey> keys);
        private delegate int MergeGenericProductsDelegate(ObservableCollection<GenericProduct> products);
        private delegate int MergeMicrosoftProductsDelegate(ObservableCollection<MicrosoftProduct> products);

        private bool _isLoading = false;
        private BackgroundWorker _loadingWorker = new BackgroundWorker();

        /// <summary>
        /// Constructor for the <see cref="MainWindow"/> class
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(AddProductCommand, CommandExecution.Execute_AddProductCommand));
            CommandBindings.Add(new CommandBinding(ExitCommand, ExecuteExitCommand));
            CommandBindings.Add(new CommandBinding(ImportCommand, ExecuteImportCommand, IsDoneLoading));
            CommandBindings.Add(new CommandBinding(SaveCommand, ExecuteSaveCommand));

            KeyFile = new GenericKeyFile();
            ProductsView = CollectionViewSource.GetDefaultView(KeyFile.Products);
            ProductsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

            this.SetStatus("Initializing stored keys");

            _loadingWorker.DoWork += _loadingWorker_DoWork;
            _loadingWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Occurs when the MainWindow is about to close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            // Save Cache if dirty
            if (KeyFile.IsDirty)
            {
                SaveCache();
            }

            // check if log file exists
            var filename = Neis.ProductKeyManager.Properties.Settings.Default.LogFile;
            var max = Neis.ProductKeyManager.Properties.Settings.Default.LogFileMaxSizeMB * 1024 * 1024;
            if (File.Exists(filename))
            {
                // if logfile is greater than the max size, keep deleting just the first line until the size fits within the restraints
                var info = new FileInfo(filename);
                while (info.Length > max)
                {
                    var diff = info.Length - max;
                    var lines = File.ReadAllLines(filename);
                    var skipLines = 0;
                    while (diff > 0)
                    {
                        diff -= (lines[skipLines++].Length + 2); // 2 = length of Windows line endings '/n/r'
                    }
                    File.WriteAllLines(filename, lines.Skip(skipLines + 1).ToArray());
                    info = new FileInfo(filename);
                }
            }
        }

        /// <summary>
        /// Event that occurs when the background worker that loads data is running
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="e">Arguments for this event</param>
        void _loadingWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (File.Exists(Properties.Settings.Default.DataFile) && new FileInfo(Properties.Settings.Default.DataFile).Length > 0)
            {
                var kf = DataUtility<GenericKeyFile>.LoadFromFile(Properties.Settings.Default.DataFile);
                MergeGenericProducts(kf.Products);
            }

            KeyFile.MarkNotDirty();
            KeyFile.OnIsDirty += KeyFile_OnIsDirty;
            SetDefaultStatus();
        }


        /// <summary>
        /// Event for when the <see cref="KeyFile"/> has been marked as dirty
        /// </summary>
        /// <param name="obj">Object that has been marked dirty</param>
        private void KeyFile_OnIsDirty(NotifiableBase obj)
        {
            SaveCache();
            KeyFile.MarkNotDirty();
        }

        /// <summary>
        /// Determines whether or not a command can execute based on whether or not the program is still loading
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="args">Arguements for this event</param>
        private void IsDoneLoading(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = !_loadingWorker.IsBusy;
        }

        /// <summary>
        /// Executes the <see cref="ExitCommand"/>
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="args">Arguments for this event</param>
        private void ExecuteExitCommand(object sender, ExecutedRoutedEventArgs args)
        {
            Close();
        }
        /// <summary>
        /// Executes the <see cref="ImportCommand"/>
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="args">Arguments for this event</param>
        private void ExecuteImportCommand(object sender, ExecutedRoutedEventArgs args)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files (*.*)|*.*|XML Files (*.xml)|*.xml";
            dlg.FilterIndex = 2;

            bool? dlgRes = dlg.ShowDialog(this);
            if (dlgRes != null && dlgRes.Value)
            {
                var import = DataUtility<MicrosoftKeyFile>.LoadFromFile(dlg.FileName);
                int importKeyCount = 0;
                foreach (var p in import.Products)
                {
                    importKeyCount += p.Keys.Count;
                }

                if (importKeyCount == 0)
                {
                    var res = MessageBox.Show(
                        Application.Current.MainWindow,
                        "The file did not contain any keys to load.",
                        "No keys loaded",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }

                var keyCount = MergeMicrosoftProducts(import.Products);
                if (keyCount > 0)
                {
                    SaveCache();
                    MessageBox.Show(
                        Application.Current.MainWindow,
                        string.Format("{0} keys loaded from file.  After merging {1} keys were added.", importKeyCount, keyCount),
                        "Import successful",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    SetDefaultStatus();
                }
                else
                {
                    var res = MessageBox.Show(
                        Application.Current.MainWindow,
                        string.Format("{0} keys loaded from file however, 0 keys were added.  Most likely the keys were already present", importKeyCount),
                        "No keys added",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                }
            }

            SetDefaultStatus();
        }
        /// <summary>
        /// Executes the <see cref="SaveCommand"/>
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="args">Arguments for this event</param>
        private void ExecuteSaveCommand(object sender, ExecutedRoutedEventArgs args)
        {
            SaveCache();
            SetStatus("Save complete");
        }


        /// <summary>
        /// Merge a list of keys into a given product
        /// </summary>
        /// <param name="product">Name of product to merge keys list into</param>
        /// <param name="keys">List of <see cref="GenericKey"/> objects to merge</param>
        /// <returns>Number of keys that were added</returns>
        public int MergeKeys(string product, ObservableCollection<GenericKey> keys)
        {
            if (!Dispatcher.CheckAccess())
            {
                return (int)Dispatcher.Invoke(new MergeKeysDelegate(MergeKeys), product, keys);
            }

            _isLoading = true;
            int keyCount = 0;

            try
            {
                GenericProduct cp = null;
                foreach (var p in KeyFile.Products)
                {
                    if (p.Name == product)
                        cp = p;
                }
                if (cp == null)
                {
                    cp = new GenericProduct()
                    {
                        Name = product,
                        Keys = new ObservableCollection<GenericKey>(keys)
                    };

                    KeyFile.Products.Add(cp);
                    keyCount += cp.Keys.Count;
                }
                else
                {
                    // Merge key collection for this product
                    foreach (var key in keys)
                    {
                        if (!cp.Keys.Any(k => k.Value == key.Value))
                        {
                            cp.Keys.Add(new GenericKey() { Value = key.Value });
                            keyCount++;
                        }
                    }
                }
            }
            finally
            {
                _isLoading = false;
            }
            return keyCount;
        }
        /// <summary>
        /// Merge a list of products
        /// </summary>
        /// <param name="products">List of <see cref="GenericProduct"/> objects to merge</param>
        /// <returns>Number of keys that were added</returns>
        private int MergeGenericProducts(ObservableCollection<GenericProduct> products)
        {
            if (!Dispatcher.CheckAccess())
            {
                return (int)Dispatcher.Invoke(new MergeGenericProductsDelegate(MergeGenericProducts), products);
            }

            int keyCount = 0;
            foreach (var product in products)
            {
                keyCount += MergeKeys(product.Name, product.Keys);
            }
            return keyCount;
        }
        /// <summary>
        /// Merge a list of products
        /// </summary>
        /// <param name="products">List of <see cref="MicrosoftProduct"/> objects to merge</param>
        /// <returns>Number of keys that were added</returns>
        private int MergeMicrosoftProducts(ObservableCollection<MicrosoftProduct> products)
        {
            if (!Dispatcher.CheckAccess())
            {
                return (int)Dispatcher.Invoke(new MergeMicrosoftProductsDelegate(MergeMicrosoftProducts), products);
            }

            int keyCount = 0;
            foreach (var product in products)
            {
                var keys = new ObservableCollection<GenericKey>();
                foreach (var k in product.Keys)
                {
                    keys.Add(new GenericKey()
                    {
                        Value = k.Value,
                        ExtraData = new ExtraData()
                        {
                            Type = typeof(MicrosoftKey).ToString(),
                            Data = DataUtility<MicrosoftKey>.ToJsonString(k)
                        }
                    });
                }
                keyCount += MergeKeys(product.Name, keys);
            }
            return keyCount;
        }


        /// <summary>
        /// Saves the current cache
        /// </summary>
        private void SaveCache()
        {
            if (!_isLoading)
            {
                DataUtility<GenericKeyFile>.SaveToFile(Properties.Settings.Default.DataFile, KeyFile);
            }
        }

        /// <summary>
        /// Sets a given status message
        /// </summary>
        /// <param name="message"></param>
        private void SetStatus(string message)
        {
            //TODO: Add a timer that will change the status message back to the default message after a given interval

            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<string>(this.SetStatus), message);
                return;
            }
            this.txtStatusBar.Text = message;
        }
        /// <summary>
        /// Sets the default status message
        /// </summary>
        private void SetDefaultStatus()
        {
            int keycount = 0;
            foreach (var p in KeyFile.Products)
            { keycount += p.Keys.Count; }

            SetStatus(string.Format("{0} Products and {1} keys", KeyFile.Products.Count, keycount));
        }
    }
}