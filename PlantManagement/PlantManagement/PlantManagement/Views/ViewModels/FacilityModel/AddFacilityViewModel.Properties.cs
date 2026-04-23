namespace PlantManagement.Views.ViewModels.FacilityModel;

public partial class AddFacilityViewModel
{
    private string _facilityName = string.Empty;
    private string _maker = string.Empty;
    private string _purpose = string.Empty;
    private string _validationMessage = string.Empty;

    public string FacilityName
    {
        get => _facilityName;
        set => SetField(ref _facilityName, value);
    }

    public string Maker
    {
        get => _maker;
        set => SetField(ref _maker, value);
    }

    public string Purpose
    {
        get => _purpose;
        set => SetField(ref _purpose, value);
    }

    public string ValidationMessage
    {
        get => _validationMessage;
        set => SetField(ref _validationMessage, value);
    }
}