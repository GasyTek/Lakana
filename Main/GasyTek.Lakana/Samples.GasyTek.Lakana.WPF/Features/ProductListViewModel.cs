using System.Collections.ObjectModel;
using GasyTek.Lakana.WPF.Common;
using GasyTek.Lakana.WPF.Services;
using Samples.GasyTek.Lakana.WPF.Common;
using Samples.GasyTek.Lakana.WPF.Data;

namespace Samples.GasyTek.Lakana.WPF.Features
{
    public class ProductListViewModel : IViewKeyAware, IPresentable
    {
        private readonly IPresentationMetadata _presentationMetadata;

        public ObservableCollection<Product> Products { get; private set; }
        public ISimpleCommand<object> EditProductCommand { get; private set; }

        public ProductListViewModel()
        {
            _presentationMetadata = new PresentationMetadata {LabelProvider = () => "Product List"};

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

        #region IViewKeyAware members

        public string ViewKey { get; set; }

        #endregion

        #region IPresentable members

        public IPresentationMetadata PresentationMetadata
        {
            get { return _presentationMetadata; }
        }

        #endregion
    }
}
