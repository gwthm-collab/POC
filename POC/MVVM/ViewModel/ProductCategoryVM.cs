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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace POC.MVVM.ViewModel
{
    class ProductCategoryVM: ViewModelBase
    {
        private DataTable usersFromDB;
        AddProductCategory addHSNWindow;
        DataTable hsnPktable;

        #region Bind Property
        private KeyValuePair<int, string> _HSNCode = new KeyValuePair<int, string>();
        public KeyValuePair<int, string> HSNCode
        {
            get { return _HSNCode; }
            set { _HSNCode = value; OnPropertyChanged("HSNCode"); }
        }

        private Dictionary<int, string> _HSNKeyValue = new Dictionary<int, string>();
        public Dictionary<int, string> HSNKeyValue
        {
            get
            {
                return _HSNKeyValue;
            }
            set
            {
                _HSNKeyValue = value;
                OnPropertyChanged("HSNKeyValue");
            }
        }

        private string _ProductCategoryName;
        public string ProductCategoryName
        {
            get { return _ProductCategoryName; }
            set { _ProductCategoryName = value; OnPropertyChanged("ProductCategoryName"); }
        }

        private string _IsValidCategory = "0";
        public string IsValidCategory
        {
            get { return _IsValidCategory; }
            set { _IsValidCategory = value; OnPropertyChanged("IsValidCategory"); }
        }

        private ICommand _BtnUpdateDetails;
        public ICommand BtnUpdateDetails
        {
            get { return _BtnUpdateDetails; }
            set { _BtnUpdateDetails = value; OnPropertyChanged("BtnUpdateDetails"); }
        }

        private ICommand _ProdCategorySelection;
        public ICommand ProdCategorySelection
        {
            get { return _ProdCategorySelection; }
            set { _ProdCategorySelection = value; OnPropertyChanged("ProdCategorySelection"); }
        }

        private ICommand _TextInput;
        public ICommand TextInput
        {
            get { return _TextInput; }
            set { _TextInput = value; OnPropertyChanged("TextInput"); }
        }

        private ICommand _AddCategory;
        public ICommand AddCategory
        {
            get { return _AddCategory; }
            set { _AddCategory = value; OnPropertyChanged("AddCategory"); }
        }

        private bool _isUpdateEnabled = false;
        public bool isUpdateEnabled
        {
            get { return _isUpdateEnabled; }
            set { _isUpdateEnabled = value; OnPropertyChanged("isUpdateEnabled"); }
        }

        private DataTable _CategoryList = new DataTable();
        public DataTable CategoryList
        {
            get { return _CategoryList; }
            set { _CategoryList = value; OnPropertyChanged("CategoryList"); }
        }

        private DataRowView _SelectedRecord;
        public DataRowView SelectedRecord
        {
            get { return _SelectedRecord; }
            set { _SelectedRecord = value; OnPropertyChanged("SelectedRecord"); }
        }
        #endregion

        public ProductCategoryVM()
        {
            BtnUpdateDetails = new RelayCommand(updateToDB);
            ProdCategorySelection = new RelayCommand(populateEditBox);
            TextInput = new RelayCommand(textChanged);
            AddCategory = new RelayCommand(AddNewHSN);
            hsnPktable = DBConnector.GetFromDB(string.Format(DB_StoredProcedures.HSN_GET));
            HSNKeyValue = hsnPktable.AsEnumerable().ToDictionary(row => row.Field<int>("pk_HSN"), row => row.Field<string>("HSNCode"));
            usersFromDB = DBConnector.GetFromDB(string.Format(DB_StoredProcedures.PRODCATEGORY_GET));
            CategoryList = usersFromDB.Copy();
        }

        private void AddNewHSN(object obj)
        {
            if (addHSNWindow == null)
            {
                createAddHSNDialog();
            }
            else
            {
                if (addHSNWindow.IsLoaded.Equals(false))
                {
                    addHSNWindow = null;
                    createAddHSNDialog();
                }
                else
                {
                    addHSNWindow.Focus();
                }
            }
        }

        private void createAddHSNDialog()
        {
            addHSNWindow = new AddProductCategory
            {
                DataContext = new AddProductCategoryVM(Events, HSNKeyValue)
            };
            addHSNWindow.Closing += (o, e) =>
            {
                usersFromDB = DBConnector.GetFromDB(string.Format(DB_StoredProcedures.PRODCATEGORY_GET));
                CategoryList = null;
                CategoryList = usersFromDB.Copy();
            };
            addHSNWindow.Show();
        }

        public object Events(EventContainer_enum eventTriggered)
        {
            switch (eventTriggered)
            {
                case EventContainer_enum.cancel:
                    {
                        addHSNWindow.Close();
                    }
                    break;
            }
            return null;
        }

        private void populateEditBox(object obj)
        {
            if (SelectedRecord != null)
            {
                isUpdateEnabled = true;
                ProductCategoryName = SelectedRecord["ProductCategory"] as string;
                HSNCode = HSNKeyValue.Where(x => x.Value.Equals(SelectedRecord["HSNCode"] as string)).FirstOrDefault();
                IsValidCategory = ((int)SelectedRecord["isValid"]).ToString();
            }
        }

        private void updateToDB(object obj)
        {
            DBConnector.SendToDB(string.Format(DB_StoredProcedures.PRODCATEGORY_UPDATE, SelectedRecord["pk_productCategory"], ProductCategoryName,
                            HSNCode.Key, IsValidCategory));
            CategoryList = null;
            usersFromDB = DBConnector.GetFromDB(string.Format(DB_StoredProcedures.PRODCATEGORY_GET));
            CategoryList = usersFromDB.Copy();
        }

        internal void textChanged(object changedString)
        {
            switch (changedString as string)
            {
                case "HSNCode":
                    {
                        if (!string.IsNullOrEmpty(HSNCode.Value as string) && SelectedRecord == null)
                        {
                            CategoryList = null;
                            var filtered = usersFromDB.AsEnumerable().Where(r => r.Field<string>("HSNCode").ToLower().Contains((HSNCode.Value as string).ToLower()));
                            CategoryList = filtered.Count() > 0 ? filtered.CopyToDataTable() : usersFromDB.Copy();
                        }
                    }
                    break;
                case "IsValidCategory":
                    {
                        if (!string.IsNullOrEmpty(IsValidCategory) && SelectedRecord == null)
                        {
                            CategoryList = null;
                            var filtered = usersFromDB.AsEnumerable().Where(r => r.Field<string>("isValid").ToLower().Contains(IsValidCategory.ToLower()));
                            CategoryList = filtered.Count() > 0 ? filtered.CopyToDataTable() : usersFromDB.Copy();
                        }
                    }
                    break;
                case "ProductCategoryName":
                    {
                        if (!string.IsNullOrEmpty(ProductCategoryName) && SelectedRecord == null)
                        {
                            CategoryList = null;
                            var filtered = usersFromDB.AsEnumerable().Where(r => r.Field<string>("ProductCategory").ToLower().Contains(ProductCategoryName.ToLower()));
                            CategoryList = filtered.Count() > 0 ? filtered.CopyToDataTable() : usersFromDB.Copy();
                        }
                    }
                    break;
            }
        }
    }
}
