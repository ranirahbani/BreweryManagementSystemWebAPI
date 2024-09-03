using BreweryManagementSystemWebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class WholesalersController : ControllerBase
{
    //private readonly IWholesalerService _wholesalerService;

    //public WholesalersController(IWholesalerService wholesalerService)
    //{
    //    _wholesalerService = wholesalerService;
    //}

    //[HttpGet]
    //public async Task<IActionResult> GetAll()
    //{
    //    var wholesalers = await _wholesalerService.GetAllWholesalersAsync();
    //    return Ok(wholesalers);
    //}

    //[HttpGet("{id}")]
    //public async Task<IActionResult> GetById(int id)
    //{
    //    var wholesaler = await _wholesalerService.GetWholesalerByIdAsync(id);
    //    if (wholesaler == null)
    //        return NotFound();
    //    return Ok(wholesaler);
    //}

    //[HttpPost]
    //public async Task<IActionResult> Add([FromBody] WholesalerDto wholesalerDto)
    //{
    //    await _wholesalerService.AddWholesalerAsync(wholesalerDto);
    //    return CreatedAtAction(nameof(GetById), new { id = wholesalerDto.wh_Id }, wholesalerDto);
    //}

    //[HttpPut]
    //public async Task<IActionResult> Update([FromBody] WholesalerDto wholesalerDto)
    //{
    //    await _wholesalerService.UpdateWholesalerAsync(wholesalerDto);
    //    return NoContent();
    //}

    //[HttpDelete("{id}")]
    //public async Task<IActionResult> Delete(int id)
    //{
    //    await _wholesalerService.DeleteWholesalerAsync(id);
    //    return NoContent();
    //}
}

