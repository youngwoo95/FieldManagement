using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using FieldManagement.Models;

namespace FieldManagement.Behaviors;

public static class CanvasDragBehavior
{
    private static readonly DependencyProperty DragStateProperty =
        DependencyProperty.RegisterAttached(
            "DragState",
            typeof(DragState),
            typeof(CanvasDragBehavior),
            new PropertyMetadata(null));

    public static readonly DependencyProperty IsEnabledProperty =
        DependencyProperty.RegisterAttached(
            "IsEnabled",
            typeof(bool),
            typeof(CanvasDragBehavior),
            new PropertyMetadata(false, OnIsEnabledChanged));

    public static void SetIsEnabled(DependencyObject element, bool value)
    {
        element.SetValue(IsEnabledProperty, value);
    }

    public static bool GetIsEnabled(DependencyObject element)
    {
        return (bool)element.GetValue(IsEnabledProperty);
    }

    private static void SetDragState(DependencyObject element, DragState? state)
    {
        element.SetValue(DragStateProperty, state);
    }

    private static DragState? GetDragState(DependencyObject element)
    {
        return (DragState?)element.GetValue(DragStateProperty);
    }

    private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not FrameworkElement element)
        {
            return;
        }

        if ((bool)e.NewValue)
        {
            element.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
            element.PreviewMouseMove += OnMouseMove;
            element.PreviewMouseLeftButtonUp += OnMouseLeftButtonUp;
        }
        else
        {
            element.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown;
            element.PreviewMouseMove -= OnMouseMove;
            element.PreviewMouseLeftButtonUp -= OnMouseLeftButtonUp;
        }
    }

    private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not FrameworkElement element)
        {
            return;
        }

        var canvas = FindParentCanvas(element);
        if (canvas is null)
        {
            return;
        }

        var startPoint = e.GetPosition(canvas);
        SetDragState(element, new DragState(canvas, startPoint));
        element.CaptureMouse();
        e.Handled = true;
    }

    private static void OnMouseMove(object sender, MouseEventArgs e)
    {
        if (sender is not FrameworkElement element)
        {
            return;
        }

        var state = GetDragState(element);
        if (state is null || !element.IsMouseCaptured)
        {
            return;
        }

        var currentPoint = e.GetPosition(state.Canvas);
        var dx = currentPoint.X - state.LastPoint.X;
        var dy = currentPoint.Y - state.LastPoint.Y;

        var left = Canvas.GetLeft(element);
        var top = Canvas.GetTop(element);

        if (double.IsNaN(left))
        {
            left = 0;
        }

        if (double.IsNaN(top))
        {
            top = 0;
        }

        left += dx;
        top += dy;

        Canvas.SetLeft(element, left);
        Canvas.SetTop(element, top);

        if (element.DataContext is MarkerPosition marker)
        {
            marker.X = left;
            marker.Y = top;
        }

        state.LastPoint = currentPoint;
    }

    private static void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (sender is not FrameworkElement element)
        {
            return;
        }

        if (element.IsMouseCaptured)
        {
            element.ReleaseMouseCapture();
        }

        SetDragState(element, null);
    }

    private static Canvas? FindParentCanvas(DependencyObject child)
    {
        DependencyObject? current = child;
        while (current is not null)
        {
            if (current is Canvas canvas)
            {
                return canvas;
            }

            current = VisualTreeHelper.GetParent(current);
        }

        return null;
    }

    private sealed class DragState
    {
        public DragState(Canvas canvas, Point startPoint)
        {
            Canvas = canvas;
            LastPoint = startPoint;
        }

        public Canvas Canvas { get; }
        public Point LastPoint { get; set; }
    }
}
