using alamapp.Infrastructure.Domain;
using alamapp.Model.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alamapp.Model.Products
{
   public class Product:EntityBase<int>, IAggregateRoot
    {
        public Size Size { get; set; }
        public ProductTitle Title { get; set; }
        public SubCategory SubCategory
        {
            get { return Title.SubCategory; }
        }
        public Color Color
        {
            get { return Title.Color; }
        }

        public string Name
        {
            get { return Title.Name; }
        }

        public string Description
        {
            get { return Title.Description; }
        }

        public DateTime CreatedDate
        {
            get { return Title.CreatedDate; }
        }

        public Category Category
        {
            get { return Title.Category; }
        }
        public Brand Brand
        {
            get { return Title.Brand; }
        }
        public Manufacture Manufacture
        {
            get { return Title.Manufacture; }
        }
        public ProductModel ProductModel
        {
            get { return Title.ProductModel; }
        }

        public decimal Price
        {
            get { return Title.Price; }
        }
        public Guid ImageId
        {
            get { return Title.ImageId; }
        }
        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
