using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using PlantManagement.Comm;

namespace PlantManagement.Views.ViewModels.CustomerModel;

public partial class AddWorkStatusViewModel : BaseViewModel
{
    public event Action<bool?>? RequestClose;

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddWorkStatusViewModel()
    {
        SaveCommand = new RelayCommand(_ => Save());
        CancelCommand = new RelayCommand(_ => Cancel());
    }

    private void Save()
    {
        if (SelectedOrderInfo is null)
        {
            ValidationMessage = "수주 정보를 선택해 주세요.";
            return;
        }

        if (string.IsNullOrWhiteSpace(CustomerName))
        {
            ValidationMessage = "고객명이 비어 있습니다.";
            return;
        }

        ValidationMessage = string.Empty;
        RequestClose?.Invoke(true);
    }

    private void Cancel()
    {
        RequestClose?.Invoke(false);
    }

    public void SetCustomerNames(IEnumerable<string> customerNames)
    {
        CustomerNames.Clear();
        foreach (var customerName in customerNames)
        {
            CustomerNames.Add(customerName);
        }
    }

    public void SetFacilityNames(IEnumerable<string> facilityNames)
    {
        FacilityNames.Clear();
        foreach (var facilityName in facilityNames)
        {
            FacilityNames.Add(facilityName);
        }
    }

    public void SetOrderInfos(IEnumerable<WorkOrderInfoOption> orderInfos)
    {
        OrderInfos.Clear();
        foreach (var orderInfo in orderInfos)
        {
            OrderInfos.Add(orderInfo);
        }

        SelectedOrderInfo = OrderInfos.FirstOrDefault();
    }
}
