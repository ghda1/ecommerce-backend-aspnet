using System.Runtime.InteropServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/sizes")]
public class SizeController : ControllerBase
{
    private readonly ISizeService _service;

    public SizeController(ISizeService service)
    {
        _service = service;
    }

    // GET: api/sizes/{id}
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSizeById(Guid id)
    {
        try
        {
            var size = await _service.GetSizeByIdAsyncService(id);

            if (size == null)
            {
                return ApiResponses.NotFound("Size not found");
            }

            return ApiResponses.Success(size, "Size returned successfully");
        }
        catch (ApplicationException ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
    }

    // GET: api/sizes
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllSizes()
    {
        try
        {
            var sizes = await _service.GetSizesAsyncService();

            if (sizes.Count() == 0)
            {
                return ApiResponses.NotFound("No sizes found");
            }

            return ApiResponses.Success(sizes, "Sizes retrieved successfully");


        }
        catch (ApplicationException ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
    }

    // POST: api/sizes
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateSize([FromBody] SizeDto newSize)
    {
        try
        {
            var size = await _service.CreateSizeAsyncService(newSize);
            return ApiResponses.Created(size, "Size created successfully");
        }
        catch (ApplicationException ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
    }

    // DELETE: api/sizes/{id}
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSize(Guid id)
    {
        try
        {
            var size = await _service.DeleteSizeByIdAsyncService(id);
            if (!size)
            {
                return ApiResponses.NotFound("Size not found");
            }

            return ApiResponses.Success("Size deleted successfully");
        }
        catch (ApplicationException ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
    }
}