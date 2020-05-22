using POC.Common;
using POC.MVVM.Model;
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
            CustomerList = DBConnector.GetFromDB(string.Format(DB_StoredProcedures.CUSTOMER_GET, string.Empty, string.Empty, string.Empty, string.Empty));
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
    }
}
