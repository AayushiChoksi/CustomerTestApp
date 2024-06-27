using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using CustomersTestApp.Commands;
using CustomersTestApp.Messaging;
using CustomersTestApp.Models;

namespace CustomersTestApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<CustomerViewModel> _customers;
        private ObservableCollection<CustomerViewModel> _allCustomers;
        private CustomerViewModel _selectedCustomer;
        private CustomerViewModel _editableCustomer;
        private string _filterText;
        private string _selectedFilterOption;

        private string _newCustomerName;
        private string _newCustomerEmail;
        private string _newCustomerDiscount;
        private bool _canAddCustomer;

        public ObservableCollection<CustomerViewModel> Customers
        {
            get { return _customers; }
            set { _customers = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> FilterOptions { get; } = new ObservableCollection<string> { "Name", "Email" };

        public CustomerViewModel SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged();
                if (_selectedCustomer != null)
                {
                    EditableCustomer = new CustomerViewModel(new Customer(
                        _selectedCustomer.Id,  // Id is treated as a string
                        _selectedCustomer.Name,
                        _selectedCustomer.Email,
                        _selectedCustomer.Discount,
                        _selectedCustomer.Can_Remove));

                    EditableCustomer.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == nameof(EditableCustomer.CanSave))
                        {
                            ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
                        }
                    };
                }
                else
                {
                    EditableCustomer = null;
                }
                ((RelayCommand)RemoveCustomerCommand).RaiseCanExecuteChanged();
                ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public CustomerViewModel EditableCustomer
        {
            get { return _editableCustomer; }
            set { _editableCustomer = value; OnPropertyChanged(); }
        }

        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }

        public string SelectedFilterOption
        {
            get => _selectedFilterOption;
            set
            {
                _selectedFilterOption = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }

        public string NewCustomerName
        {
            get => _newCustomerName;
            set
            {
                _newCustomerName = value;
                OnPropertyChanged();
                ValidateAddCustomer();
            }
        }

        public string NewCustomerEmail
        {
            get => _newCustomerEmail;
            set
            {
                _newCustomerEmail = value;
                OnPropertyChanged();
                ValidateAddCustomer();
            }
        }

        public string NewCustomerDiscount
        {
            get => _newCustomerDiscount;
            set
            {
                _newCustomerDiscount = value;
                OnPropertyChanged();
                ValidateAddCustomer();
            }
        }

        public bool CanAddCustomer
        {
            get => _canAddCustomer;
            set
            {
                _canAddCustomer = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCustomerCommand { get; }
        public ICommand RemoveCustomerCommand { get; }
        public ICommand SaveCommand { get; }

        public string Description => "List of customers with filter";

        public MainViewModel()
        {
            _allCustomers = new ObservableCollection<CustomerViewModel>();
            Customers = new ObservableCollection<CustomerViewModel>();
            AddCustomerCommand = new RelayCommand(AddCustomer, () => CanAddCustomer);
            RemoveCustomerCommand = new RelayCommand(RemoveCustomer, CanRemoveCustomer);
            SaveCommand = new RelayCommand(SaveCustomer, CanSaveCustomer);

            // Example data
            _allCustomers.Add(new CustomerViewModel(new Customer { Name = "John Doe", Email = "john.doe@example.com", Discount = 10, Can_Remove = false }));
            _allCustomers.Add(new CustomerViewModel(new Customer { Name = "Jane Smith", Email = "jane.smith@example.com", Discount = 15, Can_Remove = false }));
            ApplyFilter();

            Messenger.Instance.Register<CustomerRemovedMessage>(OnCustomerRemoved);
            Messenger.Instance.Register<CustomerAddedMessage>(OnCustomerAdded);
        }

        private void AddCustomer()
        {
            var newCustomer = new CustomerViewModel(new Customer
            {
                Name = NewCustomerName,
                Email = NewCustomerEmail,
                Discount = int.Parse(NewCustomerDiscount),
                Can_Remove = true
            });
            _allCustomers.Add(newCustomer);
            ApplyFilter();
            Messenger.Instance.Send(new CustomerAddedMessage(newCustomer));

            // Reset fields
            NewCustomerName = string.Empty;
            NewCustomerEmail = string.Empty;
            NewCustomerDiscount = string.Empty;
        }

        private void RemoveCustomer()
        {
            if (SelectedCustomer != null && SelectedCustomer.Can_Remove)
            {
                var customerToRemove = SelectedCustomer;
                _allCustomers.Remove(customerToRemove);
                ApplyFilter();
                Messenger.Instance.Send(new CustomerRemovedMessage(customerToRemove));
                SelectedCustomer = null;  // Clear the selection
                EditableCustomer = null;  // Clear the editable customer details
            }
        }

        private bool CanRemoveCustomer()
        {
            return SelectedCustomer != null && SelectedCustomer.Can_Remove;
        }

        private void SaveCustomer()
        {
            if (SelectedCustomer != null && EditableCustomer != null)
            {
                SelectedCustomer.Name = EditableCustomer.Name;
                SelectedCustomer.Email = EditableCustomer.Email;
                SelectedCustomer.Discount = EditableCustomer.Discount;
                ApplyFilter();
                Messenger.Instance.Send(new CustomerUpdatedMessage(SelectedCustomer));
            }
        }

        private bool CanSaveCustomer()
        {
            return EditableCustomer != null && EditableCustomer.CanSave;
        }

        private void ValidateAddCustomer()
        {
            if (string.IsNullOrWhiteSpace(NewCustomerName) ||
                string.IsNullOrWhiteSpace(NewCustomerEmail) ||
                !new EmailAddressAttribute().IsValid(NewCustomerEmail) ||
                !int.TryParse(NewCustomerDiscount, out int discount) ||
                discount < 0 || discount > 30)
            {
                CanAddCustomer = false;
            }
            else
            {
                CanAddCustomer = true;
            }
            ((RelayCommand)AddCustomerCommand).RaiseCanExecuteChanged();
        }

        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(FilterText))
            {
                Customers = new ObservableCollection<CustomerViewModel>(_allCustomers);
            }
            else
            {
                switch (SelectedFilterOption)
                {
                    case "Name":
                        Customers = new ObservableCollection<CustomerViewModel>(_allCustomers.Where(c => c.Name.Contains(FilterText, StringComparison.OrdinalIgnoreCase)));
                        break;
                    case "Email":
                        Customers = new ObservableCollection<CustomerViewModel>(_allCustomers.Where(c => c.Email.Contains(FilterText, StringComparison.OrdinalIgnoreCase)));
                        break;
                    default:
                        Customers = new ObservableCollection<CustomerViewModel>(_allCustomers);
                        break;
                }
            }
        }

        private void OnCustomerRemoved(CustomerRemovedMessage message)
        {
            // Handle customer removal logic if necessary
        }

        private void OnCustomerAdded(CustomerAddedMessage message)
        {
            // Handle customer addition logic if necessary
        }
    }
}
