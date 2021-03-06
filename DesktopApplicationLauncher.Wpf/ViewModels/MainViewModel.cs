namespace DesktopApplicationLauncher.Wpf.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Input;

    using DesktopApplicationLauncher.Wpf.Commands;
    using DesktopApplicationLauncher.Wpf.Infrastructure.Business;
    using DesktopApplicationLauncher.Wpf.Infrastructure.Models;

    using Microsoft.Win32;

    public sealed class MainViewModel : ViewModelBase
    {
        private readonly Window _ownerWindow;

        private readonly IApplicationService _applicationService;

        [SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Reviewed")]
        public ObservableCollection<ApplicationListItemModel> Apps
        {
            get => _apps;
            set
            {
                _apps = value;
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
            }
        }

        private int _appViewWidth;

        private ObservableCollection<ApplicationListItemModel> _apps;

        public int AppViewWidth
        {
            get => _appViewWidth;
            set
            {
                _appViewWidth = value;
                OnPropertyChanged();
            }
        }

        public ICommand DeleteAppCommand { get; }

        public ICommand CloseAppViewCommand { get; }

        public ICommand AddAppCommand { get; }

        public ICommand AddAppPathCommand { get; }

        public ICommand SaveAppCommand { get; }

        public ICommand OpenAppCommand { get; }

        public ICommand SwapAppsCommand { get; }

        public MainViewModel(Window ownerWindow, IApplicationService applicationService)
        {
            _ownerWindow = ownerWindow ?? throw new ArgumentNullException(nameof(ownerWindow));
            _applicationService = applicationService ?? throw new ArgumentNullException(nameof(applicationService));

            AddAppCommand = new RelayCommand(AddApp);
            OpenAppCommand = new RelayCommand(OpenApp);
            DeleteAppCommand = new RelayCommand(RemoveSelectedApp, _ => SelectedApp != null);
            AddAppPathCommand = new RelayCommand(AddAppPath, _ => SelectedApp != null);
            SaveAppCommand = new RelayCommand(SaveApp, _ => SelectedApp != null);
            CloseAppViewCommand = new RelayCommand(_ => CloseAppView());
            SwapAppsCommand = new RelayCommand(SwapApps);

            LoadAllApps();
        }

        private void SwapApps(object parameter)
        {
            (int startIndex, int endIndex) = ((int, int))parameter;

            for (var i = startIndex; i <= endIndex; i++)
            {
                var item = Apps[i];
                _applicationService.UpdateApplicationOrder(item.Id, i);
            }
        }

        private void AddApp(object parameter)
        {
            SelectedApp = parameter is ApplicationListItemModel model ? model : CreateAddApp();
            AppViewWidth = 250;
        }

        private void SaveApp(object parameter)
        {
            SaveAppToDb();
            CloseAppView();
            LoadAllApps();
        }

        private void RemoveSelectedApp(object parameter)
        {
            var selectedApp = SelectedApp;
            _applicationService.DeleteApp(selectedApp.Id);
        }

        [SuppressMessage("Microsoft.Design", "CA1031: Do not catch general exception types", Justification = "REviewed")]
        private void OpenApp(object parameter)
        {
            if (parameter is ApplicationListItemModel appItem)
            {
                try
                {
                    Process.Start(new ProcessStartInfo(appItem.Path)
                                      {
                                          Arguments = appItem.Arguments,
                                          UseShellExecute = true
                                      });
                }
                catch (Exception exception)
                {
                    // Empty
                    MessageBox.Show(_ownerWindow, $"File Open Error!!{Environment.NewLine}{exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveAppToDb()
        {
            var selectedApp = SelectedApp;
            if (selectedApp.Id > 0)
            {
                _applicationService.UpdateApplication(new ApplicationUpdateModel
                                                       {
                                                           Id = selectedApp.Id,
                                                           Name = selectedApp.Name,
                                                           Arguments = selectedApp.Arguments,
                                                           Path = selectedApp.Path
                                                       });
            }
            else
            {
                _applicationService.AddApplication(new ApplicationAddModel
                                                       {
                                                           Name = selectedApp.Name,
                                                           Arguments = selectedApp.Arguments,
                                                           Path = selectedApp.Path
                                                       });
            }
        }

        private void CloseAppView()
        {
            AppViewWidth = 0;
        }

        private void LoadAllApps()
        {
            Apps = new ObservableCollection<ApplicationListItemModel>(_applicationService.ListAllApplications());
        }

        private ApplicationListItemModel CreateAddApp()
        {
            var app = new ApplicationListItemModel { CreateDate = DateTime.Now };
            app.PropertyChanged += (_, _) => OnPropertyChanged(nameof(SelectedApp));
            return app;
        }

        private void AddAppPath(object parameter)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                SelectedApp.Path = openFileDialog.FileName;
            }
        }
    }
}
