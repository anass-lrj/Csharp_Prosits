using PizzeriaMVC.Models;
using PizzeriaMVC.Views;
using System.Linq;

namespace PizzeriaMVC.Controllers
{
    public class OrderController
    {
        private readonly Order _order = new();
        private readonly ConsoleView _view;

        public OrderController(ConsoleView view)
        {
            _view = view;
        }

        public void AddPizza(Pizza? pizza)
        {
            if (pizza == null)
            {
                _view.ShowMessage("Pizza introuvable.");
                return;
            }
            _order.AddPizza(pizza);
            _view.ShowMessage($"Ajouté : {pizza.Name} - {pizza.Price:C}");
        }

        public void ShowCurrentOrder() => _view.ShowOrder(_order);

        public void Checkout()
        {
            if (!_order.Items.Any())
            {
                _view.ShowMessage("Aucune pizza dans la commande.");
                return;
            }
            _view.ShowMessage("Validation de la commande :\n" + _order.ToString());
            _order.Clear();
        }
    }
}
