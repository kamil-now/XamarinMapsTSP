using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace XamarinTSP.UI.Behaviors
{
    public class ListViewAutoSizeBehavior : BehaviorBase<ListView>
    {
        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.ItemAppearing += AppearanceChanged;
            bindable.ItemDisappearing += AppearanceChanged;
            bindable.BindingContextChanged += Bindable_BindingContextChanged;
        }

        private void Bindable_BindingContextChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.ItemAppearing -= AppearanceChanged;
            bindable.ItemDisappearing -= AppearanceChanged;
        }
        private void AppearanceChanged(object sender, ItemVisibilityEventArgs e) => UpdateHeight(e.Item);

        private void UpdateHeight(object item)
        {
            if (AssociatedObject.HasUnevenRows)
            {
                double height;

                if ((height = AssociatedObject.HeightRequest) ==
                    (double)VisualElement.HeightRequestProperty.DefaultValue)
                    height = 0;

                height += MeasureRowHeight(item);

                SetHeight(height);
            }
            else if (AssociatedObject.RowHeight == (int)ListView.RowHeightProperty.DefaultValue)
            {
                var height = MeasureRowHeight(item);
                AssociatedObject.RowHeight = height;
                SetHeight(height);
            }
        }

        int MeasureRowHeight(object item)
        {
            var template = AssociatedObject.ItemTemplate;
            var cell = (Cell)template.CreateContent();
            cell.BindingContext = item;
            var height = cell.RenderHeight;
            var mod = height % 1;
            if (mod > 0)
                height = height - mod + 1;
            return (int)height;
        }

        void SetHeight(double height)
        {
            double minHeight = AssociatedObject.MinimumHeightRequest;
            if (AssociatedObject.Header is VisualElement header)
                height += header.HeightRequest;
            if (AssociatedObject.Footer is VisualElement footer)
            {
                height += footer.HeightRequest;
                if (footer.HeightRequest < minHeight)
                {
                    footer.HeightRequest = minHeight;
                }
            }
            AssociatedObject.HeightRequest = 200;// height > minHeight ? height : minHeight;
        }
    }
}
