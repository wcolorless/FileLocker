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

namespace FileLocker.ui.logic
{
    public class PrimeWindowVM : INotifyPropertyChanged
    {

        private Window _primeWindow;
        public ObservableCollection<FileStorage> _AllResources = new ObservableCollection<FileStorage>();
        private SettingsFile _settingsFile;
        public FileStorage SelectedResource { get; set; }
        string LastResourcesInString = "";

        [field: NonSerialized]
        private ListBox _ResourceListBox;

        [field: NonSerialized]
        private Window _PrimeWindowWindow;

        [field: NonSerialized]
        private Command _OpenProfileWindowCommand;

        [field: NonSerialized]
        private Command _RemoveSelectedResource;

        [field: NonSerialized]
        private Command _ShowAddNewStorageWindowCommand;

        [field: NonSerialized] 
        private Command _OpenStorage;


        public ObservableCollection<FileStorage> AllResources
        {
            get { return _AllResources; }
        }

        public Command RemoveSelectedResource
        {
            get
            {
                return _RemoveSelectedResource ??= new Command(o =>
                {
                    if(SelectedResource != null)
                    {
                        if (_AllResources.Contains(SelectedResource))
                        {
                            _settingsFile.FileStorages.RemoveAll(x => x.Path == SelectedResource.Path);
                            _AllResources.Remove(SelectedResource);
                            SelectedResource = null;
                            SettingsLoader.SaveSettings(_settingsFile);
                        }
                    }
                });
            }
        }


        public Command ShowAddNewStorageWindowCommand
        {
            get
            {
                return _ShowAddNewStorageWindowCommand ??= new Command(o =>
                {
                    var window_AddStorage = new AddStorageWindow(_AllResources, _settingsFile) {Owner = _primeWindow};
                    window_AddStorage.ShowDialog();
                });
            }
        }

        public Command OpenStorage
        {
            get
            {
                return _OpenStorage ??= new Command(o =>
                {
                    if (SelectedResource == null)
                    {
                        MessageBox.Show("Select your storage and repeat");
                        return;
                    }
                    var enterPassWindow = new EnterPasswordWindow(SelectedResource.Name) {Owner = _primeWindow};
                    var result = enterPassWindow.ShowDialog();
                    if (result == true)
                    {
                        var path = SelectedResource.Path;
                        SystemEnvs.CurrentStoragePath = path;
                        var storage = StorageLoader.Load(path);
                        var addFileWindow = new AddFileWindow(storage) { Owner = _primeWindow};
                        addFileWindow.ShowDialog();
                    }
                });
            }
        }

        public Window PrimeWindowWindow
        {
            set
            {
                _PrimeWindowWindow = value;
            }
        }

        public PrimeWindowVM(SettingsFile settingsFile, Window primeWindow)
        {
            settingsFile?.FileStorages?.ForEach(x => _AllResources.Add(x));
            _primeWindow = primeWindow;
            _settingsFile = settingsFile;
        }

        public static PrimeWindowVM Create(SettingsFile settingsFile, Window primeWindow)
        {
            return new PrimeWindowVM(settingsFile, primeWindow);
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
