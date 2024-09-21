using ProductApi.Domain.Entities;

namespace ProductApi.Application.DTOs.Mapper
{
    public static class ProductMapper
    {
        public static Product ToEntity(ProductDTO productDTO) => new()
        {

            Id = productDTO.Id,
            Name = productDTO.Name,
            Price = productDTO.Price,
            Quntity = productDTO.Quntity,
            Description = productDTO.Description,
            Category = productDTO.Category
        };

        // convert from DTO to entity to help stwich from Database data and the DTO
        public static (ProductDTO?, IEnumerable<ProductDTO>?) FromEntity(Product product, IEnumerable<Product>? products)
        {
            // Return Single Product
            if (product is not null || products is null)
            {
                var singleProduct = new ProductDTO
                (
                    product!.Id,
                    product.Category,
                    product.Description,
                    product.Name,
                    product.Price,
                    product.Quntity
                );
                return (singleProduct, null);
            }
            //return list of products
            if (products is not null || product is null)
            {
                var listProducts = products!.Select(p => new ProductDTO
                (
                    p.Id,
                    p.Category,
                    p.Description,
                    p.Name,
                    p.Price,
                    p.Quntity
                )).ToList();
                return (null, listProducts);
            }
            return (null, null);
        }
    }
}