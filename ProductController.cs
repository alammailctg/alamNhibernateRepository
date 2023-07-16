using alamapp.Infrastructure.CookieStorage;
using alamapp.ServiceImplementations.Interface;
using alamapp.ServiceImplementations.Messaging.Product;
using alamapp.ServiceImplementations.ViewModel.Product;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebAPIApplication.Models;
using WebAPIApplication.Models.JsonDto;


namespace WebAPIApplication.Controllers
{
    
    public class ProductController : BaseApiController
    {
        private IProductService _productService;
        private ICookieStorageService _cookieStorage;
        public ProductController(IProductService productService, ICookieStorageService cookieStorageService, ICookieStorageService cookieStorage)
            : base(cookieStorageService)
        {
            _productService = productService;
            _cookieStorage = cookieStorage;

        }
        //
        // GET: /Product/
        [HttpGet]
        public HttpResponseMessage GetBasketSummaries()
        {
            BasketSummaryHome basketSummaryhome = new BasketSummaryHome();

            basketSummaryhome.BasketSummary = base.GetBasketSummary();

            HttpResponseMessage httpResponse = Request.CreateResponse(HttpStatusCode.OK, basketSummaryhome);
            return httpResponse;
        }

        [HttpGet]
        public HttpResponseMessage GetAllProductByTitleId(int id)
        {
            IEnumerable<ProductView> response = _productService.GetProductByTitle(id);

            HttpResponseMessage httpResponse = Request.CreateResponse(HttpStatusCode.OK,response.Distinct());
            return httpResponse;
        }


        //[HttpPost]
        //public HttpResponseMessage GetProductByCategory(GetProductsByCategoryRequest request)
        //{
        //    GetProductsByCategoryResponse response = _productService.GetProductByCategory(request);
        //    HttpResponseMessage httpResponse = Request.CreateResponse(HttpStatusCode.OK, response.Products.Distinct());
        //    return httpResponse;
        //}
        //[HttpGet]
        //public HttpResponseMessage GetProductByBrandWithStock()
        //{
        //    IEnumerable<ProductView> response = _productService.GetProductCountStockByBrand();
        //    HttpResponseMessage httpResponse = Request.CreateResponse(HttpStatusCode.OK, response);
        //    return httpResponse;
        //}

        //[HttpGet]
        //public HttpResponseMessage GetProductByColorWithStock()
        //{
        //    IEnumerable<ProductView> response = _productService.GetProductCountStockByColor();
        //    HttpResponseMessage httpResponse = Request.CreateResponse(HttpStatusCode.OK, response);
        //    return httpResponse;
        //}

        //public GetProductsByCategoryRequest GenerateProductByCategoryRequest(int CategoryId)
        //{
        //    GetProductsByCategoryRequest request = new GetProductsByCategoryRequest();
        //    request.CategoryId = CategoryId;
        //    request.SortBy = ProductSort.PriceToLow;
        //    return request;
        //}
        //[HttpPost]
        //public HttpResponseMessage GetProductByProductItems(ProductSearchCriteria productRequest)
        //{
        //    GetProductsByCategoryRequest request = GenerateProductByItemRequest(productRequest);
        //    GetProductsByCategoryResponse response = _productService.GetProductByCategory(request);

        //    HttpResponseMessage httpResponse = Request.CreateResponse(HttpStatusCode.OK, response);

