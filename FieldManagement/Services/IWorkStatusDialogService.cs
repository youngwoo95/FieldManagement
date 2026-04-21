using FieldManagement.Models;

namespace FieldManagement.Services;

public interface IWorkStatusDialogService
{
    WorkerModel? ShowAddWorkStatusDialog();
}
