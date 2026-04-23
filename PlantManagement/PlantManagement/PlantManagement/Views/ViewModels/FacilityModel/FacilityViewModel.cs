using System.Windows.Data;
using System.Windows.Input;
using PlantManagement.Comm;
using PlantManagement.ViewItems;
using PlantManagement.Views.ViewModels.FacilityModel.DialogViews;

namespace PlantManagement.Views.ViewModels.FacilityModel;

public partial class FacilityViewModel : BaseViewModel
{
    private readonly IFacilityDialogService _facilityDialogService;
    public ICommand SearchCommand { get; }
    public ICommand AddCommand { get; }
    public ICommand RemoveCommand { get; }


    public FacilityViewModel(IFacilityDialogService facilityDialogService)
    {
        _facilityDialogService = facilityDialogService;
        
        SearchCommand = new RelayCommand(_ => SearchFacilitys());
        AddCommand = new RelayCommand(_ => AddFacility());
        RemoveCommand = new RelayCommand(_ => RemoveFacilitys());
        
        
        _filteredFacilitys = CollectionViewSource.GetDefaultView(_facilitys);
        _filteredFacilitys.Filter = FilterFacility;

        LoadFacilitys();
        _filteredFacilitys.Refresh();
    }

    private bool FilterFacility(object item)
    {
        if (item is not FacilityViewItems facility) return false;
        if (string.IsNullOrWhiteSpace(SearchKeyword)) return true;

        return facility.Name.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase)
               || facility.Maker.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase);
    }

    private void LoadFacilitys()
    {
        _facilitys.Clear();
        _facilitys.Add(new FacilityViewItems
        {
            IsChecked = false,
            Name = "AESA",
            Maker = "한화시스템",
            Purpose = "감시목적"
        });
        _facilitys.Add(new FacilityViewItems
        {
            IsChecked = false,
            Name = " DC-Link 필름 커패시터",
            Maker = "성호전자",
            Purpose = "전기차용"
        });
        _facilitys.Add(new FacilityViewItems
        {
            IsChecked = false,
            Name = " EVA (Ethylene Vinyl Acetate)",
            Maker = "한화솔루션",
            Purpose = "태양전지"
        });
        _facilitys.Add(new FacilityViewItems
        {
            IsChecked = false,
            Name = " ESS",
            Maker = "SK 이터닉스",
            Purpose = "배터리 저장전치"
        });
    }

    public void SearchFacilitys()
    {
        _filteredFacilitys.Refresh();
    }
    
    
    private void AddFacility()
    {
        _facilityDialogService.ShowAddFacilityDialog();
    }

    private void RemoveFacilitys()
    {
        var targets = _facilitys.Where(x => x.IsChecked).ToList();
        foreach (var target in targets)
        {
            _facilitys.Remove(target);
        }
    }
    
}