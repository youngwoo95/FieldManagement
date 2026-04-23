using System.Collections.ObjectModel;
using System.ComponentModel;
using PlantManagement.ViewItems;

namespace PlantManagement.Views.ViewModels.FacilityModel;

public partial class FacilityViewModel
{
    private readonly ObservableCollection<FacilityViewItems> _facilitys = new();
    private ICollectionView _filteredFacilitys = null!;

    private string _searchKeyword = string.Empty;


    public ObservableCollection<FacilityViewItems> Facilitys => _facilitys;

    public ICollectionView FilteredFacilitys => _filteredFacilitys;


    public string SearchKeyword
    {
        get => _searchKeyword;
        set
        {
            if (_searchKeyword == value)
            {
                return;
            }

            _searchKeyword = value;
            
            OnPropertyChanged();
            _filteredFacilitys.Refresh();
        }
    }


}