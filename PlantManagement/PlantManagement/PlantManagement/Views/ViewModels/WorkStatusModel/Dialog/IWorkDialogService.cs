using PlantManagement.ViewItems;

namespace PlantManagement.Views.ViewModels.WorkStatusModel.Dialog;

public interface IWorkDialogService
{
    Task<WorkStatusViewItems?> ShowAddWorkStatusDialogAsync();
}
