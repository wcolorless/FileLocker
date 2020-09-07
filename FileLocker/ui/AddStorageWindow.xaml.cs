using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FileLocker.settings;
using FileLocker.ui.logic;

namespace FileLocker.ui
{
    /// <summary>
    /// Логика взаимодействия для AddStorageWindow.xaml
    /// </summary>
    public partial class AddStorageWindow : Window
    {
        public ObservableCollection<FileStorage> _AllResources;
        private SettingsFile _settingsFile;
        public AddStorageWindow(ObservableCollection<FileStorage> allResources, SettingsFile settingsFile)
        {
            InitializeComponent();
            _AllResources = allResources;
            _settingsFile = settingsFile;
            DataContext = AddStorageWindowVM.Create(allResources, this, _settingsFile);
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
