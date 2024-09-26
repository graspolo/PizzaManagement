using Microsoft.AspNetCore.Mvc;
using Moq;
using PizzaManagement.Controllers;
using PizzaManagement.Services;
using Microsoft.Extensions.Logging;
using Request = PizzaManagement.Directory.OrderDirectory.Request;
using PizzaManagement.Models;


namespace PizzaManagement.Test.Controllers
{
    public class OrderControllerTests
    {
        private readonly Mock<IOrderService> _orderServiceMock;
        private readonly OrderController _orderController;

        public OrderControllerTests()
        {
            _orderServiceMock = new Mock<IOrderService>();
            _orderController = new OrderController(_orderServiceMock.Object, Mock.Of<ILogger<OrderController>>());
        }

        [Fact]
        public void SendOrder_InvalidOrder_ReturnsBadRequest()
        {
            // Setup
            var invalidOrder = new Request.Order { PizzaIds = null };
            _orderServiceMock.Setup(service => service.ValidateOrder(invalidOrder)).Returns(false);

            // Act
            var result = _orderController.SendOrder(invalidOrder);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void SendOrder_EmptyOrder_ReturnsBadRequest()
        {
            // Setup
            var emptyOrder = new Request.Order { PizzaIds = new List<int>() };
            _orderServiceMock.Setup(service => service.ValidateOrder(emptyOrder)).Returns(false);

            // Act
            var result = _orderController.SendOrder(emptyOrder);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void SendOrder_OrderWithNonExistentPizza_ReturnsBadRequest()
        {
            // Setup
            var orderWithNonExistentPizza = new Request.Order { PizzaIds = new List<int> { 999 } };
            _orderServiceMock.Setup(service => service.ValidateOrder(orderWithNonExistentPizza)).Returns(false);

            // Act
            var result = _orderController.SendOrder(orderWithNonExistentPizza);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void SendOrder_ValidOrder_ReturnsOk()
        {
            // Setup
            var validOrder = new Request.Order { PizzaIds = new List<int> { 1, 2 } };
            _orderServiceMock.Setup(service => service.ValidateOrder(validOrder)).Returns(true);
            _orderServiceMock.Setup(service => service.CreateOrder(validOrder)).Returns(new Order());

            // Act
            var result = _orderController.SendOrder(validOrder);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetNextOrder_NoOrdersInQueue_ReturnsNotFound()
        {
            // Setup
            _orderServiceMock.Setup(service => service.GetNextOrder()).Returns((Order)null);

            // Act
            var result = _orderController.GetNextOrder();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void GetNextOrder_OrderInQueue_ReturnsOk()
        {
            // Setup
            var order = new Order { Id = 1, Pizzas = new List<Pizza>(), OrderDate = DateTime.Now, TotalPrice = 20.0 };
            _orderServiceMock.Setup(service => service.GetNextOrder()).Returns(order);

            // Act
            var result = _orderController.GetNextOrder();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }
    }

}
