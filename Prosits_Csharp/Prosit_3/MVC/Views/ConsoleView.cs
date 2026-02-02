using PizzeriaMVC.Models;
using System;

namespace PizzeriaMVC.Views
{
    public class ConsoleView
    {
        public void ShowHeader() => Console.WriteLine("=== Pizzeria MVC - Exemple d'architecture ===\n");

        public void ShowMenu(Menu menu)
        {
            Console.WriteLine("Menu :");
            foreach (var p in menu.Pizzas)
            {
                Console.WriteLine(p.ToString());
            }
            Console.WriteLine();
        }

        public void ShowOrder(Order order)
        {
            Console.WriteLine("Commande actuelle :");
            Console.WriteLine(order.ToString());
            Console.WriteLine();
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine();
        }

        public string Prompt(string prompt)
        {
            Console.Write(prompt + " ");
            return Console.ReadLine() ?? string.Empty;
        }
    }
}
