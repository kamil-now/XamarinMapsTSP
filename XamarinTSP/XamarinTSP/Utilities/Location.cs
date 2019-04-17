using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using XamarinTSP.Abstractions;

namespace XamarinTSP.Utilities
{
    public class Location : PropertyChangedBase, IDisposable
    {
        public EventHandler OnDispose;
        public EventHandler PositionChanged;

        private volatile object _lck = new object();
        private string _name;
        private Position _position;

        public Guid Id { get; }
        public string DisplayString
        {
            get
            {
                if (!string.IsNullOrEmpty(Name)) return Name;
                return $"{Position.Latitude } {Position.Longitude}";
            }
        }
        public Position Position
        {
            get => _position;
            set
            {
                _position = value;
                PositionChanged?.Invoke(this, null);
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => DisplayString);
            }
        }
        public string Name
        {
            get => _name; set
            {
                _name = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => DisplayString);
            }
        }
        public Location()
        {
            Id = Guid.NewGuid();
            _name = "";
        }
        public ICommand EditFinishedCommand => new Command(() =>
        {
            lock (_lck)
            {
                NotifyOfPropertyChange(() => Name);
            }

        });
        public void Dispose() => OnDispose?.Invoke(this, null);

    }
}
