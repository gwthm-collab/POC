using Org.BouncyCastle.Asn1.X509;
using POC.Common;
using POC.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace POC.MVVM.ViewModel
{
    class AddProductCategoryVM : ViewModelBase
    {
        Func<EventContainer_enum, object> triggerEvent;

        #region Bind Property
        private ICommand _AddCategory;
        public ICommand AddCategory
        {
            get { return _AddCategory; }
            set { _AddCategory = value; OnPropertyChanged("AddCategory"); }
        }

        private ICommand _CancelCategory;
        public ICommand CancelCategory
        {
            get { return _CancelCategory; }
            set { _CancelCategory = value; OnPropertyChanged("CancelCategory"); }
        }

        private ICommand _textBoxChanged;
        public ICommand textBoxChanged
        {
            get { return _textBoxChanged; }
            set { _textBoxChanged = value; OnPropertyChanged("textBoxChanged"); }
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

        private KeyValuePair<int, string> _HSNCode = new KeyValuePair<int, string>();
        public KeyValuePair<int, string> HSNCode
        {
            get { return _HSNCode; }
            set { _HSNCode = value; OnPropertyChanged("HSNCode"); }
        }

        private string _ProductCategoryName;
        public string ProductCategoryName
        {
            get { return _ProductCategoryName; }
            set { _ProductCategoryName = value; OnPropertyChanged("ProductCategoryName"); }
        }

        private string _IsValidCategory = "1";
        public string IsValidCategory
        {
            get { return _IsValidCategory; }
            set { _IsValidCategory = value; OnPropertyChanged("IsValidCategory"); }
        }

        private bool _isAddEnabled = false;
        public bool isAddEnabled
        {
            get { return _isAddEnabled; }
            set { _isAddEnabled = value; OnPropertyChanged("isAddEnabled"); }
        }

        #endregion

        public AddProductCategoryVM(Func<EventContainer_enum, object> events, Dictionary<int, string> keyValues)
        {
            triggerEvent += events;
            HSNKeyValue = new Dictionary<int, string>(keyValues);
            AddCategory = new RelayCommand(AddFromForm);
            CancelCategory = new RelayCommand((obj) => _ = triggerEvent(EventContainer_enum.cancel));
            textBoxChanged = new RelayCommand((obj) => isAddEnabled = true);
        }

        private void AddFromForm(object obj)
        {
            //Event to DB
            bool status = DBConnector.SendToDB(string.Format(DB_StoredProcedures.PRODCATEGORY_INSERT, ProductCategoryName, HSNCode.Value, IsValidCategory));

            if (status)
            {
                MessageBox.Show($"Product Category: {ProductCategoryName}\nAdded Successfully.", "Add Product Category", MessageBoxButton.OK, MessageBoxImage.Information);
                HSNCode = new KeyValuePair<int, string>();
                ProductCategoryName = string.Empty;
                IsValidCategory = "1";
            }
            else
            {
                MessageBox.Show($"Product Category: {ProductCategoryName}\nError in adding Product Category.", "Add Product Category", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
