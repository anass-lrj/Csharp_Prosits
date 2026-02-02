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
            while (true)
            {
                _view.Clear();
                _view.ShowHeader();
                _pizzaController.ShowMenu();
                _view.ShowCommands();

                var input = _view.Prompt(">");
                if (string.IsNullOrWhiteSpace(input)) continue;
                input = input.Trim().ToLowerInvariant();

                if (input == "q") break;
                if (input == "h") { _view.ShowCommands(); _view.Prompt("Appuyez sur Entrée pour continuer..."); continue; }
                if (input == "v") { _orderController.ShowCurrentOrder(); _view.Prompt("Appuyez sur Entrée pour continuer..."); continue; }
                if (input == "c") { _orderController.Checkout(); _view.Prompt("Appuyez sur Entrée pour continuer..."); continue; }
                if (input == "r")
                {
                    var idStr = _view.Prompt("Entrez l'ID de la pizza à retirer:");
                    if (int.TryParse(idStr, out var removeId)) _orderController.RemovePizzaById(removeId);
                    else _view.ShowMessage("ID invalide.");
                    _view.Prompt("Appuyez sur Entrée pour continuer...");
                    continue;
                }

                // tenter d'ajouter par simple ID
                if (int.TryParse(input, out var id))
                {
                    var pizza = _pizzaController.GetPizzaById(id);
                    _orderController.AddPizza(pizza);
                    _view.Prompt("Appuyez sur Entrée pour continuer...");
                    continue;
                }

                _view.ShowMessage("Commande non reconnue. Tapez 'h' pour l'aide.");
                _view.Prompt("Appuyez sur Entrée pour continuer...");
            }

            _view.ShowMessage("Au revoir !");
        }
    }
}
