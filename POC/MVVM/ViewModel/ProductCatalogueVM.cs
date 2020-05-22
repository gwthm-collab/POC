using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using POC.Common;
using POC.MVVM.View;

namespace POC.MVVM.ViewModel
{
    class ProductCatalogueVM : ViewModelBase
    {
        #region Bind Property
        private ICommand _btnAddProducts;
        public ICommand BtnAddProducts
        {
            get { return _btnAddProducts; }
            set { _btnAddProducts = value; OnPropertyChanged("BtnAddProducts"); }
        } 


        #endregion

        public ProductCatalogueVM()
        {
            BtnAddProducts = new RelayCommand(AddProducts);
        }

        private void AddProducts(object obj)
        {
            AddProducts addWindow = new AddProducts();
            addWindow.Show();
        }
    }
}
