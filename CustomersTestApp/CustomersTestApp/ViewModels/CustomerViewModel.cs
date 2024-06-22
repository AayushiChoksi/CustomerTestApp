using CustomersTestApp.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CustomersTestApp.ViewModels
{
    public class CustomerViewModel : BaseViewModel
    {
        private Customer _customer;

        public CustomerViewModel(Customer customer)
        {
            _customer = customer;
        }

        public string Id => _customer.Id;
        public string Name
        {
            get => _customer.Name;
            set
            {
                if (_customer.Name != value)
                {
                    _customer.Name = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanSave)); // Raise notification for CanSave
                }
            }
        }
        public string Email
        {
            get => _customer.Email;
            set
            {
                if (_customer.Email != value)
                {
                    _customer.Email = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanSave)); // Raise notification for CanSave
                }
            }
        }
        public int Discount
        {
            get => _customer.Discount;
            set
            {
                if (_customer.Discount != value)
                {
                    _customer.Discount = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanSave)); // Raise notification for CanSave
                }
            }
        }
        public bool Can_Remove
        {
            get => _customer.Can_Remove;
            set
            {
                if (_customer.Can_Remove != value)
                {
                    _customer.Can_Remove = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool CanSave => !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Email) && Discount >= 0 && Discount <= 30;
    }
}
