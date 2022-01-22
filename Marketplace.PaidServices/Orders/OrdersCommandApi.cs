using Marketplace.PaidServices.Messages.Orders;
using Marketplace.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.PaidServices.Orders;

[Route("api/order")]
[Authorize]
public class OrdersCommandApi : ControllerBase
{
    private readonly OrdersCommandService _app;

    public OrdersCommandApi(OrdersCommandService commandService)
    {
        _app = commandService;
    }

    [HttpPost]
    public Task<ActionResult> Post(Commands.V1.CreateOrder command)
    {
        return this.HandleCommand(_app.Handle(command));
    }

    [Route("addservice")]
    [HttpPost]
    public Task<ActionResult> Post(Commands.V1.AddService command)
    {
        return this.HandleCommand(_app.Handle(command));
    }

    [Route("removeservice")]
    [HttpPost]
    public Task<ActionResult> Post(Commands.V1.RemoveService command)
    {
        return this.HandleCommand(_app.Handle(command));
    }

    [Route("fulfill")]
    [HttpPost]
    public Task<ActionResult> Post(Commands.V1.FulfillOrder command)
    {
        return this.HandleCommand(_app.Handle(command));
    }
}