using System.Collections.ObjectModel;
using GasyTek.Lakana.Common.UI;
using GasyTek.Lakana.Navigation.Services;
using Samples.GasyTek.Lakana.Navigation.Common;
using Samples.GasyTek.Lakana.Navigation.Data;

namespace Samples.GasyTek.Lakana.Navigation.Features
{
    public class ProductListViewModel : IViewKeyAware, IPresentable
    {
        private readonly IUIMetadata _uiMetadata;

        public ObservableCollection<Product> Products { get; private set; }
        public ISimpleCommand<object> EditProductCommand { get; private set; }

        public string Description
        {
            get { return GetDescription(); }
        }

        public ProductListViewModel()
        {
            _uiMetadata = new UIMetadata {LabelProvider = () => "Product List"};

            Products = new ObservableCollection<Product>
                           {
                               new Product {Name = "Mac Book Pro", Quantity = 2},
                               new Product {Name = "Domain Driven Design Book", Quantity = 3},
                               new Product {Name = "RAM Memory", Quantity = 23},
                               new Product {Name = "C# - Bible Book", Quantity = 15},
                               new Product {Name = "Amazon Kindle", Quantity = 12}
                           };

            EditProductCommand = new SimpleCommand<object>(OnEditProductCommand);
        }

        private void OnEditProductCommand(object param)
        {
            var p = param as Product;
            if (p != null)
            {
                // Opens the ProductEditView on top of ProductEditViewModel
                // Note that this viewmodel implements IViewKeyAware so that it will have the same ViewKey as its View
                var navigationInfo = NavigationInfo.CreateComplex(ViewId.ProductEdit, ViewKey, new ProductEditViewModel(p));
                Singletons.NavigationService.NavigateTo<ProductEditView>(navigationInfo);
            }
        }

        private string GetDescription()
        {
            return "Clicking on 'Edit' button will begin a local linear navigation by stacking the detailed view on this master view.\r\n" +
                   "That means that when you close the detail view, this one will always be the next that will be displayed.\r\n\r\n" +
                   "Features demonstrated : " +
                   "\r\n > Stacking views (local linear navigation)";
        }

        #region IViewKeyAware members

        public string ViewKey { get; set; }

        #endregion

        #region IPresentable members

        public IUIMetadata UIMetadata
        {
            get { return _uiMetadata; }
        }

        #endregion
    }
}
