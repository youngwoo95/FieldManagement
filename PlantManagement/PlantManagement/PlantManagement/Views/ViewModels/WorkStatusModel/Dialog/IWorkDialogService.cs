using PlantManagement.ViewItems;

namespace PlantManagement.Views.ViewModels.WorkStatusModel.Dialog;

public interface IWorkDialogService
{
    WorkStatusViewItems? ShowAddWorkStatusDialog();
}