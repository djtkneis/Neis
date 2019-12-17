using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

namespace Neis.ProductKeyManager
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
        /// Import Command
        /// </summary>
        public static RoutedCommand ImportCommand = new RoutedCommand("ImportCommand", typeof(MainWindow));
        /// <summary>
        /// Save Command
        /// </summary>
        public static RoutedCommand SaveCommand = new RoutedCommand("SaveCommand", typeof(MainWindow));

        private BackgroundWorker _loadingWorker = new BackgroundWorker();
        private volatile Data.GenericKeyFile _cache = null;
        private volatile Dictionary<string, Data.GenericProduct> _products = new Dictionary<string, Data.GenericProduct>();

        /// <summary>
        /// Constructor for the <see cref="MainWindow"/> class
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(ExitCommand, ExecuteExitCommand));
            CommandBindings.Add(new CommandBinding(ImportCommand, ExecuteImportCommand, IsDoneLoading));
            CommandBindings.Add(new CommandBinding(SaveCommand, ExecuteSaveCommand));

            _loadingWorker.DoWork += _loadingWorker_DoWork;
            _loadingWorker.RunWorkerAsync();
        }

        void _loadingWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!File.Exists(Properties.Settings.Default.DataFile) || new FileInfo(Properties.Settings.Default.DataFile).Length == 0)
            {
                _cache = new Data.GenericKeyFile();
                SaveCache();
            }
            else
            {
                _cache = Data.DataUtility<Data.GenericKeyFile>.LoadFromFile(Properties.Settings.Default.DataFile);
                MergeProducts(_cache.Products);
            }
        }

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
                var import = Data.DataUtility<Data.Microsoft.MicrosoftKeyFile>.LoadFromFile(dlg.FileName);
                int importKeyCount = 0;
                import.Products.ForEach(p => importKeyCount += p.Keys.Count);

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
                    var res = MessageBox.Show(this,
                        string.Format("{0} keys loaded from file.  After merging {1} keys were added.  Do you want to save now?", importKeyCount, keyCount), 
                        "Import successful", 
                        MessageBoxButton.YesNo, 
                        MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        SaveCache();
                    }
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
        }
        /// <summary>
        /// Executes the <see cref="SaveCommand"/>
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="args">Arguments for this event</param>
        private void ExecuteSaveCommand(object sender, ExecutedRoutedEventArgs args)
        {
            SaveCache();
        }

        private int MergeKeys(string product, List<Data.GenericKey> keys)
        {
            int keyCount = 0;
            Data.GenericProduct cp = _cache.Products.Find(p => p.Name == product);
            if (cp == null)
            {
                cp = new Data.GenericProduct()
                {
                    Name = product,
                    Keys = new List<Data.GenericKey>(keys)
                };

                _cache.Products.Add(cp);
                _products.Add(cp.Name, cp);
                keyCount += cp.Keys.Count;
            }
            else
            {
                // Merge key collection for this product
                keys.ForEach(key =>
                {
                    if (!cp.Keys.Any(k => k.Value == key.Value))
                    {
                        cp.Keys.Add(new Data.GenericKey() { Value = key.Value });
                        keyCount++;
                    }
                });

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
        private int MergeProducts(List<Data.Microsoft.MicrosoftProduct> products)
        {
            int keyCount = 0;
            products.ForEach(product =>
            {
                var keys = new List<Data.GenericKey>();
                product.Keys.ForEach(k => keys.Add(new Data.GenericKey()
                {
                    Value = k.Value,
                    ExtraData = Data.DataUtility<Data.Microsoft.MicrosoftKey>.ToXmlString(k)
                }));
                keyCount += MergeKeys(product.Name, keys);
            });
            return keyCount;
        }
        private int MergeProducts(List<Data.GenericProduct> products)
        {
            int keyCount = 0;
            products.ForEach(product =>
            {
                keyCount += MergeKeys(product.Name, product.Keys);
            });
            return keyCount;
        }
        private void SaveCache()
        {
            Data.DataUtility<Data.GenericKeyFile>.SaveToFile(Properties.Settings.Default.DataFile, _cache);
        }
    }
}