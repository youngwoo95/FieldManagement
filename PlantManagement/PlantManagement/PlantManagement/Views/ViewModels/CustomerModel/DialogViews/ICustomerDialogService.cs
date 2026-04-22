using PlantManagement.ViewItems;

namespace PlantManagement.Views.ViewModels.CustomerModel.DialogViews;

public interface ICustomerDialogService
{
    CustomerViewItems? ShowAddCustomerDialog();
}