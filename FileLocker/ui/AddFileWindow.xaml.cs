using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FileLocker.core;
using FileLocker.ui.logic;

namespace FileLocker.ui
{
    /// <summary>
    /// Логика взаимодействия для AddFileWindow.xaml
    /// </summary>
    public partial class AddFileWindow : Window
    {
        public AddFileWindow(Storage currentStorage)
        {
            InitializeComponent();
            DataContext = AddFileWindowVM.Create(currentStorage);
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SystemEnvs.Password = string.Empty;
            FileFunctions.ClearFileBufferDir();
            Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
