using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Maps;
using XamarinTSP.UI.Models;
using XamarinTSP.UI.Utilities;

namespace XamarinTSP.UI.CustomControls
{
    public class CustomMap : Map
    {
        public static readonly BindableProperty IsRouteVisibleProperty =
            BindableProperty.Create(
                propertyName: "IsRouteVisibleProperty",
                returnType: typeof(bool),
                declaringType: typeof(CustomMap),
                defaultValue: null,
                defaultBindingMode: BindingMode.TwoWay,
                validateValue: null
            );

        public bool IsRouteVisible
        {
            get { return (bool)GetValue(IsRouteVisibleProperty); }
            set { SetValue(IsRouteVisibleProperty, value); }
        }

        public static readonly BindableProperty PositionProperty =
            BindableProperty.Create(
                propertyName: "PositionProperty",
                returnType: typeof(Position),
                declaringType: typeof(CustomMap),
                defaultValue: null,
                defaultBindingMode: BindingMode.TwoWay,
                validateValue: null,
                propertyChanged: PositionChanged
            );

        public Position Position
        {
            get { return (Position)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public static readonly BindableProperty LocationsProperty =
            BindableProperty.Create(
                propertyName: "LocationsProperty",
                returnType: typeof(IEnumerable<Location>),
                declaringType: typeof(CustomMap),
                defaultValue: null,
                defaultBindingMode: BindingMode.OneWay,
                validateValue: null,
                propertyChanged: LocationsChanged

            );

        public IEnumerable<Location> Locations
        {
            get { return (IEnumerable<Location>)GetValue(LocationsProperty); }
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

        public void UpdatePins()
        {
            Pins?.Clear();
            var coordinates = Locations?.Select(x => x.Position);
            int i = 1;
            coordinates?.ForEach(x => Pins.Add(new Pin() { Position = x, Label = i++.ToString() }));
        }
        public void FocusPins()
        {
            var pins = Pins.Select(x => x.Position);
            if (pins != null && pins.Count() > 0)
            {
                MoveToRegion(MapSpanGenerator.Generate(pins));
            }
        }
        public void FocusPosition(Position position) => MoveToRegion(MapSpanGenerator.Generate(position));
        private void Locations_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdatePins();
            FocusPins();
        }
        private static void PositionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CustomMap map)
            {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(map.Position, Distance.FromKilometers(1)));
            }
        }
        private static void LocationsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CustomMap map)
            {
                if (newValue is ObservableCollection<Location> locations)
                {
                    locations.CollectionChanged += map.Locations_CollectionChanged;
                }
                if (oldValue is ObservableCollection<LocationList> oldLocations)
                {
                    oldLocations.CollectionChanged -= map.Locations_CollectionChanged;
                }
            }
        }

       
    }

}
