using System;
using System.Linq;
using System.Windows;
using PlantManagement.ViewItems;
using PlantManagement.Views.ViewModels.CustomerModel;
using PlantManagement.Views.ViewModels.FacilityModel;
using PlantManagement.Views.Views.Dialogs;

namespace PlantManagement.Views.ViewModels.WorkStatusModel.Dialog;

public class WorkDialogService : IWorkDialogService
{
    private readonly CustomerViewModel _customerViewModel;
    private readonly FacilityViewModel _facilityViewModel;

    public WorkDialogService(
        CustomerViewModel customerViewModel,
        FacilityViewModel facilityViewModel)
    {
        _customerViewModel = customerViewModel;
        _facilityViewModel = facilityViewModel;
    }
    
    public WorkStatusViewItems? ShowAddWorkStatusDialog()
    {
        var addWorkStatusViewModel = new AddWorkStatusViewModel();
        addWorkStatusViewModel.WorkOrderNo = GenerateWorkOrderNo();

        var customerNames = _customerViewModel.Customers
            .Select(x => x.Name)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase);
        addWorkStatusViewModel.SetCustomerNames(customerNames);

        var facilityNames = _facilityViewModel.Facilitys
            .Select(x => x.Name)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase);
        addWorkStatusViewModel.SetFacilityNames(facilityNames);

        var addWindow = new AddWorkStatusWindow(addWorkStatusViewModel)
        {
            Owner = Application.Current?.MainWindow
        };

        var result = addWindow.ShowDialog();
        if (result != true)
            return null;

        return new WorkStatusViewItems()
        {
            WorkOrderNo = string.IsNullOrWhiteSpace(addWorkStatusViewModel.WorkOrderNo)
                ? GenerateWorkOrderNo()
                : addWorkStatusViewModel.WorkOrderNo.Trim(),
            MachineName = addWorkStatusViewModel.MachineName,
            CustomerName = addWorkStatusViewModel.CustomerName,
            Status = string.IsNullOrWhiteSpace(addWorkStatusViewModel.Status)
                ? "InProgress"
                : addWorkStatusViewModel.Status,
            WorkDate = NormalizeWorkDate(addWorkStatusViewModel.WorkDate),
            PdfFileName = string.IsNullOrWhiteSpace(addWorkStatusViewModel.AttachmentFilePath)
                ? "pdf1.pdf"
                : addWorkStatusViewModel.AttachmentFilePath
        };
    }

    /*
     * 자동채번
     */
    private static string GenerateWorkOrderNo()
    {
        return $"WORK_{DateTime.Now:yyyyMMddHHmmss}";
    }

    private static string NormalizeWorkDate(string workDate)
    {
        if (string.IsNullOrWhiteSpace(workDate))
            return DateTime.Today.ToString("yyyy-MM-dd");

        return DateTime.TryParse(workDate, out var date)
            ? date.ToString("yyyy-MM-dd")
            : workDate.Trim();
    }
}
