using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Modules.Images;

[ApiController]
[Route("image")]
public class PictureApi : ControllerBase
{
    private readonly ImageQueryService _queryService;

    public PictureApi(ImageQueryService queryService)
    {
        _queryService = queryService;
    }

    [HttpGet]
    public Task<byte[]> GetFile(string file)
    {
        return _queryService.GetFile(file);
    }
}