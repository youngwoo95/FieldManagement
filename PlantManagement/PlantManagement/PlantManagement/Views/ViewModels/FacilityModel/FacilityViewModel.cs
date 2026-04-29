using System.Windows.Data;
using System.Windows.Input;
using PlantManagement.Comm;
using PlantManagement.Service.v1.Facility;
using PlantManagement.ViewItems;
using PlantManagement.Views.ViewModels;
using PlantManagement.Views.ViewModels.FacilityModel.DialogViews;

namespace PlantManagement.Views.ViewModels.FacilityModel;

public partial class FacilityViewModel : BaseViewModel, IReloadableViewModel
{
    private readonly IFacilityDialogService _facilityDialogService;
    private readonly IFacilityService _facilityService;

    public ICommand EditCommand { get; }
    public ICommand AddCommand { get; }
    public ICommand RemoveCommand { get; }

    public FacilityViewModel(
        IFacilityDialogService facilityDialogService,
        IFacilityService facilityService)
    {
        _facilityDialogService = facilityDialogService;
        _facilityService = facilityService;

        EditCommand = new RelayCommand(_ => EditFacilitys());
        AddCommand = new RelayCommand(_ => AddFacility());
        RemoveCommand = new RelayCommand(_ => _ = RemoveFacilitysAsync());

        _filteredFacilitys = CollectionViewSource.GetDefaultView(_facilitys);
        _filteredFacilitys.Filter = FilterFacility;

        _filteredFacilitys.Refresh();
    }

    public async Task ReloadAsync()
    {
        await LoadFacilitys();
        _filteredFacilitys.Refresh();
    }

    private bool FilterFacility(object item)
    {
        if (item is not FacilityViewItems facility)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(SearchKeyword))
        {
            return true;
        }

        return facility.Name.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase)
               || facility.Maker.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase);
    }

    private async Task LoadFacilitys()
    {
        var model = await _facilityService.GetFacilityService();
        var rows = model ?? [];

        _facilitys.Clear();
        foreach (var item in rows)
        {
            _facilitys.Add(new FacilityViewItems
            {
                Seq = item.facilitySeq,
                Name = item.facilityName ?? string.Empty,
                IsChecked = false,
                Maker = item.maker ?? string.Empty,
                Purpose = item.purpose ?? string.Empty
            });
        }
    }

    private void SearchFacilitys()
    {
        _filteredFacilitys.Refresh();
    }

    private void AddFacility()
    {
        var facility = _facilityDialogService.ShowAddFacilityDialog();
        if (facility is null)
        {
            return;
        }

        _facilitys.Add(facility);
        _filteredFacilitys.Refresh();
    }

    private void EditFacilitys()
    {
        var targets = _facilitys.Where(x => x.IsChecked).ToList();
        if (targets.Count != 1)
        {
            return;
        }

        var target = targets[0];
        var edited = _facilityDialogService.ShowEditFacilityDialog(target);
        if (edited is null)
        {
            return;
        }

        target.Name = edited.Name;
        target.Maker = edited.Maker;
        target.Purpose = edited.Purpose;
        _filteredFacilitys.Refresh();
    }

    private async Task RemoveFacilitysAsync()
    {
        var ids = _facilitys
            .Where(x => x.IsChecked)
            .Select(x => x.Seq)
            .ToList();

        if (ids.Count == 0)
        {
            return;
        }

        var ok = await _facilityService.RemoveFacilityService(ids);
        if (!ok)
        {
            return;
        }

        var targets = _facilitys.Where(x => ids.Contains(x.Seq)).ToList();
        foreach (var target in targets)
        {
            _facilitys.Remove(target);
        }

        _filteredFacilitys.Refresh();
    }
}
