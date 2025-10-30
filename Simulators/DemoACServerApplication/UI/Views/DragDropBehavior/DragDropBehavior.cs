using DemoServerApplication.ACSystem;
using DemoServerApplication.UI.ViewModels;
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace DemoServerApplication.UI.Views.DragDropBehavior
{
    /// <summary>
    /// An attached behavior that allows you to drag and drop items among various ItemsControls, e.g. ItemsControl, ListBox, TabControl, etc.
    /// </summary>
    public class DragDropBehavior : Behavior<ItemsControl>, IDisposable
    {
        private const int DRAG_WAIT_COUNTER_LIMIT = 10;
        private const string DEFAULT_DATA_FORMAT_STRING = "DemoServerApplication.UI.Views.DragDropBehavior.DataFormat";

        private bool _isMouseDown;
        private bool _isDragging;
        private object _data;
        private Point _dragStartPosition;
        private DragAdorner _dragAdorner;
        private DropAdorner _dropAdorner;
        private int _dragScrollWaitCounter = DRAG_WAIT_COUNTER_LIMIT;

        /// <summary>
        /// Called when attached to an ItemsControl.
        /// </summary>
        protected override void OnAttached()
        {
            this.AssociatedObject.AllowDrop = true;

            this.AssociatedObject.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
            this.AssociatedObject.MouseMove += OnMouseMove;
            this.AssociatedObject.PreviewMouseLeftButtonUp += OnPreviewMouseLeftButtonUp;
            this.AssociatedObject.Drop += OnDrop;
            this.AssociatedObject.PreviewDrop += OnPreviewDrop;
            this.AssociatedObject.QueryContinueDrag += OnPreviewQueryContinueDrag;
            this.AssociatedObject.DragEnter += OnDragEnter;
            this.AssociatedObject.DragOver += OnDragOver;
            this.AssociatedObject.DragLeave += OnDragLeave;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the type of the items in the ItemsControl. 
        /// </summary>
        public Type ItemType { get; set; }

        /// <summary>
        /// Gets or sets the data template of the items to use while dragging. 
        /// </summary>
        public DataTemplate DataTemplate { get; set; }

        public Orientation DropIndication { get { return _DropIndication; } set { _DropIndication = value; } }
        private Orientation _DropIndication = Orientation.Vertical;

        #endregion

        #region Button Events

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ItemsControl itemsControl = (ItemsControl)sender;
            Point p = e.GetPosition(itemsControl);

            _data = UIHelpers.GetItemFromPointInItemsControl(itemsControl, p);
            if (_data != null)
            {
                _isMouseDown = true;
                _dragStartPosition = p;
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseDown)
            {
                ItemsControl itemsControl = (ItemsControl)sender;
                Point currentPosition = e.GetPosition(itemsControl);

                if ((_isDragging == false) &&
                    (Math.Abs(currentPosition.X - _dragStartPosition.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                    (Math.Abs(currentPosition.Y - _dragStartPosition.Y) > SystemParameters.MinimumVerticalDragDistance))
                {
                    DragStarted(itemsControl);
                }
            }
        }

        private void OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ResetState();
            DetachAdorners();
        }

        #endregion

        #region Drag Events

        private void OnDragOver(object sender, DragEventArgs e)
        {
            ItemsControl itemsControl = (ItemsControl)sender;
            if (DataIsPresent(e) && CanDrop(itemsControl, GetData(e)))
            {
                UpdateDragAdorner(e.GetPosition(itemsControl));
                UpdateDropAdorner(itemsControl, e);
                HandleDragScrolling(itemsControl, e);
                e.Handled = true;
            }
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            ItemsControl itemsControl = (ItemsControl)sender;
            if (DataIsPresent(e) && CanDrop(itemsControl, GetData(e)))
            {
                object data = GetData(e);
                InitializeDragAdorner(itemsControl, data, e.GetPosition(itemsControl));
                InitializeDropAdorner(itemsControl, e);
                e.Handled = true;
            }
        }

        private void OnPreviewQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (e.EscapePressed)
            {
                e.Action = DragAction.Cancel;
                ResetState();
                DetachAdorners();
                e.Handled = true;
            }
        }

        private void OnPreviewDrop(object sender, DragEventArgs e)
        {
            DetachAdorners();
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            ItemsControl itemsControl = (ItemsControl)sender;

            DetachAdorners();
            e.Handled = true;

            if (DataIsPresent(e) && CanDrop(itemsControl, GetData(e)))
            {
                var credentialHolderViewModel = GetData(e) as CredentialHolderViewModel;
                var doorViewModel = UIHelpers.GetItemFromPointInItemsControl(itemsControl, e.GetPosition(itemsControl)) as DoorViewModel;

                if (itemsControl.Items.IsNullOrEmpty())
                {
                    e.Effects = DragDropEffects.None;
                    return;
                }

                e.Effects = DragDropEffects.Move;

                if(credentialHolderViewModel != null && doorViewModel != null)
                    AccessManager.Instance.RequestAccess(credentialHolderViewModel.CredentialHolder.Id, doorViewModel.Door.Id);
            }
            else e.Effects = DragDropEffects.None;
        }

        private void OnDragLeave(object sender, DragEventArgs e)
        {
            DetachAdorners();
            e.Handled = true;
        }

        #endregion

        private bool CanDrag(ItemsControl itemsControl, object item)
        {
            return (this.ItemType == null) || this.ItemType.IsInstanceOfType(item);
        }

        private bool CanDrop(ItemsControl itemsControl, object item)
        {
            return (this.ItemType == null) || this.ItemType.IsInstanceOfType(item);
        }

        private void DragStarted(ItemsControl itemsControl)
        {
            if (!CanDrag(itemsControl, _data))
                return;

            _isDragging = true;

            DataObject dObject;
            if (this.ItemType != null)
                dObject = new DataObject(this.ItemType, _data);
            else dObject = new DataObject(DEFAULT_DATA_FORMAT_STRING, _data);

            DragDropEffects effects = DragDrop.DoDragDrop(itemsControl, dObject, DragDropEffects.Copy | DragDropEffects.Move);

            ResetState();
        }

        private void HandleDragScrolling(ItemsControl itemsControl, DragEventArgs e)
        {
            var verticalMousePosition = UIHelpers.GetRelativeVerticalMousePosition(itemsControl, e.GetPosition(itemsControl));

            if (verticalMousePosition != UIHelpers.RelativeVerticalMousePosition.Middle)
            {
                if (_dragScrollWaitCounter == DRAG_WAIT_COUNTER_LIMIT)
                {
                    _dragScrollWaitCounter = 0;

                    ScrollViewer scrollViewer = UIHelpers.GetVisualDescendent<ScrollViewer>(itemsControl);
                    if (scrollViewer != null && scrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Visible)
                    {
                        e.Effects = DragDropEffects.Scroll;

                        double movementSize = (verticalMousePosition == UIHelpers.RelativeVerticalMousePosition.Top) ? 1.0 : -1.0;
                        scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + movementSize);
                    }
                }
                else _dragScrollWaitCounter++;
            }
            else  e.Effects = ((e.KeyStates & DragDropKeyStates.ControlKey) != 0) ? DragDropEffects.Copy : DragDropEffects.Move;
        }

        private void ResetState()
        {
            _isMouseDown = false;
            _isDragging = false;
            _data = null;
            _dragScrollWaitCounter = DRAG_WAIT_COUNTER_LIMIT;
        }

        private void InitializeDragAdorner(ItemsControl itemsControl, object dragData, Point startPosition)
        {
            if (this.DataTemplate == null) return;
            if (_dragAdorner != null) return;

            var adornerLayer = AdornerLayer.GetAdornerLayer(itemsControl);
            if (adornerLayer == null) return;

            _dragAdorner = new DragAdorner(dragData, DataTemplate, itemsControl, adornerLayer);
            _dragAdorner.UpdatePosition(startPosition.X, startPosition.Y);
        }

        private void UpdateDragAdorner(Point currentPosition)
        {
            if (_dragAdorner != null)
                _dragAdorner.UpdatePosition(currentPosition.X, currentPosition.Y);
        }

        private void InitializeDropAdorner(ItemsControl itemsControl, DragEventArgs e)
        {
            if (_dropAdorner == null)
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(itemsControl);
                UIElement itemContainer = UIHelpers.GetItemContainerFromPointInItemsControl(itemsControl, e.GetPosition(itemsControl));
                if (adornerLayer != null && itemContainer != null)
                    _dropAdorner = new DropAdorner(itemContainer, adornerLayer);
            }
        }

        private void UpdateDropAdorner(ItemsControl itemsControl, DragEventArgs e)
        {
            if (_dropAdorner != null)
                _dropAdorner.InvalidateVisual();
        }

        private void DetachAdorners()
        {
            if (_dropAdorner != null)
            {
                _dropAdorner.Dispose();
                _dropAdorner = null;
            }

            if (_dragAdorner != null)
            {
                _dragAdorner.Dispose();
                _dragAdorner = null;
            }
        }

        private bool DataIsPresent(DragEventArgs e)
        {
            if (this.ItemType != null)
            {
                if (e.Data.GetDataPresent(this.ItemType)) return true;

                string format = e.Data.GetFormats().FirstOrDefault();
                if (string.IsNullOrEmpty(format)) return false;

                object data = e.Data.GetData(format);
                if (data == null) return false;

                return this.ItemType.IsInstanceOfType(data);
            }
            else return !(e.Data.GetFormats().IsNullOrEmpty());
        }

        private object GetData(DragEventArgs e)
        {
            if ((this.ItemType != null) && (e.Data.GetDataPresent(this.ItemType)))
                return e.Data.GetData(ItemType);

            string format = e.Data.GetFormats().FirstOrDefault();
            if (string.IsNullOrEmpty(format)) return null;

            object data = e.Data.GetData(format);
            if (data == null) return null;

            if (this.ItemType != null && !this.ItemType.IsInstanceOfType(data)) return null;

            return data;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                DetachAdorners();
        }

        #endregion
    }
}
