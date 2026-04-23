using System;
using System.Windows.Input;
using PlantManagement.Comm;

namespace PlantManagement.Views.ViewModels.CustomerModel;

public partial class AddCustomerViewModel : BaseViewModel
{
    public event Action<bool?>? RequestClose;

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddCustomerViewModel()
    {
        SaveCommand = new RelayCommand(_ => Save());
        CancelCommand = new RelayCommand(_ => Cancel());
    }

    private void Save()
    {
        if (string.IsNullOrWhiteSpace(CustomerName))
        {
            ValidationMessage = "고객사명은 필수입니다.";
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
