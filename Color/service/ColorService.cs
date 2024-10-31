using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

public interface IColorService
{
    Task<ColorDto> CreateColorAsyncService(ColorDto newColor);
    Task<List<ColorDto>> GetColorsAsyncService();
    Task<ColorDto?> GetColorByIdAsyncService(Guid colorId);
    Task<bool> DeleteColorByIdAsyncService(Guid colorId);
    Task<ColorDto?> UpdateColorByIdAsyncService(Guid colorId, ColorDto updateColor);
}

public class ColorService : IColorService
{
    private readonly AppDBContext _appDbContext;
    private readonly IMapper _mapper;

    public ColorService(AppDBContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<ColorDto> CreateColorAsyncService(ColorDto newColor)
    {
        try
        {
            var color = _mapper.Map<Color>(newColor);
            await _appDbContext.Colors.AddAsync(color);
            await _appDbContext.SaveChangesAsync();
            var colorData = _mapper.Map<ColorDto>(color);
            return colorData;
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

    public async Task<List<ColorDto>> GetColorsAsyncService()
    {
        try
        {
            var colors = await _appDbContext.Colors.ToListAsync();

            var colorsData = _mapper.Map<List<ColorDto>>(colors);
            return colorsData;
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
    public async Task<ColorDto?> GetColorByIdAsyncService(Guid colorId)
    {
        try
        {
            var color = await _appDbContext.Colors.FindAsync(colorId);
            if (color == null)
            {
                return null;
            }
            var colorData = _mapper.Map<ColorDto>(color);
            return colorData;
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

    public async Task<bool> DeleteColorByIdAsyncService(Guid colorId)
    {
        try
        {
            var color = await _appDbContext.Colors.FindAsync(colorId);
            if (color == null)
            {
                return false;
            }
            _appDbContext.Colors.Remove(color);
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

    public async Task<ColorDto?> UpdateColorByIdAsyncService(Guid colorId, ColorDto updateColor)
    {
        try
        {
            var color = await _appDbContext.Colors.FindAsync(colorId);
            if (color == null)
            {
                return null;
            }

            color.Value = updateColor.Value ?? color.Value;

            _appDbContext.Colors.Update(color);
            await _appDbContext.SaveChangesAsync();

            var colorData = _mapper.Map<ColorDto>(color);
            return colorData;
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