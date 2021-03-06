// Christopher Braun 2016

using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace FMA.View.Helpers
{
    public class SelectionAdorner : Adorner
    {
        private readonly VisualCollection visualChildren;
        private const int Offset = 4;
        private Border selectionBorder;

        public SelectionAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            IsClipEnabled = true;
            visualChildren = new VisualCollection(this);
            BuildAdornerCorner();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var desiredWidth = AdornedElement.DesiredSize.Width;
            var desiredHeight = AdornedElement.DesiredSize.Height;

            selectionBorder.Width = desiredWidth + 2*Offset;
            selectionBorder.Height = desiredHeight + 2*Offset;

            selectionBorder.Arrange(new Rect(-Offset, -Offset, selectionBorder.Width, selectionBorder.Height));

            return finalSize;
        }

        private void BuildAdornerCorner()
        {
            selectionBorder = new Border
            {
                Opacity = 0.60,
                BorderBrush = new SolidColorBrush(Colors.Blue),
                BorderThickness = new Thickness(1.5)
            };

            visualChildren.Add(selectionBorder);
        }

        protected override int VisualChildrenCount => visualChildren.Count;

        protected override Visual GetVisualChild(int index)
        {
            return visualChildren[index];
        }
    }
}