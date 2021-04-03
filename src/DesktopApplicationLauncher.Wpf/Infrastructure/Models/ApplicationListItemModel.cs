namespace DesktopApplicationLauncher.Wpf.Infrastructure.Models
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using DesktopApplicationLauncher.Wpf.Infrastructure.Entities;

    public sealed class ApplicationListItemModel : INotifyPropertyChanged
    {
        private DateTime _createDate;
        private DateTime? _lastAccessedDate;
        private string _arguments;
        private string _path;
        private string _name;
        private int _sortOrder;

        private ApplicationItemType _itemType;

        public int Id { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (value == _name)
                {
                    return;
                }

                _name = value;
                OnPropertyChanged();
            }
        }

        public string Path
        {
            get => _path;
            set
            {
                if (value == _path)
                {
                    return;
                }

                _path = value;
                OnPropertyChanged();
            }
        }

        public string Arguments
        {
            get => _arguments;
            set
            {
                if (value == _arguments)
                {
                    return;
                }

                _arguments = value;
                OnPropertyChanged();
            }
        }

        public DateTime? LastAccessedDate
        {
            get => _lastAccessedDate;
            set
            {
                if (Nullable.Equals(value, _lastAccessedDate))
                {
                    return;
                }

                _lastAccessedDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime CreateDate
        {
            get => _createDate;
            set
            {
                if (value.Equals(_createDate))
                {
                    return;
                }

                _createDate = value;
                OnPropertyChanged();
            }
        }

        public int SortOrder
        {
            get => _sortOrder;
            set
            {
                if (value.Equals(_sortOrder))
                {
                    return;
                }

                _sortOrder = value;
                OnPropertyChanged();
            }
        }

        public ApplicationItemType ItemType
        {
            get => _itemType;
            set
            {
                if (value.Equals(_itemType))
                {
                    return;
                }

                _itemType = value;

                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
