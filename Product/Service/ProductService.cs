using AutoMapper;
using Microsoft.EntityFrameworkCore;

public interface IProductService
{
    Task<List<ProductDto>> GetProductsAsyncService(int pageNumber, int pageSize, string searchQuery, string sortBy, string sortOrder);
    Task<ProductDto> CreateProductAsyncService(CreateProductDto newProduct);
    Task<ProductDto?> GetProductByIdAsyncService(Guid productId);
    Task<bool> DeleteProductByIdAsyncService(Guid productId);
    Task<ProductDto?> UpdateProductByIdAsyncService(Guid productId, UpdateProductDto updateProduct);
}

public class ProductService : IProductService
{
    private readonly AppDBContext _appDbContext;
    private readonly IMapper _mapper;

    public ProductService(AppDBContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }
    public async Task<ProductDto> CreateProductAsyncService(CreateProductDto newProduct)
    {
        try
        {
            System.Console.WriteLine("service layer to create a product");
            var product = _mapper.Map<Product>(newProduct);
            var sizes = new List<string>();
            foreach (var sizeId in newProduct.SizeIds)
            {
                Console.WriteLine($"here 1");

                var size = await _appDbContext.Sizes.FindAsync(sizeId);
                Console.WriteLine($"here 2 {size}");
                if (size != null)
                {
                    Console.WriteLine($"here 3");
                    var sizeValue = _mapper.Map<Size>(size.Value);

                    Console.WriteLine($"here 4");
                    product.Sizes.Add(size);
                    await _appDbContext.Products.AddAsync(product);
                    Console.WriteLine($"here 5");
                }
                else
                {
                    throw new ApplicationException($"This size id {sizeId} does not exist in the sizes Db");
                }
            }

            foreach (var colorId in newProduct.ColorIds)
            {
                var color = await _appDbContext.Colors.FindAsync(colorId);
                if (color != null)
                {
                    //    var colorValue = _mapper.Map<Color>(color.Value);
                    product.Colors.Add(color);
                }
                else
                {
                    throw new ApplicationException($"This color id {colorId} does not exist in the colors Db");
                }
            }

            await _appDbContext.Products.AddAsync(product);
            await _appDbContext.SaveChangesAsync();
            var productData = _mapper.Map<ProductDto>(product);
            return productData;
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while saving the product to the database. Please try again.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }
    }

    // Get all products
    public async Task<List<ProductDto>> GetProductsAsyncService(int pageNumber, int pageSize, string searchQuery, string sortBy, string sortOrder)
    {
        try
        {
            var products = await _appDbContext.Products.Include(p => p.Sizes).Include(p => p.Colors).ToListAsync();
            // using query to search for all the products whos matching the title otherwise return null
            var filterProduct = products.AsQueryable();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                filterProduct = filterProduct.Where(p => p.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
                if (filterProduct.Count() == 0)
                {
                    var exisitingProduct = _mapper.Map<List<ProductDto>>(filterProduct);
                    return exisitingProduct;
                }
            }
            // sort the list of product dependes on size or color or meterial as desc or asc othwewise shows the default
            filterProduct = sortBy?.ToLower() switch
            {
                "title" => sortOrder == "desc" ? filterProduct.OrderByDescending(p => p.Title) : filterProduct.OrderBy(p => p.Title),
                "price" => sortOrder == "desc" ? filterProduct.OrderByDescending(p => p.Price) : filterProduct.OrderBy(p => p.Price),
                _ => filterProduct.OrderBy(p => p.Title) // default 
            };
            // return the result in pagination 
            var paginationResult = filterProduct.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var productData = _mapper.Map<List<ProductDto>>(paginationResult);
            return productData;
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while retrieving products from the database.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }
    }

    // Get product by ID
    public async Task<ProductDto?> GetProductByIdAsyncService(Guid productId)
    {
        try
        {
            var product = await _appDbContext.Products.FindAsync(productId);
            if (product == null)
            {
                return null;
            }
            var productData = _mapper.Map<ProductDto>(product);
            return productData;
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while retrieving the product from the database.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }
    }

    // Delete product by ID
    public async Task<bool> DeleteProductByIdAsyncService(Guid productId)
    {
        try
        {
            var product = await _appDbContext.Products.FindAsync(productId);
            if (product == null)
            {
                return false;
            }
            _appDbContext.Products.Remove(product);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while deleting the product from the database.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }
    }

    // Update product by ID
    public async Task<ProductDto?> UpdateProductByIdAsyncService(Guid productId, UpdateProductDto updateProduct)
    {
        try
        {
            var product = await _appDbContext.Products.FindAsync(productId);
            if (product == null)
            {
                return null;
            }

            product.Material = updateProduct.Material ?? product.Material;
            product.Image = updateProduct.Image ?? product.Image;
            product.Title = updateProduct.Title ?? product.Title;
            product.Price = updateProduct.Price ?? product.Price;

            if (updateProduct.SizeIds != null)
            {
                foreach (var sizeId in updateProduct.SizeIds)
                {
                    var size = await _appDbContext.Sizes.FindAsync(sizeId);
                    if (size != null)
                    {
                        var sizeValue = _mapper.Map<Size>(size.Value);
                        product.Sizes.Add(sizeValue);
                    }
                    else
                    {
                        throw new ApplicationException($"This size id {sizeId} does not exist in the sizes Db");
                    }
                }
            }

            if (updateProduct.ColorIds != null)
            {
                foreach (var colorId in updateProduct.ColorIds)
                {
                    var color = await _appDbContext.Colors.FindAsync(colorId);
                    if (color != null)
                    {
                        var colorValue = _mapper.Map<Color>(color.Value);
                        product.Colors.Add(colorValue);
                    }
                    else
                    {
                        throw new ApplicationException($"This size id {colorId} does not exist in the sizes Db");
                    }
                }
            }

            _appDbContext.Products.Update(product);
            await _appDbContext.SaveChangesAsync();

            var productData = _mapper.Map<ProductDto>(product);
            return productData;
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while updating the product in the database.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }
    }
}