using BreweryManagementSystemWebAPI.DTOs;
using BreweryManagementSystemWebAPI.Models;
using BreweryManagementSystemWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class BeerController : ControllerBase
{
    private readonly IBeerService _beerService;

    public BeerController(IBeerService beerService)
    {
        _beerService = beerService;
    }

    // FR1- List all the beers by brewery.
    [HttpPost("ListBeersByBrewery")]
    //public async Task<IActionResult> ListBeersByBrewery([FromQuery] int? breweryId, [FromQuery] string? breweryName)
    public async Task<IActionResult> ListBeersByBrewery([FromBody] BreweryDto? breweryDto)
    {
        try
        {

            if (breweryDto == null)
            {
                return BadRequest("BreweryDto cannot be null.");
            }

            var brewery = new Brewery
            {
                br_Id = breweryDto.br_Id ?? 0,
                br_Name = breweryDto.br_Name ?? "",

            };

            var beers = await _beerService.ListBeersByBreweryAsync(brewery.br_Id, brewery.br_Name);
            return Ok(beers);
        }
        catch (Exception ex)
        {
            // Return the detailed Error message in the response
            return BadRequest(ex.Message);
        }
    }

    // FR2- A brewer can add new beer.
    [HttpPost("AddBeer")]
    public async Task<IActionResult> AddBeer([FromBody] BeerDto? beerDto)
    {

        try
        {
            if (beerDto == null)
            {
                return BadRequest("BeerDto cannot be null.");
            }

            var beer = new Beer
            {
                be_Id = 0,
                be_Name = beerDto.be_Name ?? "",
                be_AlcoholContent = beerDto.be_AlcoholContent ?? 0, 
                be_Price = beerDto.be_Price ?? 0,
                br_Id = beerDto.br_Id ?? 0,
                br_Name = beerDto.br_Name ?? "",

            };

            var addedBeer = await _beerService.AddBeerAsync(beerDto.be_Name, beerDto.be_AlcoholContent, beerDto.be_Price, beerDto.br_Id, beerDto.br_Name);
            return Ok(addedBeer);
        }
        catch (Exception ex)
        {
            // Return the detailed Error message in the response
            return BadRequest(ex.Message);
        }
    }

    // FR3- A brewer can delete a beer
    [HttpPost("DeleteBeer")]
    //public async Task<IActionResult> DeleteBeer([FromQuery] int? beerId, [FromQuery] string? beerName)
    public async Task<IActionResult> DeleteBeer([FromBody] BeerDto? beerDto)
    {

        try
        {
            if (beerDto == null)
            {
                return BadRequest("BeerDto cannot be null.");
            }

            var beer = new Beer
            {
                be_Id = beerDto.be_Id ?? 0,
                be_Name = beerDto.be_Name ?? "",
                be_AlcoholContent = 0, //beerDto.be_AlcoholContent ?? 0,
                be_Price = 0, // beerDto.be_Price ?? 0,
                br_Id = 0, // beerDto.br_Id ?? 0,
                br_Name = "", //beerDto.br_Name ?? "",

            };

            var deletedBeer = await _beerService.DeleteBeerAsync(beer.be_Id, beer.be_Name);
            return Ok(deletedBeer);
        }
        catch (Exception ex)
        {
            // Return the detailed Error message in the response
            return BadRequest(ex.Message);
        }
    }
}
