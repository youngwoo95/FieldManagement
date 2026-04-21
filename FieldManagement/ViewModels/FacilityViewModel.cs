using System.Collections.ObjectModel;
using FieldManagement.Models;

namespace FieldManagement.ViewModels;

public class FacilityViewModel : BaseViewModel
{
    private ObservableCollection<FacilityModel> _facilityItem = new();

    public ObservableCollection<FacilityModel> FacilityItem
    {
        get => _facilityItem;
        set
        {
            _facilityItem = value;
            OnPropertyChanged();
        }
    }

    public FacilityViewModel()
    {
        LoadFacility();
    }

    private void LoadFacility()
    {
        FacilityItem = new ObservableCollection<FacilityModel>
        {
            new FacilityModel
            {
                IsChecked = true,
                Name = "Driller Machine",
                Maker = "Kafo Korea",
                Purpose = "-"
            }
        };
    }
    
    
    
}