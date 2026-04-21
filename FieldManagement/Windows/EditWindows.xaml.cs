using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using FieldManagement.Services;

namespace FieldManagement.Windows;

public partial class EditWindows : Window
{
    private const string EquipmentDataFormat = "DraggedEquipmentName";

    private readonly FloorManagementService _floorManagementService = FloorManagementService.Instance;
    private readonly ObservableCollection<string> _allEquipmentItems = new();
    private readonly ObservableCollection<string> _floorEquipmentItems = new();

    private Point _dragStartPoint;

    public string? SelectedFloorName { get; private set; }

    public EditWindows()
        : this(null)
    {
    }

    public EditWindows(string? preferredFloorName)
    {
        InitializeComponent();

        AllEquipmentListBox.ItemsSource = _allEquipmentItems;
        FloorEquipmentListBox.ItemsSource = _floorEquipmentItems;

        LoadFloorComboBox(preferredFloorName);
        CreateFloorButton.IsEnabled = false;
        FloorMessageText.Text = "층명을 입력하고 조회하세요.";

        if (!string.IsNullOrWhiteSpace(preferredFloorName) &&
            _floorManagementService.FindExactFloorName(preferredFloorName) is not null)
            LoadEquipmentLists(preferredFloorName);
    }

    private void LoadFloorComboBox(string? preferredFloorName = null)
    {
        var floorNames = _floorManagementService.GetFloorNames();
        FloorComboBox.ItemsSource = floorNames;

        if (!string.IsNullOrWhiteSpace(preferredFloorName) && floorNames.Contains(preferredFloorName))
        {
            FloorComboBox.SelectedItem = preferredFloorName;
            return;
        }

        if (floorNames.Count > 0 && FloorComboBox.SelectedItem is null)
            FloorComboBox.SelectedIndex = 0;
    }

    private void LoadEquipmentLists(string floorName)
    {
        SelectedFloorName = floorName;
        FloorNameTextBox.Text = floorName;

        _allEquipmentItems.Clear();
        foreach (var equipment in _floorManagementService.GetEquipmentsExceptFloor(floorName))
            _allEquipmentItems.Add(equipment);

        _floorEquipmentItems.Clear();
        foreach (var equipment in _floorManagementService.GetEquipmentsForFloor(floorName))
            _floorEquipmentItems.Add(equipment);
    }

    private void SearchFloorButton_Click(object sender, RoutedEventArgs e)
    {
        var keyword = FloorNameTextBox.Text?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(keyword))
        {
            FloorMessageText.Text = "조회할 층명을 입력해주세요.";
            CreateFloorButton.IsEnabled = false;
            return;
        }

        var exactFloorName = _floorManagementService.FindExactFloorName(keyword);
        if (exactFloorName is not null)
        {
            LoadFloorComboBox(exactFloorName);
            LoadEquipmentLists(exactFloorName);
            CreateFloorButton.IsEnabled = false;
            FloorMessageText.Text = $"'{exactFloorName}' 조회 완료";
            return;
        }

        var partialMatches = _floorManagementService.SearchFloorNames(keyword);
        LoadFloorComboBox();

        if (partialMatches.Count > 0)
        {
            FloorComboBox.SelectedItem = partialMatches[0];
            FloorMessageText.Text = $"일치 층이 없어 유사 결과 {partialMatches.Count}건을 표시합니다.";
            CreateFloorButton.IsEnabled = true;
            return;
        }

        SelectedFloorName = null;
        _allEquipmentItems.Clear();
        _floorEquipmentItems.Clear();
        FloorMessageText.Text = $"'{keyword}' 층이 없습니다. 생성 버튼으로 추가하세요.";
        CreateFloorButton.IsEnabled = true;
    }

    private void CreateFloorButton_Click(object sender, RoutedEventArgs e)
    {
        var name = FloorNameTextBox.Text?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(name))
        {
            FloorMessageText.Text = "생성할 층명을 입력해주세요.";
            return;
        }

        try
        {
            var createdFloorName = _floorManagementService.CreateFloor(name);
            LoadFloorComboBox(createdFloorName);
            LoadEquipmentLists(createdFloorName);
            CreateFloorButton.IsEnabled = false;
            FloorMessageText.Text = $"'{createdFloorName}' 층이 생성되었습니다.";
        }
        catch (Exception ex)
        {
            FloorMessageText.Text = ex.Message;
        }
    }

    private void FloorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (FloorComboBox.SelectedItem is not string floorName || string.IsNullOrWhiteSpace(floorName))
            return;

        LoadEquipmentLists(floorName);
        CreateFloorButton.IsEnabled = false;
        FloorMessageText.Text = $"'{floorName}' 편집 중";
    }

    private void EquipmentListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        _dragStartPoint = e.GetPosition(this);

        if (sender is not ListBox sourceListBox)
            return;

        var container = FindParent<ListBoxItem>(e.OriginalSource as DependencyObject);
        sourceListBox.SelectedItem = container?.DataContext;
    }

    private void EquipmentListBox_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton != MouseButtonState.Pressed)
            return;

        if (sender is not ListBox sourceListBox || sourceListBox.SelectedItem is not string equipmentName)
            return;

        var currentPos = e.GetPosition(this);
        var delta = currentPos - _dragStartPoint;
        if (Math.Abs(delta.X) < SystemParameters.MinimumHorizontalDragDistance &&
            Math.Abs(delta.Y) < SystemParameters.MinimumVerticalDragDistance)
        {
            return;
        }

        var data = new DataObject();
        data.SetData("SourceListBox", sourceListBox);
        data.SetData(EquipmentDataFormat, equipmentName);
        DragDrop.DoDragDrop(sourceListBox, data, DragDropEffects.Move);
    }

    private void EquipmentListBox_DragOver(object sender, DragEventArgs e)
    {
        if (sender is not ListBox targetListBox)
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;
            return;
        }

        var sourceListBox = e.Data.GetData("SourceListBox") as ListBox;
        var equipmentName = e.Data.GetData(EquipmentDataFormat) as string;
        if (sourceListBox is null || string.IsNullOrWhiteSpace(equipmentName) || ReferenceEquals(sourceListBox, targetListBox))
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
            return;

        var sourceListBox = e.Data.GetData("SourceListBox") as ListBox;
        var equipmentName = e.Data.GetData(EquipmentDataFormat) as string;
        if (sourceListBox is null || string.IsNullOrWhiteSpace(equipmentName) || ReferenceEquals(sourceListBox, targetListBox))
            return;

        var sourceCollection = sourceListBox.ItemsSource as ObservableCollection<string>;
        var targetCollection = targetListBox.ItemsSource as ObservableCollection<string>;
        if (sourceCollection is null || targetCollection is null)
            return;

        if (sourceCollection.Contains(equipmentName))
            sourceCollection.Remove(equipmentName);

        if (!targetCollection.Contains(equipmentName))
            targetCollection.Add(equipmentName);
    }

    private static T? FindParent<T>(DependencyObject? child) where T : DependencyObject
    {
        while (child is not null)
        {
            if (child is T parent)
                return parent;

            child = VisualTreeHelper.GetParent(child);
        }

        return null;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(SelectedFloorName))
        {
            FloorMessageText.Text = "저장할 층을 먼저 조회하거나 선택해주세요.";
            return;
        }

        _floorManagementService.SaveFloorEquipments(SelectedFloorName, _floorEquipmentItems);
        FloorMessageText.Text = "저장되었습니다.";
        DialogResult = true;
        Close();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
