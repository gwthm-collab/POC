using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using POC.Common;
using POC.MVVM.Model;

namespace POC.MVVM.ViewModel
{
    class LoadCustomerVM : ViewModelBase
    {
        Func<CustomerEventList_t, object> triggerEvent;

        #region Bind Property
        private ICommand _AddPeople;
        public ICommand AddPeople
        {
            get { return _AddPeople; }
            set { _AddPeople = value; OnPropertyChanged("AddPeople"); }
        }

        private ICommand _CancelPeople;
        public ICommand CancelPeople
        {
            get { return _CancelPeople; }
            set { _CancelPeople = value; OnPropertyChanged("CancelPeople"); }
        }

        private ICommand _textBoxChanged;
        public ICommand textBoxChanged
        {
            get { return _textBoxChanged; }
            set { _textBoxChanged = value; OnPropertyChanged("textBoxChanged"); }
        }

        private string _CustomerName;
        public string CustomerName
        {
            get { return _CustomerName; }
            set { _CustomerName = value; OnPropertyChanged("CustomerName"); }
        }

        private string _CusMobileNum;
        public string CusMobileNum
        {
            get { return _CusMobileNum; }
            set
            {
                _CusMobileNum = value;
                OnPropertyChanged("CusMobileNum");
            }
        }

        private string _CusAddress;
        public string CusAddress
        {
            get { return _CusAddress; }
            set { _CusAddress = value; OnPropertyChanged("CusAddress"); }
        }

        private string _CusWhatsappNum;
        public string CusWhatsappNum
        {
            get { return _CusWhatsappNum; }
            set { _CusWhatsappNum = value; OnPropertyChanged("CusWhatsappNum"); }
        }

        private string _EmailID;
        public string EmailID
        {
            get { return _EmailID; }
            set { _EmailID = value; OnPropertyChanged("EmailID"); }
        }

        private bool _isAddEnabled = false;
        public bool isAddEnabled
        {
            get { return _isAddEnabled; }
            set { _isAddEnabled = value; OnPropertyChanged("isAddEnabled"); }
        }

        #endregion

        public LoadCustomerVM(Func<CustomerEventList_t, object> events)
        {
            triggerEvent += events;
            AddPeople = new RelayCommand(AddFromForm);
            CancelPeople = new RelayCommand((obj) => _ = triggerEvent(CustomerEventList_t.cancelPeople));
            textBoxChanged = new RelayCommand((obj) => isAddEnabled = true);
        }

        private void AddFromForm(object obj)
        {
            Customer customer = new Customer();
            customer.Name = CustomerName;
            customer.MobileNumber = CusMobileNum;
            customer.Address = CusAddress;
            customer.TelegramNumber = CusWhatsappNum;
            customer.Email_ID = EmailID;
            
            //Event to DB
            bool status = DBConnector.SendToDB(string.Format(DB_StoredProcedures.CUSTOMER_INSERT, customer.Name, customer.MobileNumber, customer.Address,
                customer.TelegramNumber, customer.Email_ID));

            if (status)
            {
                MessageBox.Show($"Customer: {CustomerName}\nAdded Successfully.", "Add Customer", MessageBoxButton.OK, MessageBoxImage.Information);
                CustomerName = string.Empty;
                CusMobileNum = string.Empty;
                CusAddress = string.Empty;
                CusWhatsappNum = string.Empty;
                EmailID = string.Empty;
            }
            else
            {
                MessageBox.Show($"Customer: {CustomerName}\nError in adding customer.", "Add Customer", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
