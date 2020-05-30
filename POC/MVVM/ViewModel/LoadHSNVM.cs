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
    class LoadHSNVM : ViewModelBase
    {
        Func<EventContainer_enum, object> triggerEvent;

        #region Bind Property
        private ICommand _AddHSN;
        public ICommand AddHSN
        {
            get { return _AddHSN; }
            set { _AddHSN = value; OnPropertyChanged("AddHSN"); }
        }

        private ICommand _CancelHSN;
        public ICommand CancelHSN
        {
            get { return _CancelHSN; }
            set { _CancelHSN = value; OnPropertyChanged("CancelHSN"); }
        }

        private ICommand _textBoxChanged;
        public ICommand textBoxChanged
        {
            get { return _textBoxChanged; }
            set { _textBoxChanged = value; OnPropertyChanged("textBoxChanged"); }
        }

        private string _HSNCode;
        public string HSNCode
        {
            get { return _HSNCode; }
            set { _HSNCode = value; OnPropertyChanged("HSNCode"); }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { _Description = value; OnPropertyChanged("Description"); }
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

        private string _IsValidHSN = "1";
        public string IsValidHSN
        {
            get { return _IsValidHSN; }
            set { _IsValidHSN = value; OnPropertyChanged("IsValidHSN"); }
        }

        private bool _isAddEnabled = false;
        public bool isAddEnabled
        {
            get { return _isAddEnabled; }
            set { _isAddEnabled = value; OnPropertyChanged("isAddEnabled"); }
        }

        #endregion

        public LoadHSNVM(Func<EventContainer_enum, object> events)
        {
            triggerEvent += events;
            AddHSN = new RelayCommand(AddFromForm);
            CancelHSN = new RelayCommand((obj) => _ = triggerEvent(EventContainer_enum.cancel));
            textBoxChanged = new RelayCommand((obj) => isAddEnabled = true);
        }

        private void AddFromForm(object obj)
        {
            //Event to DB
            bool status = DBConnector.SendToDB(string.Format(DB_StoredProcedures.HSN_INSERT, HSNCode, Description, CGST, SGST, IGST,
                CompCess, IsValidHSN));

            if (status)
            {
                MessageBox.Show($"HSN: {HSNCode}\nAdded Successfully.", "Add HSN", MessageBoxButton.OK, MessageBoxImage.Information);
                HSNCode = string.Empty;
                Description = string.Empty;
                CGST = string.Empty;
                SGST = string.Empty;
                IGST = string.Empty;
                CompCess = string.Empty;
                IsValidHSN = "1";
            }
            else
            {
                MessageBox.Show($"HSN: {HSNCode}\nError in adding HSN.", "Add HSN", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
