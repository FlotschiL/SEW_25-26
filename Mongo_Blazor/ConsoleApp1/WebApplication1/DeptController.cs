using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/data")]
public class DynamicDataController : ControllerBase
{
    private readonly DynamicMongoService _mongoService;

    public DynamicDataController(DynamicMongoService mongoService) => 
        _mongoService = mongoService;

    [HttpGet("{collectionName}")]
    public async Task<IActionResult> Get(string collectionName)
    {
        var data = await _mongoService.GetCollectionDataAsync(collectionName);
        return Ok(data);
    }
}