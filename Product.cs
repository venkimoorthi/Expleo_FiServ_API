using System;
using System.Collections.Generic;
using System.Text;

namespace ProductService
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductAsync(CancellationToken ct);
        Task<Product> GetProductByIDAsync(int id, CancellationToken ct);
        Task<Product> AddProduct(Product value, CancellationToken ct);
        Task<Product> UpdateProduct(int id, Product value, CancellationToken ct);
        Task<bool> DeleteProduct(int id, CancellationToken ct);
    }
    public class Product: IProductService
    {
        public SingletonProductRepo repo { get; set; }
        public Product()
        {
            repo = SingletonProductRepo.Instance;

        }

        public Task<List<Product>> GetAllProductAsync(CancellationToken ct)
        {
            return Task.FromResult(repo.Products);
        }
        public Task<Product> GetProductByIDAsync(int id, CancellationToken ct)
        {
            var res = repo.Products.Where(e => e.ID == id).FirstOrDefault();
            return Task.FromResult(res);
        }
        public Task<Product> AddProduct(Product value, CancellationToken ct)
        {
            if (repo.Products.Count > 0)
            {
                var res = repo.Products.Select(i => i.ID).Max();
                value.ID = res + 1;
            }
            else
            {
                value.ID = 1;
            }
            repo.Products.Add(value);
            return Task.FromResult(value);
        }
        public Task<Product> UpdateProduct(int id, Product value, CancellationToken ct)
        {

            var res = repo.Products.Where(e => e.ID == id).FirstOrDefault();
            value.ID = res.ID;
            repo.Products.Remove(res);
            repo.Products.Add(value);
            return Task.FromResult(value);
        }


        public Task<bool> DeleteProduct(int id, CancellationToken ct)
        {
            var res = repo.Products.Where(e => e.ID == id).FirstOrDefault();
            if (res != null)
            {
                repo.Products.Remove(res);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

    }
}
