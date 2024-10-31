using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

public interface ISizeService
{
    Task<SizeDto> CreateSizeAsyncService(SizeDto newSize);
    Task<List<SizeDto>> GetSizesAsyncService();
    Task<SizeDto?> GetSizeByIdAsyncService(Guid sizeId);
    Task<bool> DeleteSizeByIdAsyncService(Guid sizeId);
    Task<SizeDto?> UpdateSizeByIdAsyncService(Guid sizeId, SizeDto updateSize);
}

public class SizeService : ISizeService
{
    private readonly AppDBContext _appDbContext;
    private readonly IMapper _mapper;

    public SizeService(AppDBContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<SizeDto> CreateSizeAsyncService(SizeDto newSize)
    {
        try
        {
            var size = _mapper.Map<Size>(newSize);
            await _appDbContext.Sizes.AddAsync(size);
            await _appDbContext.SaveChangesAsync();
            var sizeData = _mapper.Map<SizeDto>(size);
            return sizeData;
        }
        catch (DbUpdateException dbEx)
        {
            // Handle database update exceptions
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.");
        }
        catch (Exception ex)
        {
            // Handle any other unexpected exceptions
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }
    }

    public async Task<List<SizeDto>> GetSizesAsyncService()
    {
        try
        {
            var sizes = await _appDbContext.Sizes.ToListAsync();

            var sizesData = _mapper.Map<List<SizeDto>>(sizes);
            return sizesData;
        }
        catch (DbUpdateException dbEx)
        {
            // Handle database update exceptions
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.");
        }
        catch (Exception ex)
        {
            // Handle any other unexpected exceptions
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }

    }
    public async Task<SizeDto?> GetSizeByIdAsyncService(Guid sizeId)
    {
        try
        {
            var size = await _appDbContext.Sizes.FindAsync(sizeId);
            if (size == null)
            {
                return null;
            }
            var sizeData = _mapper.Map<SizeDto>(size);
            return sizeData;
        }
        catch (DbUpdateException dbEx)
        {
            // Handle database update exceptions
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.");
        }
        catch (Exception ex)
        {
            // Handle any other unexpected exceptions
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }
    }

    public async Task<bool> DeleteSizeByIdAsyncService(Guid sizeId)
    {
        try
        {
            var size = await _appDbContext.Sizes.FindAsync(sizeId);
            if (size == null)
            {
                return false;
            }
            _appDbContext.Sizes.Remove(size);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException dbEx)
        {
            // Handle database update exceptions
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.");
        }
        catch (Exception ex)
        {
            // Handle any other unexpected exceptions
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }
    }

    public async Task<SizeDto?> UpdateSizeByIdAsyncService(Guid sizeId, SizeDto updateSize)
    {
        try
        {
            var size = await _appDbContext.Sizes.FindAsync(sizeId);
            if (size == null)
            {
                return null;
            }

            size.Value = updateSize.Value ?? size.Value;

            _appDbContext.Sizes.Update(size);
            await _appDbContext.SaveChangesAsync();

            var sizeData = _mapper.Map<SizeDto>(size);
            return sizeData;
        }
        catch (DbUpdateException dbEx)
        {
            // Handle database update exceptions
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.");
        }
        catch (Exception ex)
        {
            // Handle any other unexpected exceptions
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }
    }
}