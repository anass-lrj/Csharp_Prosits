using PizzeriaMVC.Controllers;
using PizzeriaMVC.Models;
using PizzeriaMVC.Views;

var menu = new Menu(new[] {
	new Pizza(1, "Margherita", 6.50m),
	new Pizza(2, "Napolitaine", 7.50m),
	new Pizza(3, "Reine", 8.50m),
	new Pizza(4, "4 Fromages", 9.00m),
	new Pizza(5, "Pepperoni", 9.50m)
});

var view = new ConsoleView();
var pizzaController = new PizzaController(menu, view);
var orderController = new OrderController(view);
var app = new AppController(pizzaController, orderController, view);

app.Run();
