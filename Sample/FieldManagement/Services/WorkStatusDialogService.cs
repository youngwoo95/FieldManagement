using System.Windows;
using FieldManagement.Models;
using FieldManagement.Windows;

namespace FieldManagement.Services;

public class WorkStatusDialogService : IWorkStatusDialogService
{
    public WorkerModel? ShowAddWorkStatusDialog()
    {
        var dialog = new AddWorkStatusWindow
        {
            Owner = Application.Current?.MainWindow
        };

        var result = dialog.ShowDialog();
        if (result != true)
            return null;

        if (string.IsNullOrWhiteSpace(dialog.WorkOrderNo) || string.IsNullOrWhiteSpace(dialog.MachineName))
            return null;

        return new WorkerModel
        {
            WorkOrderNo = dialog.WorkOrderNo,
            MachineName = dialog.MachineName,
            CustomerName = dialog.CustomerName,
            Status = dialog.Status,
            WorkDate = dialog.WorkDate
        };
    }
}
