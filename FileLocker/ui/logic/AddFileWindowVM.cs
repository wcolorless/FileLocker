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
using Path = System.IO.Path;

namespace FileLocker.ui.logic
{
    public class AddFileWindowVM : INotifyPropertyChanged
    {
        public Storage CurrentStorage;
        public ObservableCollection<FileItem> _Files = new ObservableCollection<FileItem>();
        public FileItem SelectedFile { get; set; }

        private SettingsFile _settingsFile;

        [field: NonSerialized]
        private Command _AddFileCommand;

        [field: NonSerialized]
        private Command _RemoveFileCommand;

        [field: NonSerialized]
        private Command _OpenFileCommand;

        [field: NonSerialized]
        private Command _SaveFileToCommand;

        public ObservableCollection<FileItem> Files
        {
            get { return _Files; }
        }

        public string StorageName { get; set; }

        public Command AddFileCommand
        {
            get
            {
                return _AddFileCommand ??= new Command(o =>
                {
                    var openFile = new OpenFileDialog();
                    if (openFile.ShowDialog() == true)
                    {
                        var fileName = Path.GetFileName(openFile.FileName);
                        var exits = CurrentStorage.Files.Exists(x => x.FileName == fileName);
                        if (exits)
                        {
                           var result = MessageBox.Show("Файл уже существует", "Ok");
                           return;
                        }
                        else
                        {
                            byte[] bytes;
                            using (var fs = new FileStream(openFile.FileName, FileMode.Open, FileAccess.Read))
                            {
                                bytes = new byte[fs.Length];
                                fs.Read(bytes, 0, (int)fs.Length);
                                fs.Close();
                            }

                            var blocks = new List<FileBlock>();
                            var encodedFileData = FileFunctions.Encode(bytes, SystemEnvs.Password);
                            if (encodedFileData == null) return;
                            blocks.Add(new FileBlock()
                            {
                                Array = encodedFileData
                            });
                            CurrentStorage.Files.Add(new FileItem()
                            {
                                FileName = fileName,
                                Id = Guid.NewGuid().ToString(),
                                FileBlocks = blocks
                            });
                            if(!string.IsNullOrEmpty(SystemEnvs.CurrentStoragePath)) StorageLoader.Save(CurrentStorage, SystemEnvs.CurrentStoragePath);
                            else throw new Exception($"AddFileWindowVM AddFileCommand Error: CurrentStoragePath is null");
                            UpdateList(CurrentStorage);
                        }
                    }
                });
            }
        }

        public Command RemoveFileCommand
        {
            get
            {
                return _RemoveFileCommand ??= new Command(o =>
                {
                    if (SelectedFile != null)
                    {
                        var file = CurrentStorage.Files.FirstOrDefault(x => x.FileName == SelectedFile.FileName);
                        if (file != null)
                        {
                            CurrentStorage.Files.Remove(file);
                            if (!string.IsNullOrEmpty(SystemEnvs.CurrentStoragePath)) StorageLoader.Save(CurrentStorage, SystemEnvs.CurrentStoragePath);
                            else throw new Exception($"AddFileWindowVM AddFileCommand Error: CurrentStoragePath is null");
                            UpdateList(CurrentStorage);
                        }
                        else throw new Exception($"AddFileWindowVM RemoveFileCommand Error: file is null");
                    }
                });
            }
        }

        public Command OpenFileCommand
        {
            get
            {
                return _OpenFileCommand ??= new Command(o =>
                {
                    try
                    {
                        if (SelectedFile != null)
                        {
                            var fileData = FileItem.GetBytes(SelectedFile);
                            var openedFilePath = FileFunctions.OpenFile(FileFunctions.Decode(fileData, SystemEnvs.Password), SelectedFile.FileName);
                            FileHelper.OpenFileInFolder(openedFilePath);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"AddFileWindowVM OpenFileCommand Error: {e.Message}");
                    }
                });
            }
        }

        public Command SaveFileToCommand
        {
            get
            {
                return _SaveFileToCommand ??= new Command(o =>
                {
                    try
                    {
                        if (SelectedFile != null)
                        {
                            var saveFilePath = new SaveFileDialog()
                            {
                                FileName = $"{SelectedFile.FileName}", 
                                Filter = $"File|{Path.GetExtension(SelectedFile.FileName)}"
                            };
                            if (saveFilePath.ShowDialog() == true)
                            {
                                var fileData = FileItem.GetBytes(SelectedFile);
                                var decodedFileData = FileFunctions.Decode(fileData, SystemEnvs.Password);
                                FileHelper.SaveFileToDir(decodedFileData, saveFilePath.FileName);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"AddFileWindowVM OpenFileCommand Error: {e.Message}");
                    }
                });
            }
        }

        private void UpdateList(Storage currentStorage)
        {
            if (currentStorage.Files == null) currentStorage.Files = new List<FileItem>();
            _Files.Clear();
            currentStorage.Files.ForEach(x =>
            {
                _Files.Add(x);
            });
        }

        public AddFileWindowVM(Storage currentStorage)
        {
            CurrentStorage = currentStorage;
            UpdateList(currentStorage);
        }

        public static AddFileWindowVM Create(Storage currentStorage)
        {
            return new AddFileWindowVM(currentStorage);
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
