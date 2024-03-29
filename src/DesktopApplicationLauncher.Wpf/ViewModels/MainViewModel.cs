﻿namespace DesktopApplicationLauncher.Wpf.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    using Commands;

    using HtmlAgilityPack;

    using Infrastructure.Business;
    using Infrastructure.Entities;
    using Infrastructure.Extensions;
    using Infrastructure.Models;

    using Microsoft.Win32;

    using Application = System.Windows.Application;

    public sealed class MainViewModel : ViewModelBase
    {
        private static readonly HttpClient HttpClient = new();

        private readonly Window _ownerWindow;

        private readonly IApplicationService _applicationService;

        private ObservableCollection<ApplicationListItemModel> _apps;

        [SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Reviewed")]
        public ObservableCollection<ApplicationListItemModel> Apps
        {
            get => _apps;
            set
            {
                if (ReferenceEquals(_apps, value))
                {
                    return;
                }

                _apps = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ParentFolderModel> _folders;

        [SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Reviewed")]
        public ObservableCollection<ParentFolderModel> Folders
        {
            get => _folders;
            set
            {
                if (ReferenceEquals(_folders, value))
                {
                    return;
                }

                _folders = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ListItemModel<ApplicationItemType>> _appTypes;

        [SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Reviewed")]
        public ObservableCollection<ListItemModel<ApplicationItemType>> AppTypes
        {
            get => _appTypes;
            set
            {
                if (ReferenceEquals(_appTypes, value))
                {
                    return;
                }

                _appTypes = value;
                OnPropertyChanged();
            }
        }

        private ApplicationListItemModel _selectedApp;

        public ApplicationListItemModel SelectedApp
        {
            get => _selectedApp;
            set
            {
                if (ReferenceEquals(_selectedApp, value))
                {
                    return;
                }

                _selectedApp = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsInInsertMode));
                OnPropertyChanged(nameof(IsFileType));
                OnPropertyChanged(nameof(ItemType));
                OnPropertyChanged(nameof(ArgumentsWatermark));
            }
        }

        private int _appViewWidth;

        public int AppViewWidth
        {
            get => _appViewWidth;
            set
            {
                if (value == _appViewWidth)
                {
                    return;
                }

                _appViewWidth = value;
                OnPropertyChanged();
            }
        }

        public bool IsInInsertMode => SelectedApp is {Id: 0};

        public bool IsFileType => SelectedApp is {ItemType: ApplicationItemType.File};

        public ApplicationItemType ItemType => SelectedApp?.ItemType ?? ApplicationItemType.File;

        public string ArgumentsWatermark
        {
            get
            {
                return ItemType switch
                {
                    ApplicationItemType.Website => "Url",
                    _ => "Arguments",
                };
            }
        }

        public int? ParentId { get; set; }

        public ICommand DeleteSelectedAppCommand { get; }

        public ICommand DeleteAppCommand { get; }

        public ICommand CloseAppViewCommand { get; }

        public ICommand AddAppCommand { get; }

        public ICommand AddAppPathCommand { get; }

        public ICommand SaveAppCommand { get; }

        public ICommand OpenAppCommand { get; }

        public ICommand SwapAppsCommand { get; }

        public ICommand ConvertToFolderCommand { get; }

        public ICommand FolderChangeCommand { get; }

        public ICommand AppsMoveCommand { get; }

        public MainViewModel(Window ownerWindow, IApplicationService applicationService)
        {
            _ownerWindow = ownerWindow ?? throw new ArgumentNullException(nameof(ownerWindow));
            _applicationService = applicationService ?? throw new ArgumentNullException(nameof(applicationService));

            AddAppCommand = new RelayCommand(AddApp);
            OpenAppCommand = new RelayCommand(OpenApp);
            DeleteAppCommand = new RelayCommand(DeleteApp);
            DeleteSelectedAppCommand = new RelayCommand(DeleteApp, _ => SelectedApp?.Id > 0);
            AddAppPathCommand = new RelayCommand(AddAppPath, _ => SelectedApp != null);
            SaveAppCommand = new RelayCommand(SaveApp, _ => SelectedApp != null);
            CloseAppViewCommand = new RelayCommand(_ => CloseAppView());
            SwapAppsCommand = new RelayCommand(SwapApps);
            AppsMoveCommand = new RelayCommand(AppsMove);
            ConvertToFolderCommand = new RelayCommand(ConvertToFolder, CanConvertToFolder);
            FolderChangeCommand = new RelayCommand(ChangeFolder);

            LoadAllApps();

            _appTypes = new ObservableCollection<ListItemModel<ApplicationItemType>>(ListItemExtensions.GetListItemsOfEnum<ApplicationItemType>().OrderBy(x => (int)x.Value));
        }

        private void AppsMove(object parameter)
        {
            var (sourceIndex, targetIndex, itemIsOver, isStopped) = ((int, int, bool, bool))parameter;

            var target = Apps[targetIndex];
            target.IsHighlighted = target.ItemType == ApplicationItemType.Folder && itemIsOver && !isStopped;

            if (isStopped && itemIsOver && target.ItemType == ApplicationItemType.Folder)
            {
                _applicationService.MoveToFolder(Apps[sourceIndex].Id, target.Id);

                LoadAllApps();
            }
        }

        private void ChangeFolder(object parameter)
        {
            if (parameter is ParentFolderModel model)
            {
                LoadFolderApps(model.Id == 0 ? null : model.Id);
            }
        }

        private void LoadFolderApps(int? parentId)
        {
            ParentId = parentId;
            LoadAllApps();
        }

        private static bool CanConvertToFolder(object parameter)
        {
            if (parameter is ApplicationListItemModel model)
            {
                return model.ItemType != ApplicationItemType.Folder;
            }

            return false;
        }

        private void ConvertToFolder(object parameter)
        {
            if (parameter is ApplicationListItemModel model)
            {
                _applicationService.ConvertToFolder(model.Id, model.Name);

                LoadAllApps();
            }
        }

        private void SwapApps(object parameter)
        {
            var (sourceIndex, targetIndex) = ((int, int))parameter;

            var target = Apps[targetIndex];

            Apps[targetIndex] = Apps[sourceIndex];

            if (sourceIndex < targetIndex)
            {
                for (var i = targetIndex - 1; i >= sourceIndex; i--)
                {
                    (Apps[i], target) = (target, Apps[i]);
                }
            }
            else
            {
                for (var i = targetIndex + 1; i <= sourceIndex; i++)
                {
                    (Apps[i], target) = (target, Apps[i]);
                }
            }

            var startIndex = Math.Min(sourceIndex, targetIndex);
            var endIndex = Math.Max(sourceIndex, targetIndex);
            for (var i = startIndex; i <= endIndex; i++)
            {
                var item = Apps[i];
                _applicationService.UpdateApplicationOrder(item.Id, i);
            }
        }

        private void AddApp(object parameter)
        {
            SelectedApp = parameter as ApplicationListItemModel ?? CreateAddApp();
            SelectedApp.PropertyChanged -= LoadWebsiteNameAndDescriptionEventHandler;
            SelectedApp.PropertyChanged += LoadWebsiteNameAndDescriptionEventHandler;
            AppViewWidth = 250;
        }

        private void SaveApp(object parameter)
        {
            SaveAppToDb();
            CloseAppView();
            LoadAllApps();
        }

        private void DeleteApp(object parameter)
        {
            var selectedApp = parameter as ApplicationListItemModel ?? SelectedApp;
            if (selectedApp == null)
            {
                return;
            }

            _applicationService.DeleteApp(selectedApp.Id);

            SelectedApp = null;
            CloseAppView();
            LoadAllApps();
        }

        [SuppressMessage("Microsoft.Design", "CA1031: Do not catch general exception types", Justification = "Reviewed")]
        private void OpenApp(object parameter)
        {
            if (parameter is ApplicationListItemModel appItem)
            {
                appItem.LastAccessedDate = _applicationService.UpdateApplicationLastAccessDate(appItem.Id);

                if (appItem.ItemType == ApplicationItemType.Folder)
                {
                    LoadFolderApps(appItem.Id);
                }
                else if (appItem.ItemType == ApplicationItemType.Website)
                {
                    try
                    {
                        using var _ = Process.Start(new ProcessStartInfo(appItem.Arguments) {UseShellExecute = true});
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(_ownerWindow, $"Website Open Error!!{Environment.NewLine}{exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    try
                    {
                        using var _ = Process.Start(
                            new ProcessStartInfo(appItem.Path)
                                {
                                    Arguments = appItem.Arguments,
                                    UseShellExecute = true,
                                    WorkingDirectory = Path.GetDirectoryName(appItem.Path)!
                                });
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(_ownerWindow, $"File Open Error!!{Environment.NewLine}{exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void SaveAppToDb()
        {
            var selectedApp = SelectedApp;
            _applicationService.SaveApplication(new ApplicationSaveModel
                                                    {
                                                        Id = selectedApp.Id,
                                                        ParentId = ParentId,
                                                        Name = selectedApp.Name,
                                                        ItemType = selectedApp.ItemType,
                                                        Description = selectedApp.Description,
                                                        Arguments = selectedApp.Arguments,
                                                        Path = selectedApp.Path
                                                    });
        }

        private void CloseAppView()
        {
            AppViewWidth = 0;
        }

        private void LoadAllApps()
        {
            Apps = new ObservableCollection<ApplicationListItemModel>(_applicationService.ListAllApplications(ParentId));
            Folders = new ObservableCollection<ParentFolderModel>(_applicationService.GetParentFolders(ParentId));
        }

        private ApplicationListItemModel CreateAddApp()
        {
            var app = new ApplicationListItemModel { CreateDate = DateTime.Now };
            app.PropertyChanged += (_, e) =>
                {
                    OnPropertyChanged(nameof(SelectedApp));

                    if (e.PropertyName == nameof(SelectedApp.ItemType))
                    {
                        OnPropertyChanged(nameof(IsFileType));
                        OnPropertyChanged(nameof(ArgumentsWatermark));
                    }
                };
            return app;
        }

        private void LoadWebsiteNameAndDescriptionEventHandler(object obj, PropertyChangedEventArgs eventArgs)
        {
            if (obj is ApplicationListItemModel application
                && eventArgs.PropertyName == nameof(application.Arguments)
                && application.ItemType == ApplicationItemType.Website
                && (string.IsNullOrWhiteSpace(application.Name) || string.IsNullOrWhiteSpace(application.Description))
                && !string.IsNullOrWhiteSpace(application.Arguments)
                && Uri.TryCreate(application.Arguments, UriKind.Absolute, out var uri))
            {
                _ = Application.Current.Dispatcher.InvokeAsync(() => LoadWebSiteNameAndDescription(uri, application));
            }
        }

        [SuppressMessage("Design", "CA1031: Do not catch general exception types", Justification = "Reviewed")]
        private static async Task LoadWebSiteNameAndDescription(Uri uri, ApplicationListItemModel application)
        {
            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(await HttpClient.GetStringAsync(uri).ConfigureAwait(false));
                if (string.IsNullOrWhiteSpace(application.Name))
                {
                    var titleNode = doc.DocumentNode.SelectSingleNode("//head/title");
                    if (titleNode != null)
                    {
                        application.Name = titleNode.InnerText;
                    }
                }

                if (string.IsNullOrWhiteSpace(application.Description))
                {
                    var descriptionNode = doc.DocumentNode.SelectSingleNode("//meta[@name='description']");
                    var content = descriptionNode?.Attributes["content"];
                    if (content != null)
                    {
                        application.Description = content.Value;
                    }
                }
            }
            catch
            {
                // empty
            }
        }

        private void AddAppPath(object parameter)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                SelectedApp.Path = openFileDialog.FileName;
                SelectedApp.Name = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
            }
        }
    }
}
