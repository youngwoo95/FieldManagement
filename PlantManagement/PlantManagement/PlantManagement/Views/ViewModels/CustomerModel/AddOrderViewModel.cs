using System.Collections.Generic;
using System.Windows.Input;
using PlantManagement.Comm;
using PlantManagement.Views.ViewModels.OrderModel.Dialog;

namespace PlantManagement.Views.ViewModels.CustomerModel;

public partial class AddOrderViewModel : BaseViewModel
{
   public event Action<bool?>? RequestClose;
   
   public ICommand SaveCommand { get; }
   public ICommand CancelCommand { get; }

   public AddOrderViewModel()
   {
      SaveCommand = new RelayCommand(_ => Save());
      CancelCommand = new RelayCommand(_ => Cancel());
   }

   private void Save()
   {
      if (string.IsNullOrWhiteSpace(CustomerName))
      {
         ValidationMessage = "고객사명은 필수입니다.";
         return;
      }

      ValidationMessage = string.Empty;
      RequestClose?.Invoke(true);
   }

   private void Cancel()
   {
      RequestClose?.Invoke(false);  
   }

   public void SetCustomerNames(IEnumerable<string> customerNames)
   {
      CustomerNames.Clear();
      foreach (var customerName in customerNames)
      {
         CustomerNames.Add(customerName);
      }
   }
   
}
