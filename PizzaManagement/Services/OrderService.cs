using PizzaManagement.Enums;
using PizzaManagement.Models;
using Request = PizzaManagement.Directory.OrderDirectory.Request;


namespace PizzaManagement.Services
{
    public class OrderService : IOrderService
    {
        // Menu statico delle pizze disponibili
        private static readonly List<Pizza> Menu = new List<Pizza>()
            {
                new Pizza { Id = 1, Name = "Margherita", Price = 5, Ingredients = new List<Ingredient> { Ingredient.TomatoSauce, Ingredient.Mozzarella } },
                new Pizza { Id = 2, Name = "Ortolana", Price = 6, Ingredients = new List<Ingredient> { Ingredient.TomatoSauce, Ingredient.Mozzarella, Ingredient.GrilledVegetables } },
                new Pizza { Id = 3, Name = "Diavola", Price = 6.5, Ingredients = new List<Ingredient> { Ingredient.TomatoSauce, Ingredient.Mozzarella, Ingredient.SpicyPepperoni } },
                new Pizza { Id = 4, Name = "Bufalina", Price = 7, Ingredients = new List<Ingredient> { Ingredient.TomatoSauce, Ingredient.BufalaMozzarella } }
            };

        // Coda degli ordini
        private static List<Order> OrderQueue = new List<Order>();

        // Restituisce il menu delle pizze
        public IEnumerable<Pizza> GetMenu()
        {
            return Menu;
        }

        // Valida un ordine verificando che le pizze ordinate esistano nel menu
        public bool ValidateOrder(Request.Order order)
        {
            if (order == null || order.PizzaIds == null || !order.PizzaIds.Any())
            {
                return false;
            }

            var invalidPizzaIds = order.PizzaIds.Where(id => !Menu.Any(m => m.Id == id)).ToList();
            return !invalidPizzaIds.Any();
        }

        // Crea un nuovo ordine e lo aggiunge alla coda
        public Order CreateOrder(Request.Order order)
        {
            var orderedPizzas = order.PizzaIds
                .GroupBy(id => id)
                .SelectMany(group => Menu.Where(p => p.Id == group.Key).Select(p => new { Pizza = p, Quantity = group.Count() }))
                .SelectMany(p => Enumerable.Repeat(p.Pizza, p.Quantity))
                .ToList();

            var newOrder = new Order
            {
                Id = OrderQueue.Count > 0 ? OrderQueue.Max(o => o.Id) + 1 : 1,
                Pizzas = orderedPizzas,
                OrderDate = DateTime.Now,
                TotalPrice = orderedPizzas.Sum(p => p.Price)
            };

            OrderQueue.Add(newOrder);
            return newOrder;
        }

        // Restituisce il prossimo ordine nella coda
        public Order GetNextOrder()
        {
            if (!OrderQueue.Any())
            {
                return null;
            }

            var nextOrder = OrderQueue.OrderBy(o => o.OrderDate).First();
            OrderQueue.Remove(nextOrder);
            return nextOrder;
        }
    }

}
