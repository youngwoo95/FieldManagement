using FieldManagement.Models;

namespace FieldManagement.Services;

public interface ICustomerDialogService
{
    CustomerModel? ShowAddCustomerDialog();
}
