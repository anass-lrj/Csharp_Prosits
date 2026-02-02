using PizzeriaMVC.Views;

namespace PizzeriaMVC.Controllers
{
    public class AppController
    {
        private readonly PizzaController _pizzaController;
        private readonly OrderController _orderController;
        private readonly ConsoleView _view;

        public AppController(PizzaController pizzaController, OrderController orderController, ConsoleView view)
        {
            _pizzaController = pizzaController;
            _orderController = orderController;
            _view = view;
        }

        public void Run()
        {
            _view.ShowHeader();

            while (true)
            {
                _pizzaController.ShowMenu();
                _view.ShowMessage("Entrez l'ID de la pizza à ajouter, 'v' pour voir la commande, 'c' pour valider, 'q' pour quitter.");
                var input = _view.Prompt(">");

                if (string.IsNullOrWhiteSpace(input)) continue;
                input = input.Trim().ToLowerInvariant();

                if (input == "q") break;
                if (input == "v") { _orderController.ShowCurrentOrder(); continue; }
                if (input == "c") { _orderController.Checkout(); continue; }

                if (int.TryParse(input, out var id))
                {
                    var pizza = _pizzaController.GetPizzaById(id);
                    _orderController.AddPizza(pizza);
                    continue;
                }

                _view.ShowMessage("Commande non reconnue. Entrez un ID, 'v', 'c' ou 'q'.");
            }

            _view.ShowMessage("Au revoir !");
        }
    }
}
