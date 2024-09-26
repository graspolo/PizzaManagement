using PizzaManagement.Models;
using Request = PizzaManagement.Directory.OrderDirectory.Request;

namespace PizzaManagement.Services
{
    public interface IOrderService
    {
        IEnumerable<Pizza> GetMenu();
        Order CreateOrder(Request.Order order);
        Order GetNextOrder();
        bool ValidateOrder(Request.Order order);
    }
}
