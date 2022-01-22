using System.Threading.Tasks;
using Marketplace.Ads.Domain.ClassifiedAds;
using Marketplace.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static Marketplace.Ads.Messages.Commands;


namespace Marketplace.Ads.ClassifiedAds;

[Route("api/ad")]
[Authorize]
public class ClassifiedAdsCommandsApi : CommandApi<ClassifiedAd>
{
    public ClassifiedAdsCommandsApi(
        ILogger logger,
        ClassifiedAdsCommandService applicationService)
        : base(logger, applicationService)
    {
    }

    [HttpPost]
    public Task<IActionResult> Post(V1.Create command)
    {
        return HandleCommand(command, cmd => cmd.OwnerId = GetUserId());
    }

    [Route("title")]
    [HttpPut]
    public Task<IActionResult> Put(V1.ChangeTitle command)
    {
        return HandleCommand(command);
    }

    [Route("text")]
    [HttpPut]
    public Task<IActionResult> Put(V1.UpdateText command)
    {
        return HandleCommand(command);
    }

    [Route("price")]
    [HttpPut]
    public Task<IActionResult> Put(V1.UpdatePrice command)
    {
        return HandleCommand(command);
    }

    [Route("requestpublish")]
    [HttpPut]
    public Task<IActionResult> Put(V1.RequestToPublish command)
    {
        return HandleCommand(command);
    }

    [Route("publish")]
    [HttpPut]
    public Task<IActionResult> Put(V1.Publish command)
    {
        return HandleCommand(command);
    }

    [Route("delete")]
    [HttpPost]
    public Task<IActionResult> Delete(V1.Delete command)
    {
        return HandleCommand(command);
    }

    [Route("image")]
    [HttpPost]
    public Task<IActionResult> Post(V1.UploadImage command)
    {
        return HandleCommand(command);
    }
}