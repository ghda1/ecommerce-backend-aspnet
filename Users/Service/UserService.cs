using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

public interface IUserService
{
    Task<PaginatedResult<UserDto>> GetUsersAsyncService(PaginationQuery paginationQuery);
    Task<UserDto?> GetUserByIdAsyncService(Guid userId);
    Task<bool> DeleteUserByIdAsyncService(Guid userId);
    Task<UserDto?> UpdateUserByIdAsyncService(Guid userId, UpdateUserDto updateUser);
}
public class UserService : IUserService
{
    private readonly AppDBContext _appDbContext;
    private readonly IMapper _mapper;

    public UserService(AppDBContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }
    public async Task<PaginatedResult<UserDto>> GetUsersAsyncService(PaginationQuery paginationQuery)
    {
        try
        {
            var users = await _appDbContext.Users.ToListAsync();
            // using query to search for all the users whos matching the name otherwise return null
            var filterUsers = users.AsQueryable();
            if (!string.IsNullOrEmpty(paginationQuery.SearchQuery))
            {
                filterUsers = filterUsers.Where(c => c.FirstName.Contains(paginationQuery.SearchQuery, StringComparison.OrdinalIgnoreCase));
                if (filterUsers.Count() == 0)
                {
                    var exisitingUsers = _mapper.Map<List<UserDto>>(filterUsers);
                    return new PaginatedResult<UserDto>
                    {
                        Items = exisitingUsers
                    };
                }
            }
            // sort the list of users dependes on first or last name as desc or asc othwewise shows the default
            filterUsers = paginationQuery.SortBy?.ToLower() switch
            {
                "firstname" => paginationQuery.SortOrder == "desc" ? filterUsers.OrderByDescending(c => c.FirstName) : filterUsers.OrderBy(c => c.FirstName),
                "lastname" => paginationQuery.SortOrder == "desc" ? filterUsers.OrderByDescending(c => c.LastName) : filterUsers.OrderBy(c => c.LastName),
                _ => filterUsers.OrderBy(c => c.FirstName) // default 
            };
            var totalCount = filterUsers.Count();

            // return the result in pagination 
            var paginationResult = filterUsers.Skip((paginationQuery.PageNumber - 1) * paginationQuery.PageSize).Take(paginationQuery.PageSize);
            var userData = _mapper.Map<List<UserDto>>(paginationResult);
            return new PaginatedResult<UserDto>
            {
                Items = userData,
                TotalCount = totalCount,
                PageNumber = paginationQuery.PageNumber,
                PageSize = paginationQuery.PageSize
            };
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
    public async Task<UserDto?> GetUserByIdAsyncService(Guid userId)
    {
        try
        {
            var user = await _appDbContext.Users.Include(u => u.Addresses).FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                return null;
            }
            var userData = _mapper.Map<UserDto>(user);
            return userData;
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

    public async Task<bool> DeleteUserByIdAsyncService(Guid userId)
    {
        try
        {
            var user = await _appDbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }
            _appDbContext.Users.Remove(user);
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

    public async Task<UserDto?> UpdateUserByIdAsyncService(Guid userId, UpdateUserDto updateUser)
    {
        try
        {
            var user = await _appDbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return null;
            }

            user.FirstName = updateUser.FirstName ?? user.FirstName;
            user.LastName = updateUser.LastName ?? user.LastName;
            user.Email = updateUser.Email ?? user.Email;
            if (updateUser.Password != null)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(updateUser.Password);
            }
            user.Phone = updateUser.Phone ?? user.Phone;

            _appDbContext.Update(user);
            await _appDbContext.SaveChangesAsync();

            var userData = _mapper.Map<UserDto>(user);
            return userData;
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