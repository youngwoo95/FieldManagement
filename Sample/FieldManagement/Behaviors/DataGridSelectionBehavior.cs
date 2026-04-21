using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace FieldManagement.Behaviors;

public static class DataGridSelectionBehavior
{
    public static readonly DependencyProperty ClearSelectionOnCheckBoxClickProperty =
        DependencyProperty.RegisterAttached(
            "ClearSelectionOnCheckBoxClick",
            typeof(bool),
            typeof(DataGridSelectionBehavior),
            new PropertyMetadata(false, OnClearSelectionOnCheckBoxClickChanged));

    public static void SetClearSelectionOnCheckBoxClick(DependencyObject element, bool value)
    {
        element.SetValue(ClearSelectionOnCheckBoxClickProperty, value);
    }

    public static bool GetClearSelectionOnCheckBoxClick(DependencyObject element)
    {
        return (bool)element.GetValue(ClearSelectionOnCheckBoxClickProperty);
    }

    private static void OnClearSelectionOnCheckBoxClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not DataGrid dataGrid)
        {
            return;
        }

        if ((bool)e.NewValue)
        {
            dataGrid.PreviewMouseLeftButtonDown += DataGridOnPreviewMouseLeftButtonDown;
        }
        else
        {
            dataGrid.PreviewMouseLeftButtonDown -= DataGridOnPreviewMouseLeftButtonDown;
        }
    }

    private static void DataGridOnPreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (sender is not DataGrid dataGrid || e.OriginalSource is not DependencyObject source)
        {
            return;
        }

        if (FindAncestor<CheckBox>(source) is null)
        {
            return;
        }

        dataGrid.Dispatcher.BeginInvoke(() =>
        {
            dataGrid.UnselectAll();
            dataGrid.SelectedItem = null;
        }, DispatcherPriority.Background);
    }

    private static T? FindAncestor<T>(DependencyObject? current) where T : DependencyObject
    {
        while (current is not null)
        {
            if (current is T match)
            {
                return match;
            }

            current = VisualTreeHelper.GetParent(current);
        }

        return null;
    }
}
