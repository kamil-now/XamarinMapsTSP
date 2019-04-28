using Autofac;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Maps;
using XamarinTSP.UI.Models;
using XamarinTSP.UI.ViewModels;

namespace XamarinTSP.UI.CustomControls
{
    public class CustomMap : Map
    {
        public static readonly BindableProperty LocationsProperty =
            BindableProperty.Create(
                propertyName: "LocationsProperty",
                returnType: typeof(List<Location>),
                declaringType: typeof(CustomMap),
                defaultValue: null,
                defaultBindingMode: BindingMode.OneWay
            );

        public List<Location> Locations
        {
            get { return (List<Location>)GetValue(LocationsProperty); }
            set { SetValue(LocationsProperty, value); }
        }
        public static readonly BindableProperty RouteCoordinatesProperty =
            BindableProperty.Create(
                propertyName: "RouteCoordinatesProperty",
                returnType: typeof(List<Position>),
                declaringType: typeof(CustomMap),
                defaultValue: null,
                defaultBindingMode: BindingMode.OneWay
            );

        public List<Position> RouteCoordinates
        {
            get { return (List<Position>)GetValue(RouteCoordinatesProperty); }
            set { SetValue(RouteCoordinatesProperty, value); }
        }
        public CustomMap()
        {
            var vm = (Application.Current as App).Container.Resolve<CustomMapViewModel>();
            Locations = new List<Location>();
            vm.List.Locations.CollectionChanged += Locations_CollectionChanged;
            vm.CustomMap = this;
        }
        private void Locations_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            bool listChanged = false;
            if (e.OldItems != null)
            {
                foreach (Location item in e.OldItems)
                {
                    var itemToRemove = Locations.First(x => x.Id == item.Id);
                    Locations.Remove(itemToRemove);
                    listChanged = true;
                }
            }
            if (e.NewItems != null)
            {
                foreach (Location item in e.NewItems)
                {
                    Locations.Add(item);
                    listChanged = true;
                }
            }
            if (listChanged)
            {
                //TODO aggregate update async queue
                UpdatePins();
                FocusPins();
            }
        }

        public void UpdatePins()
        {
            Pins.Clear();
            var coordinates = Locations.Select(x => x.Position);
            int i = 1;
            coordinates.ForEach(x => Pins.Add(new Pin() { Position = x, Label = i++.ToString() }));
        }
        public void FocusPins()
        {
            var pins = Pins.Select(x => x.Position);
            if (pins != null && pins.Count() > 0)
            {
                MoveToRegion(MapSpanGenerator.Generate(pins));
            }
        }
    }

}
