using Microsoft.Win32;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
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

        #region INotifyPropertyChanged
        /// <summary>
        /// Event for when a property has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="property">Name of property that changed</param>
        protected void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
            }
        }
        #endregion

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
        private void SetKeyFile(GenericKeyFile kf)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<GenericKeyFile>(this.SetKeyFile), kf);
                return;
            }
            this.SetValue(KeyFileProperty, kf);
            this._productsViewSource.Source = kf.Products;
            this.Products.Refresh();
        }
        #endregion

        #region Products
        /// <summary>
        /// Dependency property declaration for the Products property
        /// </summary>
        public static readonly DependencyProperty ProductsProperty = DependencyProperty.Register("Products", typeof(ICollectionView), typeof(MainWindow), new UIPropertyMetadata());

        /// <summary>
        /// Gets or sets the Products property
        /// </summary>
        public ICollectionView Products
        {
            get { return GetProducts(); }
        }
        private ICollectionView GetProducts()
        {
            if (!Dispatcher.CheckAccess())
            {
                return Dispatcher.Invoke(new Func<ICollectionView>(this.GetProducts)) as ICollectionView;
            }
            return (ICollectionView)this._productsViewSource.View;
        }
        #endregion

        private CollectionViewSource _productsViewSource = new CollectionViewSource();
        private BackgroundWorker _loadingWorker = new BackgroundWorker();
        private volatile Dictionary<string, GenericProduct> _products = new Dictionary<string, GenericProduct>();

        /// <summary>
        /// Constructor for the <see cref="MainWindow"/> class
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(ExitCommand, ExecuteExitCommand));
            CommandBindings.Add(new CommandBinding(ImportCommand, ExecuteImportCommand, IsDoneLoading));
            CommandBindings.Add(new CommandBinding(SaveCommand, ExecuteSaveCommand));

            this.SetStatus("Initializing stored keys");

            _loadingWorker.DoWork += _loadingWorker_DoWork;
            _loadingWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Event that occurs when the background worker that loads data is running
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="e">Arguments for this event</param>
        void _loadingWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!File.Exists(Properties.Settings.Default.DataFile) || new FileInfo(Properties.Settings.Default.DataFile).Length == 0)
            {
                KeyFile = new GenericKeyFile();
                SaveCache();
            }
            else
            {
                KeyFile = DataUtility<GenericKeyFile>.LoadFromFile(Properties.Settings.Default.DataFile);
                MergeProducts(KeyFile.Products);
            }

            SetDefaultStatus();
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
                    var res = MessageBox.Show(this,
                        "The file did not contain any keys to load.",
                        "No keys loaded",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }

                var keyCount = MergeProducts(import.Products);
                if (keyCount > 0)
                {
                    SaveCache();
                    MessageBox.Show(this,
                        string.Format("{0} keys loaded from file.  After merging {1} keys were added.", importKeyCount, keyCount),
                        "Import successful",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    SetDefaultStatus();
                }
                else
                {
                    var res = MessageBox.Show(this,
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
        private int MergeKeys(string product, ObservableCollection<GenericKey> keys)
        {
            int keyCount = 0;
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
                _products.Add(cp.Name, cp);
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

                // Update Hash Table
                if (!_products.ContainsKey(product))
                {
                    // in theory, this should never happen if we are properly keeping the dictionary in sync with the cache
                    _products.Add(product, cp);
                }
                else
                {
                    _products[product] = cp;
                }
            }

            return keyCount;
        }
        /// <summary>
        /// Merge a list of products
        /// </summary>
        /// <param name="products">List of <see cref="GenericProduct"/> objects to merge</param>
        /// <returns>Number of keys that were added</returns>
        private int MergeProducts(ObservableCollection<GenericProduct> products)
        {
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
        private int MergeProducts(ObservableCollection<MicrosoftProduct> products)
        {
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
            DataUtility<GenericKeyFile>.SaveToFile(Properties.Settings.Default.DataFile, KeyFile);
        }

        private void SetDefaultStatus()
        {
            int keycount = 0;
            foreach (var p in KeyFile.Products)
            { keycount += p.Keys.Count; }

            SetStatus(string.Format("{0} Products and {1} keys", KeyFile.Products.Count, keycount));
        }

        private void SetStatus(string message)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<string>(this.SetStatus), message);
                return;
            }
            this.txtStatusBar.Text = message;
        }
    }
}