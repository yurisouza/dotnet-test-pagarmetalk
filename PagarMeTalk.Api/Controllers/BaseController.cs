using Microsoft.AspNetCore.Mvc;
using PagarMeTalk.Api.Shared;
using System.Net;

namespace PagarMeTalk.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IActionResult GenerateResponse(HttpStatusCode statusCode, IResult result)
                    => StatusCode((int)statusCode, result);

        protected IActionResult GenerateBadRequestResponse()
            => StatusCode((int)HttpStatusCode.BadRequest, ModelState.GetNotifications());
    }
}