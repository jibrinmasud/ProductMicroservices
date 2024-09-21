using Azure;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.DTOs.Mapper;
using ProductApi.Application.Interface;

namespace ProductApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(IProduct _iproduct) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProduct()
        {
            //get all products
            var products = await _iproduct.GetAllAsync();
            if (!products.Any())
                return NotFound("Product Does not Exist");
            var (_, list) = ProductMapper.FromEntity(null!, products);
            return list!.Any() ? Ok(list) : NotFound("No product Found");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            //get a single product by ID
            var product = await _iproduct.FindByIdAsync(id);
            if (product == null)
                return NotFound("Requested Product Not Found");
            //map products from entity to DTO
            var (_product, _) = ProductMapper.FromEntity(product, null!);
            return _product is not null ? Ok(_product) : NotFound("Requested Product Not Found");
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateProduct(ProductDTO productDTO)
        {

            //check the model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var getEntity = ProductMapper.ToEntity(productDTO);
            var response = await _iproduct.CreateAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response.Message);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, ProductDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var getEntity = ProductMapper.ToEntity(productDTO);
            var response = await _iproduct.UpdateAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        public async Task<ActionResult<Response>> DeleteProduct(ProductDTO productDTO)
        {
            var getEntity = ProductMapper.ToEntity(productDTO);
            var response = await _iproduct.DeleteAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }

    }
}