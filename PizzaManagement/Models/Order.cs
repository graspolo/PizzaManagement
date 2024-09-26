using PizzaManagement.Models;
using System.ComponentModel.DataAnnotations;

public class Order
{
    public int Id { get; set; }

    [Required]
    public List<Pizza> Pizzas { get; set; }

    public DateTime OrderDate { get; set; }

    public double TotalPrice { get; set; }
}
