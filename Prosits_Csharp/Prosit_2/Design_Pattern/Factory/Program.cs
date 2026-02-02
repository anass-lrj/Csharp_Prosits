using System;




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
            Console.WriteLine("Livraison par la airs en avion");
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



// Version HARD CODER 
namespace Prosits_Csharp.Prosit_2.Design_Pattern.Factory
{

    public interface ITransport { void Livrer(); }

    public class Camion : ITransport
    {
        public void Livrer() => Console.WriteLine("Livraison par la route en camion");


        public class Bateau : ITransport
        {
            public void Livrer() => Console.WriteLine("Livraison par la mer en bateau");

        }

        // Si on veut ajouter l'Avion, il faut modifier le code ici...
        public class Avion : ITransport
        {
            public void Livrer() => Console.WriteLine("Livraison par la air en avion");

        }


        public class LogistiqueCentralisee
        {
            // On est obligé de passer un paramètre pour choisir
            public void PlanifierLivraison(string typeTransport)
            {
                ITransport transport = null;


                // On utilise "new" directement. La classe est fusionné avec Camion, Bateau et Avion.
                switch (typeTransport)
                {
                    case "route":
                        transport = new Camion();
                        break;
                    case "mer":
                        transport = new Bateau();
                        break;
                    case "air":
                        transport = new Avion();
                        break;
                    default:
                        throw new Exception("pas de moyen trv");
                }

                Console.WriteLine("Preparation");
                transport.Livrer();
            }
        }

        class Program
        {   
            static void Main(string[] args)
            {

                Logistique logistiqueA = new LogistiqueRoutiere();
                logistiqueA.PlanifierLivraison();

                Logistique logistiqueB = new LogistiqueMaritime();
                logistiqueB.PlanifierLivraison();

                Logistique logistiqueC = new LogistiqueAerienne();
                logistiqueC.PlanifierLivraison();

                // CQFR
                // La méthode PlanifierLivraison est la même pour tous LES TRANSPORT,
                // mais le résultat (l'objet créé) change selon la classe instanciée.

                Console.WriteLine("-------------------------");
                LogistiqueCentralisee logistique = new LogistiqueCentralisee();

                logistique.PlanifierLivraison("route");
                logistique.PlanifierLivraison("mer");
                logistique.PlanifierLivraison("air");
            }
        }
    }
}