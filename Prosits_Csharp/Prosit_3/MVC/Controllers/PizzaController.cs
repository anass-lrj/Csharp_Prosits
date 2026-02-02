using PizzeriaMVC.Models;
using PizzeriaMVC.Views;

namespace PizzeriaMVC.Controllers
{
    public class PizzaController
    {
        private readonly Menu _menu;
        private readonly ConsoleView _view;

        public PizzaController(Menu menu, ConsoleView view)
        {
            _menu = menu;
            _view = view;
        }

        public void ShowMenu() => _view.ShowMenu(_menu);

        public Pizza? GetPizzaById(int id) => _menu.FindById(id);
    }
}
