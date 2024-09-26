using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaManagement.Services;
using Request = PizzaManagement.Directory.OrderDirectory.Request;

namespace PizzaManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet("GetMenu", Name = "GetMenu")]
        public ActionResult Get()
        {
            _logger.LogInformation("Fetching menu.");
            var menu = _orderService.GetMenu();
            return Ok(menu);
        }

        [HttpPost("SendOrder", Name = "SendOrder")]
        public ActionResult SendOrder([FromBody] Request.Order order)
        {
            _logger.LogInformation("Received order: {@Order}", order);
            if (!_orderService.ValidateOrder(order))
            {
                _logger.LogWarning("Invalid order received: {@Order}", order);
                return BadRequest("Ordine non valido.");
            }

            var newOrder = _orderService.CreateOrder(order);
            _logger.LogInformation("Order created successfully: {@NewOrder}", newOrder);
            return Ok(new { newOrder.Id, newOrder.TotalPrice });
        }

        [HttpGet("GetNextOrder", Name = "GetNextOrder")]
        public ActionResult GetNextOrder()
        {
            _logger.LogInformation("Fetching next order.");
            var nextOrder = _orderService.GetNextOrder();
            if (nextOrder == null)
            {
                _logger.LogInformation("No orders in the queue.");
                return NotFound("Nessun ordine nella coda.");
            }

            _logger.LogInformation("Next order fetched: {@NextOrder}", nextOrder);
            return Ok(nextOrder);
        }
    }

}
