using System;
using System.Collections.Generic;
using System.Security;
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

namespace FileLocker.ui
{
    /// <summary>
    /// Логика взаимодействия для EnterPasswordWindow.xaml
    /// </summary>
    public partial class EnterPasswordWindow : Window
    {
        public EnterPasswordWindow(string storageName)
        {
            InitializeComponent();
            StorageName.Text = storageName;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;
                case Key.Space when string.IsNullOrEmpty(StoragePasswordBox.Password):
                    MessageBox.Show("Enter Your Password!");
                    break;
                case Key.Space:
                    SystemEnvs.Password = StoragePasswordBox.Password;
                    DialogResult = true;
                    break;
            }
        }
    }
}
