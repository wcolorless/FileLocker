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
using FileLocker.settings;
using FileLocker.setup;
using FileLocker.ui.logic;

namespace FileLocker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SettingsFile _settingsFile;
        private readonly PrimeWindowVM _context;
        public MainWindow()
        {
            InitializeComponent();
            _settingsFile = SettingsLoader.GetSettings();
            _context = PrimeWindowVM.Create(_settingsFile, this);
            DataContext = _context;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
