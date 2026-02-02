using System.Collections.Generic;
using System.Linq;

namespace PizzeriaMVC.Models
{
    public class Menu
    {
        private readonly List<Pizza> _pizzas;

        public Menu(IEnumerable<Pizza> pizzas)
        {
            _pizzas = pizzas?.ToList() ?? new List<Pizza>();
        }

        public IReadOnlyList<Pizza> Pizzas => _pizzas;

        public Pizza? FindById(int id) => _pizzas.FirstOrDefault(p => p.Id == id);
    }
}
