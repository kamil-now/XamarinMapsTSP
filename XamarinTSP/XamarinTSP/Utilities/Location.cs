using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using XamarinTSP.Abstractions;

namespace XamarinTSP.Utilities
{
    public class Location : PropertyChangedBase, IDisposable
    {
        public Location()
        {
            _name = "";
            //Pin = new Pin() { Label = _name };
        }
        public EventHandler OnDispose;
        public EventHandler OnEdit;
        private volatile object _lck = new object();
        private string _name;
        //public Pin Pin { get; set; }
        public Position Coordinates { get; set; }
        public string Name
        {
            get => _name; set
            {
                _name = value;
                OnEdit?.Invoke(null, null);
                NotifyOfPropertyChange(() => Name);
            }
        }
        public ICommand EditFinishedCommand => new Command(() =>
        {
            lock (_lck)
            {
                OnEdit?.Invoke(null, null);
                NotifyOfPropertyChange(() => Name);
            }

        });
        //public void SetPinPosition(Position position)
        //{
        //    Pin.Position = position;
        //    NotifyOfPropertyChange(() => Pin);
        //}

        public void Dispose() => OnDispose(this, null);
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Name)) return Name;
            return $"{Coordinates.Latitude } {Coordinates.Longitude}";
        }
    }
}
