using POC.Common;
using POC.MVVM.Model;
using POC.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POC.MVVM.ViewModel
{
    class UpdateCustomerVM : ViewModelBase
    {
        private DataTable usersFromDB;
        LoadCustomer loadCustomerWindow;

        #region Bind Property
        private string _CustomerName;
        public string CustomerName
        {
            get { return _CustomerName; }
            set { _CustomerName = value; OnPropertyChanged("CustomerName"); }
        }

        private string _CustomerMobile;
        public string CustomerMobile
        {
            get { return _CustomerMobile; }
            set { _CustomerMobile = value; OnPropertyChanged("CustomerMobile"); }
        }

        private string _CustomerAddress;
        public string CustomerAddress
        {
            get { return _CustomerAddress; }
            set { _CustomerAddress = value; OnPropertyChanged("CustomerAddress"); }
        }

        private string _CustomerTelegram;
        public string CustomerTelegram
        {
            get { return _CustomerTelegram; }
            set { _CustomerTelegram = value; OnPropertyChanged("CustomerTelegram"); }
        }

        private string _CustomerEmail;
        public string CustomerEmail
        {
            get { return _CustomerEmail; }
            set { _CustomerEmail = value; OnPropertyChanged("CustomerEmail"); }
        }

        private ICommand _BtnUpdateDetails;
        public ICommand BtnUpdateDetails
        {
            get { return _BtnUpdateDetails; }
            set { _BtnUpdateDetails = value; OnPropertyChanged("BtnUpdateDetails"); }
        }

        private ICommand _CustomerListSelection;
        public ICommand CustomerListSelection
        {
            get { return _CustomerListSelection; }
            set { _CustomerListSelection = value; OnPropertyChanged("CustomerListSelection"); }
        }

        private ICommand _TextInput;
        public ICommand TextInput
        {
            get { return _TextInput; }
            set { _TextInput = value; OnPropertyChanged("TextInput"); }
        }

        private ICommand _AddCustomer;
        public ICommand AddCustomer
        {
            get { return _AddCustomer; }
            set { _AddCustomer = value; OnPropertyChanged("AddCustomer"); }
        }

        private bool _isUpdateEnabled = false;
        public bool isUpdateEnabled
        {
            get { return _isUpdateEnabled; }
            set { _isUpdateEnabled = value; OnPropertyChanged("isUpdateEnabled"); }
        }

        private DataTable _CustomerList = new DataTable();
        public DataTable CustomerList
        {
            get { return _CustomerList; }
            set { _CustomerList = value; OnPropertyChanged("CustomerList"); }
        }

        private DataRowView _SelectedRecord;
        public DataRowView SelectedRecord
        {
            get { return _SelectedRecord; }
            set { _SelectedRecord = value; OnPropertyChanged("SelectedRecord"); }
        }
        #endregion

        public UpdateCustomerVM()
        {
            BtnUpdateDetails = new RelayCommand(updateToDB);
            CustomerListSelection = new RelayCommand(populateEditBox);
            TextInput = new RelayCommand(textChanged);
            AddCustomer = new RelayCommand(AddNewCustomer);
            usersFromDB = DBConnector.GetFromDB(string.Format(DB_StoredProcedures.CUSTOMER_GET, string.Empty, string.Empty, string.Empty, string.Empty));
            CustomerList = usersFromDB.Copy();
        }

        private void AddNewCustomer(object obj)
        {
            if (loadCustomerWindow == null)
            {
                createAddCustomerDialog();
            }
            else
            {
                if (loadCustomerWindow.IsLoaded.Equals(false))
                {
                    loadCustomerWindow = null;
                    createAddCustomerDialog();
                }
                else
                {
                    loadCustomerWindow.Focus();
                }
            }
        }

        private void createAddCustomerDialog()
        {
            loadCustomerWindow = new LoadCustomer
            {
                DataContext = new LoadCustomerVM(CustomerEvents)
            };
            loadCustomerWindow.Closing += (o, e) =>
            {
                usersFromDB = DBConnector.GetFromDB(string.Format(DB_StoredProcedures.CUSTOMER_GET, string.Empty, string.Empty, string.Empty, string.Empty));
                CustomerList = null;
                CustomerList = usersFromDB.Copy();
            };
            loadCustomerWindow.Show();
        }

        public object CustomerEvents(EventContainer_enum eventTriggered)
        {
            switch (eventTriggered)
            {
                case EventContainer_enum.cancel:
                    {
                        loadCustomerWindow.Close();
                    }
                    break;
            }
            return null;
        }

        private void populateEditBox(object obj)
        {
            if(SelectedRecord != null)
            {
                isUpdateEnabled = true;
                CustomerName = SelectedRecord["CustomerName"] as string;
                CustomerMobile = SelectedRecord["mobileNumber"] as string;
                CustomerAddress = SelectedRecord["Address"] as string;
                CustomerTelegram = SelectedRecord["whatsappNumber"] as string;
                CustomerEmail = SelectedRecord["emailID"] as string;
            }
        }

        private void updateToDB(object obj)
        {
            DBConnector.SendToDB(string.Format(DB_StoredProcedures.CUSTOMER_UPDATE, SelectedRecord["pk_customerID"],
                            CustomerName, CustomerMobile, CustomerAddress, CustomerTelegram, CustomerEmail));
            CustomerList = null;
            CustomerList = DBConnector.GetFromDB(string.Format(DB_StoredProcedures.CUSTOMER_GET, string.Empty, string.Empty, string.Empty, string.Empty));
            
        }

        internal void textChanged(object changedString)
        {
            switch (changedString as string)
            {
                case "name":
                    {
                        if (!string.IsNullOrEmpty(CustomerName) && SelectedRecord == null)
                        {
                            CustomerList = null;
                            var filtered = usersFromDB.AsEnumerable().Where(r => r.Field<string>("CustomerName").ToLower().Contains(CustomerName.ToLower()));
                            CustomerList = filtered.Count() > 0 ? filtered.CopyToDataTable() : usersFromDB.Copy();
                        }
                    }
                    break;
                case "mobile":
                    {
                        if (!string.IsNullOrEmpty(CustomerMobile) && SelectedRecord == null)
                        {
                            CustomerList = null;
                            var filtered = usersFromDB.AsEnumerable().Where(r => r.Field<string>("mobileNumber").ToLower().Contains(CustomerMobile.ToLower()));
                            CustomerList = filtered.Count() > 0 ? filtered.CopyToDataTable() : usersFromDB.Copy();
                        }
                    }
                    break;
                case "address":
                    {
                        if (!string.IsNullOrEmpty(CustomerAddress) && SelectedRecord == null)
                        {
                            CustomerList = null;
                            var filtered = usersFromDB.AsEnumerable().Where(r => r.Field<string>("Address").ToLower().Contains(CustomerAddress.ToLower()));
                            CustomerList = filtered.Count() > 0 ? filtered.CopyToDataTable() : usersFromDB.Copy();
                        }
                    }
                    break;
                case "telegram":
                    {
                        if (!string.IsNullOrEmpty(CustomerTelegram) && SelectedRecord == null)
                        {
                            CustomerList = null;
                            var filtered = usersFromDB.AsEnumerable().Where(r => r.Field<string>("whatsappNumber").ToLower().Contains(CustomerTelegram.ToLower()));
                            CustomerList = filtered.Count() > 0 ? filtered.CopyToDataTable() : usersFromDB.Copy();
                        }
                    }
                    break;
                case "email":
                    {
                        if (!string.IsNullOrEmpty(CustomerEmail) && SelectedRecord == null)
                        {
                            CustomerList = null;
                            var filtered = usersFromDB.AsEnumerable().Where(r => r.Field<string>("emailID").ToLower().Contains(CustomerEmail.ToLower()));
                            CustomerList = filtered.Count() > 0 ? filtered.CopyToDataTable() : usersFromDB.Copy();
                        }
                    }
                    break;
            }
        }
    }
}
