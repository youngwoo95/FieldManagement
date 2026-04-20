using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FieldManagement.Models;
using FieldManagement.Windows;

namespace FieldManagement;

public partial class InputView : UserControl
{
    private readonly Dictionary<int, MarkerPosition> _positions = new();
    private bool _markersInitialized;

    private bool _isDragging;
    private Point _dragStartPoint;
    private Button? _dragTarget;

    public InputView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (_markersInitialized)
            return;

        _markersInitialized = true;

        AddDraggableButton(1, 120, 80, "1");
        AddDraggableButton(2, 260, 150, "2");
        AddDraggableButton(3, 420, 220, "3");
    }

    private void AddDraggableButton(int id, double x, double y, string text)
    {
        var button = new Button
        {
            Content = text,
            Tag = id
        };

        if (TryFindResource("OverlayLampButtonStyle") is Style lampStyle)
        {
            button.Style = lampStyle;
        }

        Canvas.SetLeft(button, x);
        Canvas.SetTop(button, y);

        button.PreviewMouseLeftButtonDown += Draggable_PreviewMouseLeftButtonDown;
        button.PreviewMouseMove += Draggable_PreviewMouseMove;
        button.PreviewMouseLeftButtonUp += Draggable_PreviewMouseLeftButtonUp;

        OverlayCanvas.Children.Add(button);
    }

    private void Draggable_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not Button button)
            return;

        _isDragging = true;
        _dragTarget = button;
        _dragStartPoint = e.GetPosition(OverlayCanvas);

        button.CaptureMouse();
        e.Handled = true;
    }

    private void Draggable_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (!_isDragging || _dragTarget is null)
            return;

        Point currentPoint = e.GetPosition(OverlayCanvas);

        double dx = currentPoint.X - _dragStartPoint.X;
        double dy = currentPoint.Y - _dragStartPoint.Y;

        double left = Canvas.GetLeft(_dragTarget);
        double top = Canvas.GetTop(_dragTarget);

        if (double.IsNaN(left)) left = 0;
        if (double.IsNaN(top)) top = 0;

        Canvas.SetLeft(_dragTarget, left + dx);
        Canvas.SetTop(_dragTarget, top + dy);

        _dragStartPoint = currentPoint;
    }

    private void Draggable_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (_dragTarget is not null)
        {
            _dragTarget.ReleaseMouseCapture();

            double left = Canvas.GetLeft(_dragTarget);
            double top = Canvas.GetTop(_dragTarget);

            int id = (int)_dragTarget.Tag;
            string text = _dragTarget.Content?.ToString() ?? string.Empty;

            _positions[id] = new MarkerPosition
            {
                Id = id,
                Text = text,
                X = left,
                Y = top
            };

            Console.WriteLine(_positions[id].ToString());
        }

        _isDragging = false;
        _dragTarget = null;
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        var popup = new EditWindows
        {
            Owner = Window.GetWindow(this)
        };
        popup.ShowDialog();
    }
}
