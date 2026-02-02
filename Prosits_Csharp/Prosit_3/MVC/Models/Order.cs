using System.Collections.Generic;
using System.Linq;

namespace PizzeriaMVC.Models
{
    public class Order
    {
        private readonly List<Pizza> _items = new();

        public IReadOnlyList<Pizza> Items => _items;

        public void AddPizza(Pizza pizza)
        {
            if (pizza != null) _items.Add(pizza);
        }

        public decimal Total() => _items.Sum(p => p.Price);

        public void Clear() => _items.Clear();

        public bool RemoveById(int id)
        {
            var pizza = _items.FirstOrDefault(p => p.Id == id);
            if (pizza == null) return false;
            _items.Remove(pizza);
            return true;
        }

        public override string ToString()
        {
            if (!_items.Any()) return "Commande vide.";
            return string.Join("\n", _items.Select(p => p.ToString())) + $"\nTotal: {Total():C}";
        }
    }
}
