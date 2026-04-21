using FieldManagement.Commons.DBModels;

namespace FieldManagement.Repository.Customer;

public partial class CustomerRepository : ICustomerRepository
{
    public Task<bool> AddCustomerAsync(CustomerTb model)
    {
        throw new NotImplementedException();
    }
}