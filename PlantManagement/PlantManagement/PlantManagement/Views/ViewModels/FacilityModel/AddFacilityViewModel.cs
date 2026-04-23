using System.Windows.Input;
using PlantManagement.Comm;

namespace PlantManagement.Views.ViewModels.FacilityModel;

public partial class AddFacilityViewModel : BaseViewModel
{
    public event Action<bool?>? RequestClose;
    
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddFacilityViewModel()
    {
        SaveCommand = new RelayCommand(_ => Save());
        CancelCommand = new RelayCommand(_ => Cancel());
    }

    private void Save()
    {
        if (string.IsNullOrWhiteSpace(FacilityName))
        {
            ValidationMessage = "설비명은 필수입니다.";
            return;
        }

        ValidationMessage = string.Empty;
        RequestClose?.Invoke(true);
    }

    private void Cancel()
    {
        RequestClose?.Invoke(false);
    }

}