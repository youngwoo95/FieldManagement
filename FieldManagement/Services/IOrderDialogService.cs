using FieldManagement.Models;

namespace FieldManagement.Services;

public interface IOrderDialogService
{
    OrderModel? ShowAddOrderDialog();
}
