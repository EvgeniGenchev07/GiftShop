using BusinessLayer;
using DataLayer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLayer
{
    [TestFixture]
    public class ProductContextTest
    {
        static ProductContext productContext;
        static ProductContextTest()
        {
            productContext = new ProductContext(TestManager.dbContext);
        }

        [Test]
        public async Task CreateProduct()
        {

            Product product = new Product("Phone", "Apple",1500, 1);
            int productsBefore = TestManager.dbContext.Products.Count();

            await productContext.Create(product);

            int productsAfter = TestManager.dbContext.Products.Count();
            Product lastProduct= TestManager.dbContext.Products.Last();
            Assert.That(productsBefore + 1 == productsAfter && lastProduct.Id == product.Id,
                "Id are not equal or the product is not created!");

        }

        [Test]
        public async Task ReadProduct()
        {
           
            Product newProduct = new Product("Phone","Apple", 1500, 1);
            await productContext.Create(newProduct);

            Product product = await productContext.Read(newProduct.Id);

            Assert.That(product.Name == "Phone", "Read() does not get Product by id!");
        }

        [Test]
        public async Task ReadAllProduct()
        {
            Product newProduct = new Product("Phone", "Apple", 1500, 1);
            int productBefore = TestManager.dbContext.Products.Count();

            int productAfter = (await productContext.ReadAll()).Count;

            Assert.That(productBefore == productAfter, "ReadAll() does not return all of the Product!");
        }

        [Test]
        public async Task UpdateProduct()
        {
            Product newProduct = new Product("Phone", "Apple", 1500, 1);
            await productContext.Create(newProduct);

            Product lastProduct = (await productContext.ReadAll()).Last();
            lastProduct.Name = "Updated Product";

             await productContext.Update(lastProduct, false);

            Assert.That((await productContext.Read(lastProduct.Id)).Name == "Updated Product",
            "Update() does not change the Product's name!");
        }

        [Test]
        public async Task DeleteProdcut()
        {
            Product newProduct = new Product("Phone", "Apple", 1500, 1);
            
            await productContext.Create(newProduct);

            List<Product> products = await productContext.ReadAll();
            int productBefore = products.Count;
            Product product = products.Last();

            await productContext.Delete(product.Id);

            int productAfter = (await productContext.ReadAll()).Count;
            Assert.That(productBefore == productAfter + 1, "Delete() does not delete a product!");
        }

    }
}

