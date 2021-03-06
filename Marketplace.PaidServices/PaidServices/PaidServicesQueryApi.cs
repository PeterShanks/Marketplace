using Marketplace.PaidServices.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.PaidServices.PaidServices;

[Route("/api/services")]
public class PaidServicesQueryApi : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Models.PaidServiceItem>> Get()
    {
        return Ok(
            PaidService.AvailableServices.Select(
                Models.PaidServiceItem.FromDomain
            )
        );
    }
}