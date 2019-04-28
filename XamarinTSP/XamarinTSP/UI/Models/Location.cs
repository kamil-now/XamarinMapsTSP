using System;
using System.Linq;
using Xamarin.Forms.Maps;
using XamarinTSP.Common;
using XamarinTSP.UI.Abstractions;

namespace XamarinTSP.UI.Models
{
    public class Location : PropertyChangedBase, IDisposable
    {
        public EventHandler OnDispose;

        private Position _position;

        public Guid Id { get; }
        public string MainDisplayString { get; private set; }
        public string AdditionalLocationInfo { get; private set; }
        public string Coordinates { get; private set; }
        public Position Position
        {
            get => _position;
            set
            {
                _position = value;
                NotifyOfPropertyChange();
            }
        }
        public Location() { }
        public Location(Address address)
        {
            Id = Guid.NewGuid();
            BuildStrings(address);
            Position = new Position(address.Latitude, address.Longitude);
        }

        public void Dispose() => OnDispose?.Invoke(this, null);

        private void BuildStrings(Address address)
        {
            Coordinates = $"{address.Latitude } {address.Longitude}";
            var street = $"{address.Thoroughfare} {address.SubThoroughfare}";
            var tmp = new[] {
                    string.Join(" ", street),
                    string.Join(" ", address.PostalCode, address.Locality, address.SubLocality),
                    string.Join(" ", address.AdminArea, address.SubAdminArea),
                    string.Join(" ", address.CountryName, address.CountryCode),
                    address.FeatureName,
                    Coordinates
                };
            var retval = tmp.FirstOrDefault(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x));

            MainDisplayString = retval ?? "---";

            AdditionalLocationInfo = string.Join("\n", tmp.Select(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)));

            NotifyOfPropertyChange(() => Coordinates);
            NotifyOfPropertyChange(() => MainDisplayString);
            NotifyOfPropertyChange(() => AdditionalLocationInfo);
        }
    }
}
