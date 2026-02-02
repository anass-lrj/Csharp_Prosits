using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosits_Csharp.Prosit_2.Design_Pattern.Factory
{
    public interface ITransport
    {
        void Livrer();
    }

    public class Camion : ITransport
    {
        public void Livrer()
        {
            Console.WriteLine("Livraison par la route en camion");
        }
    }

    public class Bateau : ITransport
    {
        public void Livrer()
        {
            Console.WriteLine("Livraison par la mer en bateau");
        }
    }

    public class Avion : ITransport
    {
        public void Livrer()
        {
            Console.WriteLine("Livraison par les airs en avion");
        }
    }

    // la classe de base qui contient la logique métier mais qui va deleguer la creation.

    public abstract class Logistique
    {
        // LA FACTORY METHOD : Elle est abstraite, donc pas de "new"
        public abstract ITransport CreerTransport();

        // Cette méthode utilise le produit sans savoir lequel c'est 
        public void PlanifierLivraison()
        {
            Console.WriteLine("Préparation de la commande");

            // Appel de la Factory Method pour obtenir l'objet
            ITransport transport = CreerTransport();

            // Utilisation de l'objet (Polymorphisme)
            transport.Livrer();

            Console.WriteLine("Logistique : Commande terminée.\n");
        }
    }



    public class LogistiqueRoutiere : Logistique
    {
        public override ITransport CreerTransport()
        {
            return new Camion();
        }
    }

    public class LogistiqueMaritime : Logistique
    {
        public override ITransport CreerTransport()
        {
            return new Bateau();
        }
    }

    public class LogistiqueAerienne : Logistique
    {
        public override ITransport CreerTransport()
        {
            return new Avion();
        }
    }
}
