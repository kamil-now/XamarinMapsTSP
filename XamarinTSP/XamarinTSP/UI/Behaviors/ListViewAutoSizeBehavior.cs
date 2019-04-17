using Xamarin.Forms;

namespace XamarinTSP.UI.Behaviors
{
    public class ListViewAutoSizeBehavior : BehaviorBase<ListView>
    {
        public static BindableProperty MaxRowsCountProperty = BindableProperty.Create(
                                                       propertyName: "MaxRowsCountProperty",
                                                       returnType: typeof(int),
                                                       declaringType: typeof(ListViewAutoSizeBehavior),
                                                       defaultValue: null,
                                                       defaultBindingMode: BindingMode.OneWay);

        public static BindableProperty MinRowsCountProperty = BindableProperty.Create(
                                                      propertyName: "MinRowsCountProperty",
                                                      returnType: typeof(int),
                                                      declaringType: typeof(ListViewAutoSizeBehavior),
                                                      defaultValue: null,
                                                      defaultBindingMode: BindingMode.OneWay);
        public static BindableProperty ItemsCountProperty = BindableProperty.Create(
                                                       propertyName: "ItemsCountProperty",
                                                       returnType: typeof(int),
                                                       declaringType: typeof(ListViewAutoSizeBehavior),
                                                       defaultValue: null,
                                                       defaultBindingMode: BindingMode.OneWay,
                                                       propertyChanged: ItemsCountChanged);

        public int MaxRowsCount
        {
            get { return (int)GetValue(MaxRowsCountProperty); }
            set { SetValue(MaxRowsCountProperty, value); }
        }
        public int MinRowsCount
        {
            get { return (int)GetValue(MinRowsCountProperty); }
            set { SetValue(MinRowsCountProperty, value); }
        }
        public int ItemsCount
        {
            get { return (int)GetValue(ItemsCountProperty); }
            set { SetValue(ItemsCountProperty, value); }
        }
        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.Refreshing += (s, e) => SetHeight(ItemsCount);
            bindable.ItemAppearing += (s, e) => SetHeight(ItemsCount);
        }

        private static void ItemsCountChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ListViewAutoSizeBehavior behavior)
            {
                behavior.SetHeight((int)newValue);
            }
        }
        void SetHeight(int numberOfElements)
        {
            var cellHeight = MeasureCellHeight();
            var minHeight = MinRowsCount * cellHeight;
            var maxHeight = MaxRowsCount * cellHeight;
            var height = numberOfElements * cellHeight;


            if (AssociatedObject.Header is VisualElement header)
            {
                if (header.HeightRequest < header.MinimumHeightRequest)
                {
                    header.HeightRequest = header.MinimumHeightRequest;
                }
                height += header.HeightRequest;
            }
            if (AssociatedObject.Footer is VisualElement footer)
            {
                if (footer.HeightRequest < footer.MinimumHeightRequest)
                {
                    footer.HeightRequest = footer.MinimumHeightRequest;
                }
                height += footer.HeightRequest;
            }
            AssociatedObject.HeightRequest = height > minHeight ? height < maxHeight ? height : maxHeight : minHeight;
        }
        double MeasureCellHeight()
        {
            var template = AssociatedObject.ItemTemplate;
            var cell = (Cell)template.CreateContent();
            var height = cell.RenderHeight;
            return height * 1.1;
        }
    }
}
