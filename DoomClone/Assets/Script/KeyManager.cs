using System;
using System.Security.Cryptography;

public class KeyManager
{
    // Genereer een willekeurige sleutel met de opgegeven lengte (in bytes)
    public static string GenerateRandomKey(int keyLength)
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            byte[] keyBytes = new byte[keyLength];
            rng.GetBytes(keyBytes);
            return BitConverter.ToString(keyBytes).Replace("-", "").ToLower();
        }
    }

    // Voeg hier meer functies toe voor ander sleutelbeheer, zoals opslaan, laden, vernieuwen, etc.
}

class Program
{
    static void Main()
    {
        // Voorbeeld: Genereer een willekeurige sleutel van 16 bytes (128 bits)
        string randomKey = KeyManager.GenerateRandomKey(16);

        Console.WriteLine("Willekeurige sleutel: " + randomKey);
    }
}
