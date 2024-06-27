using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET.WindowsPresentation;

namespace LocationView.Client.Config
{
    public enum MarkerTypes
    {
        Blue,
        Green,
        LightBlue, 
        Orange,
        Pink,
        Purple,
        Red,
        Yellow,
    }
    
    internal class MarkerTypesHelper
    {
        public static readonly Dictionary<MarkerTypes, Brush> Markers =
            new Dictionary<MarkerTypes, Brush>()
            {
                {MarkerTypes.Blue, Brushes.Blue},
                {MarkerTypes.Green, Brushes.Green},
                {MarkerTypes.LightBlue, Brushes.LightBlue},
                {MarkerTypes.Orange, Brushes.Orange},
                {MarkerTypes.Pink, Brushes.Pink},
                {MarkerTypes.Purple, Brushes.Purple},
                {MarkerTypes.Red, Brushes.Red},
                {MarkerTypes.Yellow, Brushes.Yellow},
            };
    }
    
    internal class MarkerFactory
    {
        public static GMapMarker CreateMarker(MarkerTypes type, ToolTipAppearanceTypes toolTipAppearance)
        {
            var polygon = new Polygon
            {
                Points = new PointCollection { new Point(0, 0), new Point(-3, -10), new Point(3, -10), new Point(0, 0) },
                Stroke = MarkerTypesHelper.Markers[type],
                StrokeThickness = 1.5,
                Fill = MarkerTypesHelper.Markers[type]
            };
            var label = new Label
            {
                Background = Brushes.Gray.Clone(),
                Opacity = 0.5
            };
            if (toolTipAppearance == ToolTipAppearanceTypes.Always)
            {
                label.Visibility = Visibility.Visible;
            }
            else
            {
                var binding = new Binding()
                {
                    Source = polygon,
                    Path = new PropertyPath(FrameworkElement.IsMouseOverProperty),
                    Converter = new BooleanToVisibilityConverter()
                };
                label.SetBinding(FrameworkElement.VisibilityProperty, binding);
            }
            var marker =  new GMapMarker(new PointLatLng())
            {
                Shape = new StackPanel
                {
                    Visibility = Visibility.Hidden,
                    Orientation = Orientation.Vertical,
                    Children = { polygon, label }
                }
            };

            return marker;
        }
    }

    internal class MarkerNames
    {
        private static readonly Dictionary<MarkerTypes, string> Names =
            new Dictionary<MarkerTypes, string>()
            {
                {MarkerTypes.Blue, "Blue"},
                {MarkerTypes.Green, "Green"},
                {MarkerTypes.LightBlue, "Light blue"},
                {MarkerTypes.Orange, "Orange"},
                {MarkerTypes.Pink, "Pink"},
                {MarkerTypes.Purple, "Purple"},
                {MarkerTypes.Red, "Red"},
                {MarkerTypes.Yellow, "Yellow"},
            };

        public static string GetName(MarkerTypes type)
        {
            return TypesHelper.GetName(Names, type);
        }

        public static MarkerTypes GetType(string name)
        {
            return TypesHelper.GetType(Names, name);
        }
    }

}
