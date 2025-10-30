using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace DemoServerApplication.UI.Views.DragDropBehavior
{
    /// <summary>
    /// Handles the visual display of the item as it's being dragged
    /// </summary>
    public class DragAdorner : Adorner, IDisposable
    {
        private ContentPresenter _contentPresenter;
        private AdornerLayer _adornerLayer;
        private double _leftOffset;
        private double _topOffset;

        public DragAdorner(object data, DataTemplate dataTemplate, UIElement adornedElement, AdornerLayer adornerLayer)
            : base(adornedElement)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (adornerLayer == null) throw new ArgumentNullException("adornerLayer");

            _adornerLayer = adornerLayer;
            _contentPresenter = new ContentPresenter() { Content = data, ContentTemplate = dataTemplate, Opacity = 0.9 };

            _adornerLayer.Add(this);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            _contentPresenter.Measure(constraint);
            return _contentPresenter.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _contentPresenter.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return _contentPresenter;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        public void UpdatePosition(double left, double top)
        {
            _leftOffset = left;
            _topOffset = top;

            if (_adornerLayer != null)
                _adornerLayer.Update(this.AdornedElement);
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            GeneralTransformGroup result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(_leftOffset, _topOffset));

            return result;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
                _adornerLayer.Remove(this);
        }

        #endregion
    }
}