        //    return httpResponse;
        //}
        ////public GetProductsByCategoryRequest GenerateProductByItemRequest(ProductSearchCriteria productCriteria)
        ////{
        ////    GetProductsByCategoryRequest request = new GetProductsByCategoryRequest();
        ////    request.CategoryId = productCriteria.CategoryId;
        ////    request.SortBy = ProductsSortBy.PriceToLow;
        ////    return request;
        ////}
        [HttpGet]
        public HttpResponseMessage GetProductDetails(int id)
        {
            GetProductTitleDetailsRequest request=new GetProductTitleDetailsRequest(){ Id=id};
            GetProductTitleDetailsResponse response =_productService.GetProductDetails(request);
            HttpResponseMessage httpResponse = Request.CreateResponse(HttpStatusCode.OK, response);
            return httpResponse;
        }
        [HttpPost]
        public string AddProductImage()
        {
            string imageId = Guid.NewGuid().ToString();
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var httpFileUpload = HttpContext.Current.Request.Files["UploadImage"];
                if (httpFileUpload != null)
                {
                    var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/ProductImage"), imageId + ".jpg");
                    httpFileUpload.SaveAs(fileSavePath);
                }
            }
            return imageId;

        }
        public void ModifyProductImages(int imageId)
        {
            ModifyProductTitleImageRequest request = new ModifyProductTitleImageRequest();
            request.ImageId = imageId;
            _productService.ModifyProductImage(request);
        }
        public void ChangePicture(int ImageId)
        {
            string PrimaryImage = ImageId.ToString();
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var httpFileUpload = HttpContext.Current.Request.Files["UploadImage"];
                if (httpFileUpload != null)
                {
                    var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/ProductImage"), PrimaryImage + ".jpg");
                    httpFileUpload.SaveAs(fileSavePath);

                    //FileInfo fileInfo = new FileInfo(fileSavePath);
                    //fileInfo.Replace(fileSavePath, "lps");
                }
            }
        }
        [HttpGet]
        public int GetProductImage()
        {
            return _productService.CountLastProductImage();
        }
        [HttpPost]
        public HttpResponseMessage CreateProductTitleWithAssign(CreateProductTitleRequest request)
        {
            CreateProductTitleResponse response = _productService.CreateProductTitle(request);
            CreateProductRequest productRequest = new CreateProductRequest();
            productRequest.ProductTitleId = response.ProductTitle.Id;
            productRequest.SizeId = request.SizeId;
            _productService.AssignProductTitleToProduct(productRequest);

            HttpResponseMessage httpResponse = Request.CreateResponse(HttpStatusCode.OK, response.ProductTitle);

            return httpResponse;
        }
        [HttpPost]
        public HttpResponseMessage CreateProductTitles(CreateProductTitleRequest request)
        {
            CreateProductTitleResponse response = _productService.CreateProductTitle(request);
            HttpResponseMessage httpResponse = Request.CreateResponse(HttpStatusCode.OK, response.ProductTitle.Id);

            return httpResponse;

        }
        [HttpPost]
        public void ModifyProductTitles(int productTitleId, ProductTitleModifyCriteria productCriteria)
        {
            ModifyProductTitleRequest request = new ModifyProductTitleRequest();

            request.ProductTitleId = productTitleId;
            request.Name = productCriteria.Name;
            request.Price = productCriteria.Price;
            request.CategoryId = productCriteria.CategoryId;
            request.ManufactureId = productCriteria.ManufactureId;
            request.BrandId = productCriteria.BrandId;
            request.ProductModelId = productCriteria.ProductModelId;
            request.Description = productCriteria.Description;

            _productService.ModifyProductTitle(request);
        }
        [HttpPost]
        public void AssignProductTitleToProduct(CreateProductRequest request)
        {
            _productService.AssignProductTitleToProduct(request);
        }
        ////Package method
        //[HttpGet]
        //public HttpResponseMessage GetPackage(int CategoryId)
        //{
        //    GetPackageByCategoryRequest request = new GetPackageByCategoryRequest() { CategoryId = CategoryId };
        //    GetPackageByCategoryResponse response = _productService.GetPackageByCategory(request);
        //    HttpResponseMessage httpResponse = Request.CreateResponse(HttpStatusCode.OK, response);
        //    return httpResponse;
        //}
        //[HttpGet]
        //public HttpResponseMessage GetPackageDetails(int PackageId)
        //{
        //    GetPackageDetailsRequest request = new GetPackageDetailsRequest() { PackageId = PackageId };
        //    GetPackageDetailsResponse response = _productService.GetPackageDetailsByPackage(request);
        //    HttpResponseMessage httpResponse = Request.CreateResponse(HttpStatusCode.OK, response);

        //    return httpResponse;
        //}
        //[HttpPost]
        public void DeleteproductTitle(int ProductTitleId)
        {
            DeleteProductTitleRequest request = new DeleteProductTitleRequest();
            IList<int> ProductTitles = new List<int>();
            ProductTitles.Add(ProductTitleId);

            request.RemoveToProduct = ProductTitles;
            _productService.DeleteProductByProductTitle(request);
        }
        [HttpPost]
        public void DeleteProducts(JsonProductRequests jsonProducts)
        {
            DeleteProductRequest request = new DeleteProductRequest();
            request.DeleteProductId = jsonProducts.Products.ConvertToProductRequests();
            _productService.DeleteProductByProduct(request);
        }
        [HttpGet]
        public HttpResponseMessage GetAll()
        {
            GetAllProductTitleResponse response = _productService.GetAll();
            HttpResponseMessage httpResponse = Request.CreateResponse(HttpStatusCode.OK, response.ProductTitles.Distinct());
            return httpResponse;
        }
        //[HttpGet]
        //public HttpResponseMessage GetSizeAll()
        //{
        //    GetAllSizeResponse response = _productService.GetAllSize();
        //    HttpResponseMessage httpResponse = Request.CreateResponse(HttpStatusCode.OK, response);
        //    return httpResponse;
        //}

        //[HttpGet]
        //public HttpResponseMessage GetColorAll()
        //{
        //    GetAllColorResponse response = _productService.GetAllColor();
        //    HttpResponseMessage httpResponse = Request.CreateResponse(HttpStatusCode.OK, response);
        //    return httpResponse;
        //}

        //[HttpGet]
        //public HttpResponseMessage GetFabricAll()
        //{
        //    GetAllFabricsResponse response = _productService.GetAllFabric();
        //    HttpResponseMessage httpResponse = Request.CreateResponse(HttpStatusCode.OK, response);
        //    return httpResponse;
        //}

        //[HttpGet]
        //public HttpResponseMessage GetSubCategoryAll()
        //{
        //    GetAllSubCategoryResponse response = _productService.GetAllSubCategory();
        //    HttpResponseMessage httpResponse = Request.CreateResponse(HttpStatusCode.OK, response);
        //    return httpResponse;
        //}

      
        //[HttpGet]
        //public HttpResponseMessage GetProductTitle(int id)
        //{
        //    GetProductTitleRequest request=new GetProductTitleRequest(){ Id=id};
        //    GetProductTitleResponse response = _productService.GetProductTitle(request);
        //    HttpResponseMessage httpResponse = Request.CreateResponse(HttpStatusCode.OK, response);
        //    return httpResponse;
        //}
        [HttpGet]
        public HttpResponseMessage GetProduct(int id)
        {
            GetProductRequest request = new GetProductRequest() { Id = id };
            GetProductResponse response = _productService.GetProduct(request);
            HttpResponseMessage httpResponse = Request.CreateResponse(HttpStatusCode.OK, response);
            return httpResponse;
        }
        //[HttpPost]
        //public void CreateProductSampleImage(CreateProductImageRequest request)
        //{
        //    _productService.AddProductSampleImage(request);
        //}
        
        //[HttpPost]
        //public string UploadProductSampleImage()
        //{
        //    string imageId = Guid.NewGuid().ToString();
        //    if (HttpContext.Current.Request.Files.AllKeys.Any())
        //    {
        //        var img1 = HttpContext.Current.Request.Files["upImage"];
        //        var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/ProductImage"), imageId + ".jpg");
        //        img1.SaveAs(fileSavePath);

                
        //    }
        //    return imageId;
        //}

        //[HttpPost]
        //public string UploadProductSampleImageTwo()
        //{
        //    string imageId = Guid.NewGuid().ToString();
        //    if (HttpContext.Current.Request.Files.AllKeys.Any())
        //    {
        //        var httpFileUpload = HttpContext.Current.Request.Files["UploadSample2"];
        //        if (httpFileUpload != null)
        //        {
        //            var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/ProductImage"), imageId + ".jpg");
        //            httpFileUpload.SaveAs(fileSavePath);
        //        }
        //    }
        //    return imageId;
        //}
        [HttpPost]
        public HttpResponseMessage GetProductsByAjax(JsonProductSearchRequest jsonProductSearchRequest)
        {
            GetProductsByCategoryRequest productSearchRequest =
                        GenerateProductSearchRequestFrom(jsonProductSearchRequest);
            GetProductsByCategoryResponse response = _productService.GetProductsByCategory(productSearchRequest);
            HttpResponseMessage httpResponse = Request.CreateResponse(HttpStatusCode.OK, response);
            return httpResponse;
        }
        private static GetProductsByCategoryRequest GenerateProductSearchRequestFrom(
                                     JsonProductSearchRequest jsonProductSearchRequest)
        {
            GetProductsByCategoryRequest productSearchRequest =
                                              new GetProductsByCategoryRequest();

            productSearchRequest.NumberOfResultsPerPage = 6;
            productSearchRequest.Index = jsonProductSearchRequest.Index;
            productSearchRequest.CategoryId = jsonProductSearchRequest.CategoryId;
            productSearchRequest.SortBy = jsonProductSearchRequest.SortBy;

            List<RefinementGroup> refinementGroups = new List<RefinementGroup>();
            RefinementGroup refinementGroup;

            foreach (JsonRefinementGroup jsonRefinementGroup in
                                        jsonProductSearchRequest.RefinementGroups)
            {
                switch ((RefinementGroupings)jsonRefinementGroup.GroupId)
                {
                    case RefinementGroupings.brand:
                        productSearchRequest.BrandIds =
                                     jsonRefinementGroup.SelectedRefinements;
                        break;
                    case RefinementGroupings.color:
                        productSearchRequest.ColorIds =
                                     jsonRefinementGroup.SelectedRefinements;
                        break;
                    case RefinementGroupings.size:
                        productSearchRequest.SizeIds =
                                     jsonRefinementGroup.SelectedRefinements;
                        break;
                    default:
                        break;
                }
            }
            return productSearchRequest;
        }
    }
}