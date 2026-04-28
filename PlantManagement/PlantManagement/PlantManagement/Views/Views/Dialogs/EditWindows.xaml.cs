using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using PlantManagement.ViewItems;
using PlantManagement.Views.ViewModels.EquipmentStatusModel;

namespace PlantManagement.Views.Views.Dialogs;

public partial class EditWindows : Window
{
    private readonly IEquipmentDataService _equipmentDataService;
    private readonly ObservableCollection<EquipmentViewItems> _allEquipmentItems = [];
    private readonly ObservableCollection<EquipmentViewItems> _floorEquipmentItems = [];

    private Point _dragStartPoint;
    private ListBox? _dragSourceListBox;
    private EquipmentViewItems? _dragEquipment;
    private string _selectedFloor = string.Empty;

    public EditWindows(IEquipmentDataService equipmentDataService)
    {
        InitializeComponent();

        _equipmentDataService = equipmentDataService;
        AllEquipmentListBox.ItemsSource = _allEquipmentItems;
        FloorEquipmentListBox.ItemsSource = _floorEquipmentItems;

        RefreshFloorComboBox();
        FloorMessageText.Text = "층을 선택해서 장비를 배치해 주세요.";
    }

    private void SearchFloorButton_Click(object sender, RoutedEventArgs e)
    {
        var inputFloor = FloorNameTextBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(inputFloor))
        {
            FloorMessageText.Text = "조회할 층명을 입력해 주세요.";
            return;
        }

        var targetFloor = _equipmentDataService.Floors.FirstOrDefault(
            floor => string.Equals(floor, inputFloor, StringComparison.OrdinalIgnoreCase));

        if (targetFloor is null)
        {
            FloorMessageText.Text = $"층을 찾을 수 없습니다: {inputFloor}";
            return;
        }

        FloorComboBox.SelectedItem = targetFloor;
        FloorMessageText.Text = $"조회 완료: {targetFloor}";
    }

    private void CreateFloorButton_Click(object sender, RoutedEventArgs e)
    {
        var inputFloor = FloorNameTextBox.Text.Trim();
        if (_equipmentDataService.TryAddFloor(inputFloor, out var message))
        {
            RefreshFloorComboBox(inputFloor);
        }

        FloorMessageText.Text = message;
    }

    private void FloorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (FloorComboBox.SelectedItem is not string floorName)
        {
            return;
        }

        LoadFloorEquipment(floorName);
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_selectedFloor))
        {
            FloorMessageText.Text = "선택된 층이 없습니다.";
            return;
        }

        if (_equipmentDataService.UpdateFloorEquipments(_selectedFloor, _floorEquipmentItems, out var message))
        {
            FloorMessageText.Text = message;
            DialogResult = true;
            Close();
            return;
        }

        FloorMessageText.Text = message;
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void EquipmentListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        _dragStartPoint = e.GetPosition(null);
        _dragSourceListBox = sender as ListBox;
        _dragEquipment = FindEquipmentFromSource(e.OriginalSource);
    }

    private void EquipmentListBox_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton != MouseButtonState.Pressed || _dragSourceListBox is null || _dragEquipment is null)
        {
            return;
        }

        var mousePosition = e.GetPosition(null);
        var xDiff = Math.Abs(mousePosition.X - _dragStartPoint.X);
        var yDiff = Math.Abs(mousePosition.Y - _dragStartPoint.Y);

        if (xDiff < SystemParameters.MinimumHorizontalDragDistance &&
            yDiff < SystemParameters.MinimumVerticalDragDistance)
        {
            return;
        }

        var dragPayload = new EquipmentDragPayload(_dragEquipment, _dragSourceListBox);
        DragDrop.DoDragDrop(_dragSourceListBox, dragPayload, DragDropEffects.Move);
    }

    private void EquipmentListBox_DragOver(object sender, DragEventArgs e)
    {
        if (sender is not ListBox targetListBox)
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;
            return;
        }

        if (e.Data.GetData(typeof(EquipmentDragPayload)) is not EquipmentDragPayload payload ||
            ReferenceEquals(payload.SourceListBox, targetListBox))
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;
            return;
        }

        e.Effects = DragDropEffects.Move;
        e.Handled = true;
    }

    private void EquipmentListBox_Drop(object sender, DragEventArgs e)
    {
        if (sender is not ListBox targetListBox)
        {
            return;
        }

        if (e.Data.GetData(typeof(EquipmentDragPayload)) is not EquipmentDragPayload payload ||
            ReferenceEquals(payload.SourceListBox, targetListBox))
        {
            return;
        }

        var sourceCollection = ResolveCollection(payload.SourceListBox);
        var targetCollection = ResolveCollection(targetListBox);
        if (sourceCollection is null || targetCollection is null)
        {
            return;
        }

        var movedItem = sourceCollection.FirstOrDefault(item => item.Id == payload.Item.Id);
        if (movedItem is null)
        {
            return;
        }

        sourceCollection.Remove(movedItem);
        targetCollection.Add(movedItem);
        FloorMessageText.Text = "변경되었습니다. 저장 버튼을 눌러 적용해 주세요.";
    }

    private void RefreshFloorComboBox(string? preferredFloor = null)
    {
        var floors = _equipmentDataService.Floors.ToList();
        FloorComboBox.ItemsSource = floors;

        if (floors.Count == 0)
        {
            _selectedFloor = string.Empty;
            _allEquipmentItems.Clear();
            _floorEquipmentItems.Clear();
            return;
        }

        var targetFloor = preferredFloor;
        if (string.IsNullOrWhiteSpace(targetFloor) || !floors.Contains(targetFloor))
        {
            targetFloor = floors[0];
        }

        FloorComboBox.SelectedItem = targetFloor;
    }

    private void LoadFloorEquipment(string floorName)
    {
        _selectedFloor = floorName;
        FloorNameTextBox.Text = floorName;

        var floorEquipments = _equipmentDataService.GetFloorEquipments(floorName).ToList();
        var assignedIds = floorEquipments.Select(item => item.Id).ToHashSet();

        _floorEquipmentItems.Clear();
        foreach (var item in floorEquipments)
        {
            _floorEquipmentItems.Add(item);
        }

        _allEquipmentItems.Clear();
        foreach (var item in _equipmentDataService.GetAllEquipments().Where(item => !assignedIds.Contains(item.Id)))
        {
            _allEquipmentItems.Add(item);
        }
    }

    private ObservableCollection<EquipmentViewItems>? ResolveCollection(ListBox listBox)
    {
        if (ReferenceEquals(listBox, AllEquipmentListBox))
        {
            return _allEquipmentItems;
        }

        if (ReferenceEquals(listBox, FloorEquipmentListBox))
        {
            return _floorEquipmentItems;
        }

        return null;
    }

    private static EquipmentViewItems? FindEquipmentFromSource(object source)
    {
        var current = source as DependencyObject;
        while (current is not null)
        {
            if (current is FrameworkElement element && element.DataContext is EquipmentViewItems equipment)
            {
                return equipment;
            }

            current = VisualTreeHelper.GetParent(current);
        }

        return null;
    }

    private sealed class EquipmentDragPayload(EquipmentViewItems item, ListBox sourceListBox)
    {
        public EquipmentViewItems Item { get; } = item;
        public ListBox SourceListBox { get; } = sourceListBox;
    }
}
