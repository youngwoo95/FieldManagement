using PlantManagement.ViewItems;

namespace PlantManagement.Views.ViewModels.OrderModel.Dialog;

public interface IOrderDialogService
{
    OrderViewItems? ShowAddOrderDialog();
    OrderViewItems? ShowEditOrderDialog(OrderViewItems target);
}
