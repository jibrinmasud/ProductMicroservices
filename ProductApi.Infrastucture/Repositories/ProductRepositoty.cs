using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProductApi.Application.Interface;
using ProductApi.Domain.Entities;
using ProductApi.Infrastucture.Data;
using ProductCatalog.SharedLibrary.Logs;
using ProductCatalog.SharedLibrary.Responses;

namespace ProductApi.Infrastucture.Repositories
{
    public class ProductRepositoty(ProductDbContext _context) : IProduct
    {
        public async Task<Response> CreateAsync(Product entity)
        {
            try
            {
                // check if product already exist
                var getProduct = await GetByAsync(x => x.Name == entity.Name);
                if (getProduct != null && !string.IsNullOrEmpty(getProduct.Name))
                {
                    return new Response(false, $"{entity.Name} already added");
                }
                var currentEntity = _context.Products.Add(entity).Entity;
                await _context.SaveChangesAsync();
                if (currentEntity is not null && currentEntity.Id > 0)
                {
                    return new Response(true, $"{entity.Name} added successfully");
                }
                else
                    return new Response(false, "an Error has occured while adding a new Product");
            }
            catch (Exception ex)
            {
                //Log the original exception
                LogExceptions.LogException(ex);

                //display Error message to User
                return new Response(false, "an Error has occured while adding a new Product");

            }

        }

        public async Task<Response> DeleteAsync(Product entity)
        {
            try
            {
                //check for the products
                var product = await FindByIdAsync(entity.Id);
                if (product is null)
                    return new Response(false, $"{entity.Name} found not found");
                _context.Remove(product);
                await _context.SaveChangesAsync();
                return new Response(true, $"{entity.Name} has been deleted successfully");
            }
            catch (Exception ex)
            {
                //Log the original exception
                LogExceptions.LogException(ex);

                //display Error message to User
                return new Response(false, "an Error has occured while deleting a Product");
            }
        }

        public async Task<Product> FindByIdAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                return product is not null ? product : null!;

            }
            catch (Exception ex)
            {
                //Log the original exception
                LogExceptions.LogException(ex);

                //display Error message to User
                throw new Exception("an Error has occured while Retriving a Product");
                //return new Response(false, "an Error has occured while Retriving a Product");
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                var products = await _context.Products.AsNoTracking().ToListAsync();
                return products is not null ? products : null!;
            }
            catch (Exception ex)
            {
                //Log the original exception
                LogExceptions.LogException(ex);

                //display Error message to User
                throw new InvalidOperationException("an Error has occured while Retriving Products");
            }
        }

        public async Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                var product = await _context.Products.Where(predicate).FirstOrDefaultAsync()!;
                return product is not null ? product : null!;
            }
            catch (Exception ex)
            {
                //Log the original exception
                LogExceptions.LogException(ex);

                //display Error message to User
                throw new InvalidOperationException("an Error has occured while Retriving Products");
            }
        }

        public async Task<Response> UpdateAsync(Product entity)
        {
            try
            {
                var product = await FindByIdAsync(entity.Id);
                if (product is null)
                    return new Response(false, $"{entity.Name} not found");
                _context.Entry(product).State = EntityState.Detached;
                _context.Products.Update(entity);
                await _context.SaveChangesAsync();
                return new Response(true, $"{entity.Name} has been updated successfully");
            }
            catch (Exception ex)
            {
                //Log the original exception
                LogExceptions.LogException(ex);

                //display Error message to User
                return new Response(false, "an Error has occured while updating a Product");
            }
        }
    }
}