using System.Runtime.InteropServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/colors")]
public class ColorController : ControllerBase
{
    private readonly IColorService _service;

    public ColorController(IColorService service)
    {
        _service = service;
    }

    // GET: api/colors/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetColorById(Guid id)
    {
        try
        {
            var color = await _service.GetColorByIdAsyncService(id);

            if (color == null)
            {
                return ApiResponses.NotFound("Color not found");
            }

            return ApiResponses.Success(color, "Color returned successfully");
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

    // GET: api/color
    [HttpGet]
    public async Task<IActionResult> GetAllColors()
    {
        try
        {
            var colors = await _service.GetColorsAsyncService();

            if (colors.Count() == 0)
            {
                return ApiResponses.NotFound("No colors found");
            }

            return ApiResponses.Success(colors, "Colors retrieved successfully");


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

    // POST: api/colors
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateColor([FromBody] ColorDto newColor)
    {
        try
        {
            var color = await _service.CreateColorAsyncService(newColor);
            return ApiResponses.Created(color, "Color created successfully");
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

    // DELETE: api/colors/{id}
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteColor(Guid id)
    {
        try
        {
            var color = await _service.DeleteColorByIdAsyncService(id);
            if (!color)
            {
                return ApiResponses.NotFound("Color not found");
            }

            return ApiResponses.Success("Color deleted successfully");
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