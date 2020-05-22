using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using POC.Common;
using POC.MVVM.View;

namespace POC.MVVM.ViewModel
{
    class MainWindowVM : ViewModelBase
    {
        #region Bind Property
        private object _frameContent;
        public object frameContent
        {
            get { return _frameContent; }
            set { _frameContent = value; OnPropertyChanged("frameContent"); }
        }
        #endregion

        public MainWindowVM()
        {
            Page startPage = new LoginPage();
            startPage.DataContext = new LoginVM(invokeNewPage);
            frameContent = startPage;
        }

        public object invokeNewPage(PocPages_t pages)
        {
            switch (pages)
            {
                case PocPages_t.MainMenuPage:
                    {
                        Page page = new MainMenuPage();
                        page.DataContext = new MainMenuPageVM(invokeNewPage);
                        frameContent = null;
                        frameContent = page;
                    }
                    break;
                case PocPages_t.LoginPage:
                    {
                        Page startPage = new LoginPage();
                        startPage.DataContext = new LoginVM(invokeNewPage);
                        frameContent = null;
                        frameContent = startPage;
                    }
                    break;
            }
            return null;
        }
    }
}
