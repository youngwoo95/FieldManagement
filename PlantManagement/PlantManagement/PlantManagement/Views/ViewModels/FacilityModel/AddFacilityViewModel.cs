using System.Windows.Input;
using PlantManagement.Comm;
using PlantManagement.Dto.v1.Facility;
using PlantManagement.Service.v1.Facility;
using PlantManagement.ViewItems;

namespace PlantManagement.Views.ViewModels.FacilityModel;

public partial class AddFacilityViewModel : BaseViewModel
{
    private readonly IFacilityService _facilityService;
    private bool _isEditMode;
    private int _editingFacilitySeq;

    public event Action<bool?>? RequestClose;

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddFacilityViewModel(IFacilityService facilityService)
    {
        _facilityService = facilityService;
        SaveCommand = new RelayCommand(_ => _ = SaveAsync());
        CancelCommand = new RelayCommand(_ => Cancel());
    }

    public void PrepareForAdd()
    {
        _isEditMode = false;
        _editingFacilitySeq = 0;

        FacilityName = string.Empty;
        Maker = string.Empty;
        Purpose = string.Empty;
        ValidationMessage = string.Empty;
    }

    public void PrepareForEdit(FacilityViewItems target)
    {
        _isEditMode = true;
        _editingFacilitySeq = target.Seq;

        FacilityName = target.Name;
        Maker = target.Maker;
        Purpose = target.Purpose;
        ValidationMessage = string.Empty;
    }

    public FacilityViewItems BuildFacilityViewItem()
    {
        return new FacilityViewItems
        {
            Seq = _editingFacilitySeq,
            IsChecked = false,
            Name = FacilityName,
            Maker = Maker,
            Purpose = Purpose
        };
    }

    private async Task SaveAsync()
    {
        if (!ValidateInput())
        {
            return;
        }

        if (_isEditMode)
        {
            var editDto = new EditFacilityDto
            {
                facilitySeq = _editingFacilitySeq,
                facilityName = FacilityName,
                maker = Maker,
                purpose = Purpose
            };

            var editResult = await _facilityService.EditFacilityService(editDto);
            if (!editResult)
            {
                ValidationMessage = "수정에 실패했습니다.";
                return;
            }

            RequestClose?.Invoke(true);
            return;
        }

        var dtoModel = new AddFacilityDto
        {
            facilityName = FacilityName,
            maker = Maker,
            purpose = Purpose
        };

        var addResult = await _facilityService.AddFacilityService(dtoModel);
        if (!addResult)
        {
            ValidationMessage = "저장에 실패했습니다.";
            return;
        }

        RequestClose?.Invoke(true);
    }

    private void Cancel()
    {
        RequestClose?.Invoke(false);
    }

    private bool ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(FacilityName))
        {
            ValidationMessage = "설비명은 필수입니다.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(Maker))
        {
            ValidationMessage = "제조사는 필수입니다.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(Purpose))
        {
            ValidationMessage = "용도는 필수입니다.";
            return false;
        }

        ValidationMessage = string.Empty;
        return true;
    }
}
