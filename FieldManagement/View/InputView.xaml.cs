using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FieldManagement.Models;
using FieldManagement.ViewModels;
using FieldManagement.Windows;

namespace FieldManagement.View;

public partial class InputView : UserControl
{
    private bool _isDragging;
    private Point _dragStartPoint;
    private Button? _dragTarget;
    private MarkerPosition? _dragMarker;

    public InputView()
    {
        InitializeComponent();
        DataContext = new InputViewModel();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        RenderMarkers();
    }

    private void RenderMarkers()
    {
        OverlayCanvas.Children.Clear();

        if (DataContext is not InputViewModel vm)
            return;

        foreach (var marker in vm.Markers)
        {
            var button = new Button
            {
                Content = marker.Text,
                Tag = marker
            };

            if (TryFindResource("OverlayLampButtonStyle") is Style lampStyle)
                button.Style = lampStyle;

            Canvas.SetLeft(button, marker.X);
            Canvas.SetTop(button, marker.Y);

            button.PreviewMouseLeftButtonDown += Draggable_PreviewMouseLeftButtonDown;
            button.PreviewMouseMove += Draggable_PreviewMouseMove;
            button.PreviewMouseLeftButtonUp += Draggable_PreviewMouseLeftButtonUp;

            OverlayCanvas.Children.Add(button);
        }
    }

    private void Draggable_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not Button button || button.Tag is not MarkerPosition marker)
            return;

        _isDragging = true;
        _dragTarget = button;
        _dragMarker = marker;
        _dragStartPoint = e.GetPosition(OverlayCanvas);

        button.CaptureMouse();
        e.Handled = true;
    }

    private void Draggable_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (!_isDragging || _dragTarget is null || _dragMarker is null)
            return;

        var currentPoint = e.GetPosition(OverlayCanvas);

        double dx = currentPoint.X - _dragStartPoint.X;
        double dy = currentPoint.Y - _dragStartPoint.Y;

        double left = Canvas.GetLeft(_dragTarget);
        double top = Canvas.GetTop(_dragTarget);

        if (double.IsNaN(left)) left = 0;
        if (double.IsNaN(top)) top = 0;

        left += dx;
        top += dy;

        Canvas.SetLeft(_dragTarget, left);
        Canvas.SetTop(_dragTarget, top);

        _dragMarker.X = left;
        _dragMarker.Y = top;

        _dragStartPoint = currentPoint;
    }

    private void Draggable_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (_dragTarget is not null)
            _dragTarget.ReleaseMouseCapture();

        _isDragging = false;
        _dragTarget = null;
        _dragMarker = null;
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is not InputViewModel vm)
            return;

        var popup = new EditWindows(vm.SelectedFloor)
        {
            Owner = Window.GetWindow(this)
        };

        popup.ShowDialog();
        vm.RefreshFloors(popup.SelectedFloorName ?? vm.SelectedFloor);
    }
}
