using PizzeriaMVC.Models;
using System;

namespace PizzeriaMVC.Views
{
    public class ConsoleView
    {
        public void Clear() => Console.Clear();

        public void ShowHeader()
        {
            Console.WriteLine("=========================================");
            Console.WriteLine("  Pizzeria MVC — Exemple d'architecture");
            Console.WriteLine("=========================================");
            Console.WriteLine();
        }

        public void ShowMenu(Menu menu)
        {
            Console.WriteLine("Menu :");
            foreach (var p in menu.Pizzas)
            {
                Console.WriteLine(p.ToString());
            }
            Console.WriteLine();
        }

        public void ShowCommands()
        {
            Console.WriteLine("Commandes :");
            Console.WriteLine(" [id]   - Ajouter la pizza par son ID (ex: 1)");
            Console.WriteLine(" v      - Voir la commande actuelle");
            Console.WriteLine(" r      - Retirer une pizza par ID de la commande");
            Console.WriteLine(" c      - Valider (payer) la commande");
            Console.WriteLine(" h      - Afficher l'aide (cette liste)");
            Console.WriteLine(" q      - Quitter");
            Console.WriteLine();
        }

        public void ShowOrder(Order order)
        {
            Console.WriteLine("--- Commande actuelle ---");
            Console.WriteLine(order.ToString());
            Console.WriteLine("-------------------------");
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
