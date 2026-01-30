using System;

// Demonstration du pattern de conception Strategy en C#

#region Modèles
public sealed record Donnees(string Entree);
public sealed record Resultat(string Sortie);
#endregion

// Contrat de la stratégie
#region Contrat (interface) Strategy
public interface IStrategie
{
    Resultat Executer(Donnees donnees);
}
#endregion

// 3 différentes implémentations de la stratégie
#region Stratégies
public sealed class Strategie1 : IStrategie
{
    public Resultat Executer(Donnees donnees)
        => new Resultat($"[Strategie1] Traitement de: {donnees.Entree}");
}

public sealed class Strategie2 : IStrategie
{
    public Resultat Executer(Donnees donnees)
        => new Resultat($"[Strategie2] Traitement de: {donnees.Entree}");
}

public sealed class Strategie3 : IStrategie
{
    public Resultat Executer(Donnees donnees)
        => new Resultat($"[Strategie3] Traitement de: {donnees.Entree}");
}
#endregion

// Contexte utilisant une stratégie
#region Contexte
public sealed class GestionnaireDeTache
{
    private IStrategie _strategie;

    public GestionnaireDeTache(IStrategie strategie)
        => _strategie = strategie ?? throw new ArgumentNullException(nameof(strategie)); // Stratégie ne peut pas être nulle

    public void DefinirStrategie(IStrategie strategie)
        => _strategie = strategie ?? throw new ArgumentNullException(nameof(strategie)); // idem

    public Resultat ExecuterTache(Donnees donnees)
    {
        if (donnees is null) throw new ArgumentNullException(nameof(donnees)); // idem avec les données
        return _strategie.Executer(donnees);
    }
}
#endregion

public static class Program
{
    public static void Main()
    {
        var donnees = new Donnees("Exemple de tâche");

        // Stratégie initiale (choisie au démarrage)
        var contexte = new GestionnaireDeTache(new Strategie1());
        Console.WriteLine(contexte.ExecuterTache(donnees).Sortie);

        // Changement de stratégie à l'exécution
        contexte.DefinirStrategie(new Strategie2());
        Console.WriteLine(contexte.ExecuterTache(donnees).Sortie);

        contexte.DefinirStrategie(new Strategie3());
        Console.WriteLine(contexte.ExecuterTache(donnees).Sortie);
    }
}