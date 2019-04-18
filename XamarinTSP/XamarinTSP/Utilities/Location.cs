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

        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }
        public string AdminArea { get; set; }
        public Guid Id { get; }
        public string MainDisplayString
        {
            get
            {
                var retval = string.Join(", ", new[] { Street, City, Country });
                if (string.IsNullOrEmpty(retval)) return retval;
                return $"{Position.Latitude } {Position.Longitude}";
            }
        }
        public string AdditionalLocationInfo => string.Join(" ", new[] { PostalCode, City, AdminArea });
        public Position Position
        {
            get => _position;
            set
            {
                _position = value;
                PositionChanged?.Invoke(this, null);
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => MainDisplayString);
            }
        }
        public Location()
        {
            Id = Guid.NewGuid();
            _name = "";
        }
        public ICommand EditFinishedCommand => new Command(() =>
        {
            //lock (_lck)
            //{
            //    NotifyOfPropertyChange(() => Name);
            //}

        });
        public void Dispose() => OnDispose?.Invoke(this, null);

    }
}
