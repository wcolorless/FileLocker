using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.IO;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FileLocker.core;
using FileLocker.settings;
using FileLocker.setup;
using Microsoft.Win32;

namespace FileLocker.ui.logic
{
    public class AddStorageWindowVM : INotifyPropertyChanged
    {
        private Window _window;
        public ObservableCollection<FileStorage> _AllResources = new ObservableCollection<FileStorage>();
        private SettingsFile _settingsFile;

        [field: NonSerialized]
        private Command _AddNewStorageCommand;

        [field: NonSerialized]
        private Command _AddExistStorageCommand;

        public string StorageName { get; set; }

        public Command AddNewStorageCommand
        {
            get
            {
                return _AddNewStorageCommand ??= new Command(o =>
                {
                    var fileDialog = new SaveFileDialog() {Filter = "Storage File|*.sf"};
                    if (fileDialog.ShowDialog() == true)
                    {
                        if (_settingsFile.FileStorages == null) _settingsFile.FileStorages = new List<FileStorage>();
                        var storage = new Storage() { Name = StorageName, Files = new List<FileItem>() };
                        var pathToStorage = fileDialog.FileName;
                        StorageLoader.Save(storage, pathToStorage);
                        _AllResources.Add(new FileStorage()
                        {
                            Name = StorageName,
                            Path = pathToStorage
                        });
                        _settingsFile.FileStorages.Clear();
                        _settingsFile.FileStorages.AddRange(_AllResources);
                        SettingsLoader.SaveSettings(_settingsFile);
                        _window.Close();
                    }
                });
            }
        }

        public Command AddExistStorageCommand
        {
            get
            {
                return _AddExistStorageCommand ??= new Command(o =>
                {
                    var fileDialog = new OpenFileDialog() { Filter = "Storage File|*.sf" };
                    if (fileDialog.ShowDialog() == true)
                    {
                        var pathToStorage = fileDialog.FileName;
                        if (_settingsFile.FileStorages == null) _settingsFile.FileStorages = new List<FileStorage>();
                        if (_settingsFile.FileStorages.Exists(x => x.Path == pathToStorage))
                        {
                            MessageBox.Show("A Storage with this path already exists");
                            return;
                        }
                        var storageFileObject = StorageLoader.Load(pathToStorage);
                        _AllResources.Add(new FileStorage()
                        {
                            Name = storageFileObject.Name,
                            Path = pathToStorage
                        });
                        _settingsFile.FileStorages.Clear();
                        _settingsFile.FileStorages.AddRange(_AllResources);
                        SettingsLoader.SaveSettings(_settingsFile);
                        _window.Close();
                    }
                });
            }
        }

        public AddStorageWindowVM(ObservableCollection<FileStorage> allResources, Window window, SettingsFile settingsFile)
        {
            _AllResources = allResources;
            _window = window;
            _settingsFile = settingsFile;
        }

        public static AddStorageWindowVM Create(ObservableCollection<FileStorage> allResources, Window window, SettingsFile settingsFile)
        {
            return new AddStorageWindowVM(allResources, window, settingsFile);
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
