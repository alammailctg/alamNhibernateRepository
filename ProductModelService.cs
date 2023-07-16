using alamapp.Model.Products;
using alamapp.Model.RepositoryInterface.Products;
using alamapp.ServiceImplementations.Interface;
using alamapp.ServiceImplementations.Messaging.ProductModels;
using alamapp.ServiceImplementations.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alamapp.ServiceImplementations.Implementation
{
   public class ProductModelService:IProductModelService
    {
        private IProductModelRepository _productModelRepository;
        public ProductModelService(IProductModelRepository productModelRepository)
        {
            _productModelRepository = productModelRepository;
        }
        public GetAllProductModelResponse GetAllProductModel()
        {
            GetAllProductModelResponse response = new GetAllProductModelResponse();
            IEnumerable<ProductModel> productModel = _productModelRepository.FindAll();
            response.ProductModels = productModel.ConvertToProductModelViews();
            return response;
        }
    }
}
