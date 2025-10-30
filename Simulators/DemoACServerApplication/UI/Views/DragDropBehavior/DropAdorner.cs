using System;
using System.Windows;
using System.Windows.Documents;

namespace DemoServerApplication.UI.Views.DragDropBehavior
{
    /// <summary>
    /// Handles the visual indication of the drop point
    /// </summary>
    public class DropAdorner : Adorner, IDisposable
    {
        private AdornerLayer _adornerLayer;

        public DropAdorner(UIElement adornedElement, AdornerLayer adornerLayer)
            : base(adornedElement)
        {
            if (adornedElement == null) throw new ArgumentNullException("adornedElement");
            if (adornerLayer == null) throw new ArgumentNullException("adornerLayer");
                       
            _adornerLayer = adornerLayer;
            this.IsHitTestVisible = false;

            _adornerLayer.Add(this);
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
