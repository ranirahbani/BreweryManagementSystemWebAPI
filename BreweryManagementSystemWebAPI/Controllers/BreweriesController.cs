using BreweryManagementSystemWebAPI.DTOs;
using BreweryManagementSystemWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BreweriesController : ControllerBase
{
    //private readonly IBreweryService _breweryService;

    //public BreweriesController(IBreweryService breweryService)
    //{
    //    _breweryService = breweryService;
    //}

    //[HttpGet]
    //public async Task<IActionResult> GetAll()
    //{
    //    var breweries = await _breweryService.GetAllBreweriesAsync();
    //    return Ok(breweries);
    //}

    //[HttpGet("{id}")]
    //public async Task<IActionResult> GetById(int id)
    //{
    //    var brewery = await _breweryService.GetBreweryByIdAsync(id);
    //    if (brewery == null)
    //        return NotFound();
    //    return Ok(brewery);
    //}

    //[HttpPost]
    //public async Task<IActionResult> Add([FromBody] BreweryDto breweryDto)
    //{
    //    await _breweryService.AddBreweryAsync(breweryDto);
    //    return CreatedAtAction(nameof(GetById), new { id = breweryDto.br_Id }, breweryDto);
    //}

    //[HttpPut]
    //public async Task<IActionResult> Update([FromBody] BreweryDto breweryDto)
    //{
    //    await _breweryService.UpdateBreweryAsync(breweryDto);
    //    return NoContent();
    //}

    //[HttpDelete("{id}")]
    //public async Task<IActionResult> Delete(int id)
    //{
    //    await _breweryService.DeleteBreweryAsync(id);
    //    return NoContent();
    //}
}
