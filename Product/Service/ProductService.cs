using AutoMapper;
using Microsoft.EntityFrameworkCore;

public interface IProductService
{
    Task<PaginatedResult<ProductDto>> GetProductsAsyncService(PaginationQuery paginationQuery);
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
            List<Size> listSizes = new List<Size>();
            List<Color> listColors = new List<Color>();

            var product = _mapper.Map<Product>(newProduct);

            foreach (var sizeId in newProduct.SizeIds)
            {
                var size = await _appDbContext.Sizes.FindAsync(sizeId);

                if (size != null)
                {
                    listSizes.Add(size);
                }
                else
                {
                    throw new ApplicationException($"This size id {sizeId} does not exist in the sizes Db");
                }
            }
            product.Sizes = listSizes;

            foreach (var colorId in newProduct.ColorIds)
            {
                var color = await _appDbContext.Colors.FindAsync(colorId);
                if (color != null)
                {
                    listColors.Add(color);
                }
                else
                {
                    throw new ApplicationException($"This color id {colorId} does not exist in the colors Db");
                }
            }
            product.Colors = listColors;

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
    public async Task<PaginatedResult<ProductDto>> GetProductsAsyncService(PaginationQuery paginationQuery)
    {
        try
        {
            var products = await _appDbContext.Products.Include(p => p.Sizes).Include(p => p.Colors).ToListAsync();
            // using query to search for all the products whos matching the title otherwise return null
            var filterProduct = products.AsQueryable();
            if (!string.IsNullOrEmpty(paginationQuery.SearchQuery))
            {
                filterProduct = filterProduct.Where(p => p.Title.Contains(paginationQuery.SearchQuery, StringComparison.OrdinalIgnoreCase));
                if (filterProduct.Count() == 0)
                {
                    var exisitingProduct = _mapper.Map<List<ProductDto>>(filterProduct);
                    return new PaginatedResult<ProductDto>
                    {
                        Items = exisitingProduct,
                    };
                }
            }
            // sort the list of product dependes on size or color or meterial as desc or asc othwewise shows the default
            filterProduct = paginationQuery.SortBy?.ToLower() switch
            {
                "title" => paginationQuery.SortOrder == "desc" ? filterProduct.OrderByDescending(p => p.Title) : filterProduct.OrderBy(p => p.Title),
                "price" => paginationQuery.SortOrder == "desc" ? filterProduct.OrderByDescending(p => p.Price) : filterProduct.OrderBy(p => p.Price),
                _ => filterProduct.OrderBy(p => p.Title) // default 
            };

            var totalCount = filterProduct.Count();

            // return the result in pagination 
            var paginationResult = filterProduct.Skip((paginationQuery.PageNumber - 1) * paginationQuery.PageSize).Take(paginationQuery.PageSize);
            var productData = _mapper.Map<List<ProductDto>>(paginationResult);
            return new PaginatedResult<ProductDto>
            {
                Items = productData,
                TotalCount = totalCount,
                PageNumber = paginationQuery.PageNumber,
                PageSize = paginationQuery.PageSize
            };
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
            var product = await _appDbContext.Products.Include(p => p.Sizes).Include(p => p.Colors).SingleOrDefaultAsync(p => p.ProductId == productId);
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
                List<Size> listSizes = new List<Size>();
                foreach (var sizeId in updateProduct.SizeIds)
                {
                    var size = await _appDbContext.Sizes.FindAsync(sizeId);

                    if (size != null)
                    {
                        listSizes.Add(size);
                    }
                    else
                    {
                        throw new ApplicationException($"This size id {sizeId} does not exist in the sizes Db");
                    }
                }
                product.Sizes = listSizes;

            }

            if (updateProduct.ColorIds != null)
            {
                List<Color> listColors = new List<Color>();
                foreach (var colorId in updateProduct.ColorIds)
                {
                    var color = await _appDbContext.Colors.FindAsync(colorId);
                    if (color != null)
                    {
                        listColors.Add(color);
                    }
                    else
                    {
                        throw new ApplicationException($"This color id {colorId} does not exist in the colors Db");
                    }
                }
                product.Colors = listColors;
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