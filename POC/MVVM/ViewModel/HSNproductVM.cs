using POC.Common;
using POC.MVVM.Model;
using POC.MVVM.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POC.MVVM.ViewModel
{
    class HSNproductVM : ViewModelBase
    {
        private DataTable usersFromDB;
        //LoadCustomer loadCustomerWindow;

        #region Bind Property
        private string _HSNCode;
        public string HSNCode
        {
            get { return _HSNCode; }
            set { _HSNCode = value; OnPropertyChanged("HSNCode"); }
        }

        private string _GoodsDesc;
        public string GoodsDesc
        {
            get { return _GoodsDesc; }
            set { _GoodsDesc = value; OnPropertyChanged("GoodsDesc"); }
        }

        private string _CGST;
        public string CGST
        {
            get { return _CGST; }
            set { _CGST = value; OnPropertyChanged("CGST"); }
        }

        private string _SGST;
        public string SGST
        {
            get { return _SGST; }
            set { _SGST = value; OnPropertyChanged("SGST"); }
        }

        private string _IGST;
        public string IGST
        {
            get { return _IGST; }
            set { _IGST = value; OnPropertyChanged("IGST"); }
        }
        
        private string _CompCess;
        public string CompCess
        {
            get { return _CompCess; }
            set { _CompCess = value; OnPropertyChanged("CompCess"); }
        }
        
        private string _IsValidHSN = "0";
        public string IsValidHSN
        {
            get { return _IsValidHSN; }
            set { _IsValidHSN = value; OnPropertyChanged("IsValidHSN"); }
        }

        private ICommand _BtnUpdateDetails;
        public ICommand BtnUpdateDetails
        {
            get { return _BtnUpdateDetails; }
            set { _BtnUpdateDetails = value; OnPropertyChanged("BtnUpdateDetails"); }
        }

        private ICommand _HSNListSelection;
        public ICommand HSNListSelection
        {
            get { return _HSNListSelection; }
            set { _HSNListSelection = value; OnPropertyChanged("HSNListSelection"); }
        }

        private ICommand _TextInput;
        public ICommand TextInput
        {
            get { return _TextInput; }
            set { _TextInput = value; OnPropertyChanged("TextInput"); }
        }

        private ICommand _AddHSN;
        public ICommand AddHSN
        {
            get { return _AddHSN; }
            set { _AddHSN = value; OnPropertyChanged("AddHSN"); }
        }

        private bool _isUpdateEnabled = false;
        public bool isUpdateEnabled
        {
            get { return _isUpdateEnabled; }
            set { _isUpdateEnabled = value; OnPropertyChanged("isUpdateEnabled"); }
        }

        private DataTable _HSNList = new DataTable();
        public DataTable HSNList
        {
            get { return _HSNList; }
            set { _HSNList = value; OnPropertyChanged("HSNList"); }
        }

        private DataRowView _SelectedRecord;
        public DataRowView SelectedRecord
        {
            get { return _SelectedRecord; }
            set { _SelectedRecord = value; OnPropertyChanged("SelectedRecord"); }
        }
        #endregion

        public HSNproductVM()
        {
            BtnUpdateDetails = new RelayCommand(updateToDB);
            HSNListSelection = new RelayCommand(populateEditBox);
            TextInput = new RelayCommand(textChanged);
            AddHSN = new RelayCommand(AddNewHSN);
            usersFromDB = DBConnector.GetFromDB(string.Format(DB_StoredProcedures.HSN_GET));
            HSNList = usersFromDB.Copy();
        }

        private void AddNewHSN(object obj)
        {
            //if (loadCustomerWindow == null)
            //{
            //    createAddCustomerDialog();
            //}
            //else
            //{
            //    if (loadCustomerWindow.IsLoaded.Equals(false))
            //    {
            //        loadCustomerWindow = null;
            //        createAddCustomerDialog();
            //    }
            //    else
            //    {
            //        loadCustomerWindow.Focus();
            //    }
            //}
        }

        private void createAddCustomerDialog()
        {
            //loadCustomerWindow = new LoadCustomer
            //{
            //    DataContext = new LoadCustomerVM(CustomerEvents)
            //};
            //loadCustomerWindow.Closing += (o, e) =>
            //{
            //    usersFromDB = DBConnector.GetFromDB(string.Format(DB_StoredProcedures.CUSTOMER_GET, string.Empty, string.Empty, string.Empty, string.Empty));
            //    CustomerList = null;
            //    CustomerList = usersFromDB.Copy();
            //};
            //loadCustomerWindow.Show();
        }

        public object CustomerEvents(CustomerEventList_t eventTriggered)
        {
            //switch (eventTriggered)
            //{
            //    case CustomerEventList_t.cancelPeople:
            //        {
            //            loadCustomerWindow.Close();
            //        }
            //        break;
            //}
            return null;
        }

        private void populateEditBox(object obj)
        {
            if (SelectedRecord != null)
            {
                isUpdateEnabled = true;
                HSNCode = SelectedRecord["HSNCode"] as string;
                GoodsDesc = SelectedRecord["goods_Description"] as string;
                CGST = ((float)SelectedRecord["CGST"]).ToString();
                SGST = ((float)SelectedRecord["SGST"]).ToString();
                IGST = ((float)SelectedRecord["IGST"]).ToString();
                CompCess = ((float)SelectedRecord["CompensationCess"]).ToString();
                IsValidHSN = ((int)SelectedRecord["isValid"]).ToString();
            }
        }

        private void updateToDB(object obj)
        {
            DBConnector.SendToDB(string.Format(DB_StoredProcedures.HSN_UPDATE, SelectedRecord["pk_HSN"],
                            HSNCode, GoodsDesc, CGST, SGST, IGST, CompCess, IsValidHSN));
            HSNList = null;
            HSNList = DBConnector.GetFromDB(string.Format(DB_StoredProcedures.HSN_GET));

        }

        internal void textChanged(object changedString)
        {
            switch (changedString as string)
            {
                case "HSNCode":
                    {
                        if (!string.IsNullOrEmpty(HSNCode) && SelectedRecord == null)
                        {
                            HSNList = null;
                            var filtered = usersFromDB.AsEnumerable().Where(r => r.Field<string>("HSNCode").ToLower().Contains(HSNCode.ToLower()));
                            HSNList = filtered.Count() > 0 ? filtered.CopyToDataTable() : usersFromDB.Copy();
                        }
                    }
                    break;
                case "GoodsDesc":
                    {
                        if (!string.IsNullOrEmpty(GoodsDesc) && SelectedRecord == null)
                        {
                            HSNList = null;
                            var filtered = usersFromDB.AsEnumerable().Where(r => r.Field<string>("goods_Description").ToLower().Contains(GoodsDesc.ToLower()));
                            HSNList = filtered.Count() > 0 ? filtered.CopyToDataTable() : usersFromDB.Copy();
                        }
                    }
                    break;
                case "CGST":
                    {
                        if (!string.IsNullOrEmpty(CGST) && SelectedRecord == null)
                        {
                            HSNList = null;
                            var filtered = usersFromDB.AsEnumerable().Where(r => r.Field<string>("CGST").ToLower().Contains(CGST.ToLower()));
                            HSNList = filtered.Count() > 0 ? filtered.CopyToDataTable() : usersFromDB.Copy();
                        }
                    }
                    break;
                case "SGST":
                    {
                        if (!string.IsNullOrEmpty(SGST) && SelectedRecord == null)
                        {
                            HSNList = null;
                            var filtered = usersFromDB.AsEnumerable().Where(r => r.Field<string>("SGST").ToLower().Contains(SGST.ToLower()));
                            HSNList = filtered.Count() > 0 ? filtered.CopyToDataTable() : usersFromDB.Copy();
                        }
                    }
                    break;
                case "IGST":
                    {
                        if (!string.IsNullOrEmpty(IGST) && SelectedRecord == null)
                        {
                            HSNList = null;
                            var filtered = usersFromDB.AsEnumerable().Where(r => r.Field<string>("IGST").ToLower().Contains(IGST.ToLower()));
                            HSNList = filtered.Count() > 0 ? filtered.CopyToDataTable() : usersFromDB.Copy();
                        }
                    }
                    break;
                case "CompCess":
                    {
                        if (!string.IsNullOrEmpty(CompCess) && SelectedRecord == null)
                        {
                            HSNList = null;
                            var filtered = usersFromDB.AsEnumerable().Where(r => r.Field<string>("CompensationCess").ToLower().Contains(CompCess.ToLower()));
                            HSNList = filtered.Count() > 0 ? filtered.CopyToDataTable() : usersFromDB.Copy();
                        }
                    }
                    break;
                case "IsValidHSN":
                    {
                        if (!string.IsNullOrEmpty(IsValidHSN) && SelectedRecord == null)
                        {
                            HSNList = null;
                            var filtered = usersFromDB.AsEnumerable().Where(r => r.Field<string>("isValid").ToLower().Contains(IsValidHSN.ToLower()));
                            HSNList = filtered.Count() > 0 ? filtered.CopyToDataTable() : usersFromDB.Copy();
                        }
                    }
                    break;
            }
        }
    }
}
