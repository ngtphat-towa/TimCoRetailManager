using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Helpers;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
    public class SalesViewModel:Screen
    { 
        #region Private Fields
        IProductEndpoint _productEndpoint;
        ISaleEndpoint _saleEndpoint;
        IConfigHelper _configHelper;

        private BindingList<ProductModel> _products;
        private BindingList<CartItemModel> _cart= new BindingList<CartItemModel>();

        private ProductModel _selectedProduct;

        private int _itemQuantity=1;
        
        #endregion


        #region Contructor

        public SalesViewModel(IProductEndpoint productEndpoint, 
                              ISaleEndpoint saleEndpoint,
                              IConfigHelper configHelper
        ){
            _productEndpoint = productEndpoint;
            _saleEndpoint = saleEndpoint;
            _configHelper = configHelper;
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

        public BindingList<CartItemModel> Cart
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
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }
            
        public ProductModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public string SubTotal
        {
            get
            {
                return CalculateSubTotal().ToString("C");
            }
        }
        public string Tax
        {
            get
            {
                return CalculateTax().ToString("C");
            }
        }
        public string Total
        {
            get
            {
                decimal total = CalculateSubTotal() + CalculateTax();
                return total.ToString("C");
            }
        }

        public bool CanAddToCart
        {
            get
            {
                bool output = false;

                // TODO Make sure something is selected
                // Make sure there is an item quantity and not null
                if (ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
                {
                    output = true;
                }   

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

                if (Cart.Count > 0)
                {
                    output = true;
                }
                return output;

            }
        }
        #endregion

        #region Methods
        public void AddToCart()
        {
            CartItemModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
                // update the item quantity. Can't use notify property so this is a workaround
                Cart.Remove(existingItem);
                Cart.Add(existingItem);
            }
            else
            {
                CartItemModel item = new CartItemModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                Cart.Add(item);
            }

            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }
        public void RemoveFromCart()
        {
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }
        public async Task CheckOut()
        {
            // TODO: Refactor the code to move the checkout cart logic from the front-end to the back-end.
            // This will improve the separation of concerns and make the code more maintainable.
            // TODO: Create a SaleModel and post to the API
            SaleModel sale = new SaleModel();

            foreach (var item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                });
            }

            await _saleEndpoint.PostSale(sale);
        }
        #endregion

        #region Private Methods
        private async Task LoadProducts()
        {
            var productList = await _productEndpoint.GetAll();
            Products = new BindingList<ProductModel>(productList);
        }
        private decimal CalculateSubTotal()
        {

            decimal subTotal = 0;

            foreach (var item in Cart)
            {
                subTotal += (item.Product.RetailPrice * item.QuantityInCart);
            }
            return subTotal;
        }
        private decimal CalculateTax()
        {
            decimal taxAmount = 0;
            //decimal taxRate = _configHelper.GetTaxRate() / 100;
            decimal taxRate = _configHelper.GetTaxRate() ;

            taxAmount = Cart
                .Where(x => x.Product.IsTaxable)
                .Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);

            //foreach (var item in Cart)
            //{
            //    if (item.Product.IsTaxable)
            //    {
            //        taxAmount += (item.Product.RetailPrice * item.QuantityInCart * taxRate);
            //    }
            //}
            return taxAmount;
        }
        #endregion
    }
}
