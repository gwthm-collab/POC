using System;
using System.Windows;
using System.Windows.Input;
using POC.Common;
using POC.MVVM.View;

namespace POC.MVVM.ViewModel
{
    class MainMenuPageVM : ViewModelBase
    {
        Func<PocPages_t, object> pageCaller;
        LoadCustomer loadCustomerWindow;
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
        private ICommand _BtnProdStock;
        public ICommand BtnProdStock
        {
            get { return _BtnProdStock; }
            set { _BtnProdStock = value; OnPropertyChanged("BtnProdStock"); }
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
            BtnProdStock = new RelayCommand(execute: ProductsActivity);
            BtnSales = new RelayCommand(execute: SalesActivity);
            BtnAddCustomer = new RelayCommand(execute: CustomerActivity);
            BtnUpdateCustomer = new RelayCommand(execute: CustomerActivity);
        }

        private void CustomerActivity(object obj)
        {
            switch (obj as string)
            {
                case "AddCustomer":
                    {
                        if (loadCustomerWindow == null)
                        {
                            loadCustomerWindow = new LoadCustomer
                            {
                                DataContext = new LoadCustomerVM(CustomerEvents)
                            };
                            loadCustomerWindow.Show();
                        }
                        else
                        {
                            if (loadCustomerWindow.IsLoaded.Equals(false))
                            {
                                loadCustomerWindow = null;
                                loadCustomerWindow = new LoadCustomer
                                {
                                    DataContext = new LoadCustomerVM(CustomerEvents)
                                };
                                loadCustomerWindow.Show();
                            }
                            else
                            {
                                loadCustomerWindow.Focus();
                            }
                        }
                    }
                    break;
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

        public object CustomerEvents(CustomerEventList_t eventTriggered)
        {
            switch (eventTriggered)
            {
                case CustomerEventList_t.cancelPeople:
                    {
                        loadCustomerWindow.Close();
                    }
                    break;
            }
            return null;
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
                default:
                    MessageBox.Show("This is not supported in this version.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
            }
        }
    }
}
