using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using POC.Common;

namespace POC.MVVM.ViewModel
{
    class LoginVM : ViewModelBase
    {
        Func<PocPages_t, object> pageCaller;

        #region Bind Property
        private string _UserName = string.Empty;
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; OnPropertyChanged("UserName"); }
        }

        private string _Password = string.Empty;
        public string Password
        {
            get { return _Password; }
            set { _Password = value; OnPropertyChanged("Password"); }
        }

        private ICommand _LoginToApp;
        public ICommand LoginToApp
        {
            get { return _LoginToApp; }
            set { _LoginToApp = value; OnPropertyChanged("LoginToApp"); }
        }

        private bool _UserShouldEditValueNow = true;
        public bool UserShouldEditValueNow
        {
            get { return _UserShouldEditValueNow; }
            set { _UserShouldEditValueNow = value; OnPropertyChanged("UserShouldEditValueNow"); }
        }

        #endregion

        public LoginVM(Func<PocPages_t, object> pages)
        {
            pageCaller = pages;
            LoginToApp = new RelayCommand(ValidateAndLoad);
        }

        private void ValidateAndLoad(object obj)
        {
            if (obj is string && (obj as string).Equals("LoginToApp"))
            {
                if (UserName.ToLower().Equals("admin") && Password.Equals("admin"))
                {
                    pageCaller(PocPages_t.MainMenuPage);
                }
                else
                {
                    MessageBox.Show("Enter valid username or password", "Invalid Credentials", MessageBoxButton.OK, MessageBoxImage.Error);
                    Password = string.Empty;
                    UserName = string.Empty;
                    UserShouldEditValueNow = true;
                }
            }
        }
    }
}
