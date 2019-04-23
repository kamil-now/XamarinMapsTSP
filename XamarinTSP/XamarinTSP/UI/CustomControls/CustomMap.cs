using Autofac;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace XamarinTSP.UI.CustomControls
{
    public class CustomMap : Map
    {
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
            var vm = (Application.Current as App).Container.Resolve<CustomMapContext>();
            //RouteLocations = new List<Position>();
            vm.CustomMap = this;
        }
    }

}
