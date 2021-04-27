using Microsoft.AspNetCore.Mvc;
using PagarMeTalk.Api.Models;
using PagarMeTalk.Api.Services;
using PagarMeTalk.Api.Shared;
using System;
using System.Net;

namespace PagarMeTalk.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : BaseController
    {
        private readonly IOrderService _service;

        public OrdersController(IOrderService service)
        {
            _service = service;
        }


        [HttpGet("{id}")]
        [Produces("application/json")]
        public IActionResult GetOrder([FromRoute] string id)
        {
            if (Guid.TryParse(id, out var idGuid) == false)
                return GenerateResponse(HttpStatusCode.BadRequest, ErrorExtensions.CreateNotification("id", "Identificador inválido"));

            var response = _service.GetOrder(idGuid);

            if (response.Success)
                return GenerateResponse(HttpStatusCode.OK, response);

            return GenerateResponse(HttpStatusCode.NotFound, response);
        }

        [HttpPost]
        public IActionResult BeginOrder()
        {
            if (ModelState.IsValid == false)
                return GenerateBadRequestResponse();

            var response = _service.BeginOrder();

            if (response.Success)
                return GenerateResponse(HttpStatusCode.Created, response);

            return GenerateResponse(HttpStatusCode.BadRequest, response);
        }

        [HttpPatch("close/{id}")]
        public IActionResult CloseOrder([FromRoute] string id)
        {
            if (Guid.TryParse(id, out var idGuid) == false)
                return GenerateResponse(HttpStatusCode.BadRequest, ErrorExtensions.CreateNotification("id", "Identificador inválido"));

            var response = _service.CloseOrder(idGuid);

            if (response.Success)
                return GenerateResponse(HttpStatusCode.OK, response);

            return GenerateResponse(HttpStatusCode.BadRequest, response);
        }

        [HttpPatch("paid/{id}")]
        public IActionResult PaidOrder([FromRoute] string id, [FromBody] PaidOrderModel model)
        {
            if (Guid.TryParse(id, out var idGuid) == false)
                return GenerateResponse(HttpStatusCode.BadRequest, ErrorExtensions.CreateNotification("id", "Identificador inválido"));

            if (ModelState.IsValid == false)
                return GenerateBadRequestResponse();

            model.Id = idGuid;
            var response = _service.PaidOrder(model);

            if (response.Success)
                return GenerateResponse(HttpStatusCode.OK, response);

            return GenerateResponse(HttpStatusCode.BadRequest, response);
        }

        [HttpPatch("cancel/{id}")]
        public IActionResult CancelOrder([FromRoute] string id)
        {
            if (Guid.TryParse(id, out var idGuid) == false)
                return GenerateResponse(HttpStatusCode.BadRequest, ErrorExtensions.CreateNotification("id", "Identificador inválido"));

            var response = _service.CancelOrder(idGuid);

            if (response.Success)
                return GenerateResponse(HttpStatusCode.OK, response);

            return GenerateResponse(HttpStatusCode.BadRequest, response);
        }

        [HttpPost("{orderId}/item")]
        public IActionResult AddItem([FromRoute] string orderId, [FromBody] AddItemModel model)
        {
            if (Guid.TryParse(orderId, out var idGuid) == false)
                return GenerateResponse(HttpStatusCode.BadRequest, ErrorExtensions.CreateNotification("id", "Identificador inválido"));

            if (ModelState.IsValid == false)
                return GenerateBadRequestResponse();

            model.OrderId = idGuid;
            var response = _service.AddItem(model);

            if (response.Success)
                return GenerateResponse(HttpStatusCode.Created, response);

            return GenerateResponse(HttpStatusCode.BadRequest, response);
        }

        [HttpDelete("{orderId}/item/{id}")]
        public IActionResult RemoveItem([FromRoute] string orderId, [FromRoute] string id)
        {
            if (Guid.TryParse(orderId, out var orderIdGuid) == false | Guid.TryParse(id, out var idGuid) == false)
                return GenerateResponse(HttpStatusCode.BadRequest, ErrorExtensions.CreateNotification("id", "Identificador inválido"));

            var response = _service.RemoveItem(new RemoveItemModel(idGuid, orderIdGuid));

            if (response.Success)
                return GenerateResponse(HttpStatusCode.OK, response);

            return GenerateResponse(HttpStatusCode.BadRequest, response);
        }
    }
}