using System;
using System.Collections.Generic;

public class KeyManager
{
    private Dictionary<string, bool> acquiredKeys;

    public KeyManager()
    {
        acquiredKeys = new Dictionary<string, bool>();
    }

    // Voeg een sleutel toe aan de verzameling
    public void AddKey(string keyName)
    {
        if (!acquiredKeys.ContainsKey(keyName))
        {
            acquiredKeys[keyName] = false;
            Console.WriteLine($"Je hebt de {keyName} gevonden!");
        }
        else
        {
            Console.WriteLine($"Je hebt al de {keyName}.");
        }
    }

    // Controleer of de speler de vereiste sleutel heeft om een deur te openen
    public bool HasKey(string keyName)
    {
        return acquiredKeys.ContainsKey(keyName) && acquiredKeys[keyName];
    }

    // Markeer een sleutel als verkregen
    public void AcquireKey(string keyName)
    {
        if (acquiredKeys.ContainsKey(keyName))
        {
            acquiredKeys[keyName] = true;
            Console.WriteLine($"Je hebt de {keyName} gebruikt om een deur te openen.");
        }
    }
}

class Program
{
    static void Main()
    {
        KeyManager keyManager = new KeyManager();

        // Simuleer het vinden en gebruiken van sleutels
        keyManager.AddKey("Rode sleutel");
        keyManager.AddKey("Blauwe sleutel");
        keyManager.AcquireKey("Rode sleutel");

        // Controleer of de speler de vereiste sleutel heeft om een deur te openen
        if (keyManager.HasKey("Rode sleutel"))
        {
            Console.WriteLine("De deur met de rode sleutel is geopend!");
        }
        else
        {
            Console.WriteLine("Je hebt de juiste sleutel niet.");
        }
    }
}
