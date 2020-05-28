using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using POC.Common;
using POC.MVVM.View;

namespace POC.MVVM.ViewModel
{
    class MainMenuPageVM : ViewModelBase
    {
        Func<PocPages_t, object> pageCaller;
        UpdateCustomer updateCustomerWindow;

        #region Bind Property
        private ICommand _Logout;
        public ICommand Logout
        {
            get { return _Logout; }
            set { _Logout = value; OnPropertyChanged("Logout"); }
        }

        private ICommand _BtnProdCatalogue;
        public ICommand BtnProdCatalogue
        {
            get { return _BtnProdCatalogue; }
            set { _BtnProdCatalogue = value; OnPropertyChanged("BtnProdCatalogue"); }
        }
        private ICommand _BtnProdCategory;
        public ICommand BtnProdCategory
        {
            get { return _BtnProdCategory; }
            set { _BtnProdCategory = value; OnPropertyChanged("BtnProdCategory"); }
        }
        private ICommand _BtnProdHSN;
        public ICommand BtnProdHSN
        {
            get { return _BtnProdHSN; }
            set { _BtnProdHSN = value; OnPropertyChanged("BtnProdHSN"); }
        }
        private ICommand _BtnSales;
        public ICommand BtnSales
        {
            get { return _BtnSales; }
            set { _BtnSales = value; OnPropertyChanged("BtnSales"); }
        }
        private ICommand _BtnAddCustomer;
        public ICommand BtnAddCustomer
        {
            get { return _BtnAddCustomer; }
            set { _BtnAddCustomer = value; OnPropertyChanged("BtnAddCustomer"); }
        }
        private ICommand _BtnUpdateCustomer;
        public ICommand BtnUpdateCustomer
        {
            get { return _BtnUpdateCustomer; }
            set { _BtnUpdateCustomer = value; OnPropertyChanged("BtnUpdateCustomer"); }
        }


        #endregion

        public MainMenuPageVM(Func<PocPages_t, object> pages)
        {
            pageCaller = pages;
            Logout = new RelayCommand((obj) => _ = pageCaller(PocPages_t.LoginPage));
            BtnProdCatalogue = new RelayCommand(execute: ProductsActivity);
            BtnProdCategory = new RelayCommand(execute: ProductsActivity);
            BtnProdHSN = new RelayCommand(execute: ProductsActivity);
            BtnSales = new RelayCommand(execute: SalesActivity);
            BtnAddCustomer = new RelayCommand(execute: CustomerActivity);
            BtnUpdateCustomer = new RelayCommand(execute: CustomerActivity);
        }

        private void CustomerActivity(object obj)
        {
            switch (obj as string)
            {
                case "UpdateCustomer":
                    {
                        if (updateCustomerWindow == null)
                        {
                            updateCustomerWindow = new UpdateCustomer
                            {
                                DataContext = new UpdateCustomerVM()
                            };
                            updateCustomerWindow.Show();
                        }
                        else
                        {
                            if (updateCustomerWindow.IsLoaded.Equals(false))
                            {
                                updateCustomerWindow = null;
                                updateCustomerWindow = new UpdateCustomer
                                {
                                    DataContext = new UpdateCustomerVM()
                                };
                                updateCustomerWindow.Show();
                            }
                            else
                            {
                                updateCustomerWindow.Focus();
                            }
                        }
                    }
                    break;
            }
        }

        


        private void SalesActivity(object obj) => MessageBox.Show("This is not supported in this version.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

        private void ProductsActivity(object obj)
        {
            switch (obj as string)
            {
                case "ProdCatalogue":
                    {
                        ProductCatalogue catalogue = new ProductCatalogue();
                        catalogue.Show();
                    }
                    break;
                case "ProdHSN":
                    {
                        ProductHSN hsn = new ProductHSN()
                        {
                            DataContext = new HSNproductVM()
                        };
                        hsn.Show();
                    }
                    break;
                default:
                    MessageBox.Show("This is not supported in this version.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
            }
        }
    }
}
