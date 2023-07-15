using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
    public class SalesViewModel:Screen
    {
        #region Private Fields
        IProductEndpoint _productEndpoint;

        private BindingList<ProductModel> _products;
        private BindingList<string> _cart;

        private int _itemQuantity;
        
        #endregion


        #region Contructor

        public SalesViewModel(IProductEndpoint productEndpoint)
        {
            _productEndpoint = productEndpoint;
        }
        protected async override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        #endregion

        #region Properties
        public BindingList<ProductModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        public BindingList<string> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
            }
        }

        public bool CanAddToCart
        {
            get
            {
                bool output = false;

                // TODO Make sure something is selected
                // TODO Make sure there is an item quantity
                return output;
            }
        }

        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;

                // Make sure something is selected
                return output;

            }
        }
        public bool CanCheckOut
        {
            get
            {
                bool output = false;

                // Make sure there is something in the cart
                return output;

            }
        }
        #endregion

        #region Methods
        public void AddToCart()
        {

        }
        public void RemoveFromCart()
        {

        }
        public void CheckOut()
        {

        }
        #endregion

        #region Private Methods
         private async Task LoadProducts()
        {
            var productList = await _productEndpoint.GetAll();
            Products = new BindingList<ProductModel>(productList);
        }
        #endregion
    }
}
