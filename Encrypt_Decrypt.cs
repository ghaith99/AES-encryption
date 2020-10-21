using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class Program
{
	public static void Main()
	{
		byte[] secret = Encrypt("Hello - CMMI5");
		Console.WriteLine(Convert.ToBase64String(secret,0,secret.Length));
		Console.WriteLine(Decrypt(Convert.ToBase64String (secret, 0,secret.Length)));
	}

	public static byte[] Encrypt(string clearText)
	{
		byte[] bytes = Encoding.Unicode.GetBytes(clearText);
		Aes aes = Aes.Create();
		byte[] result;

        Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes("SecretPass", new byte[]
        {
            53,102,17,120,42,37,121,133,138,121,110,106,168
        });
        aes.Key = rfc2898DeriveBytes.GetBytes(32);
        aes.IV = rfc2898DeriveBytes.GetBytes(16);
        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
    
        cryptoStream.Write(bytes, 0, bytes.Length);
        cryptoStream.Close();
        result = memoryStream.ToArray();
			
		return result;
	
	}
	

    public static string Decrypt(string cipherText){

        cipherText = cipherText.Replace(" ", "+");
        byte[] array = Convert.FromBase64String(cipherText);

        Aes aes = Aes.Create();
    
        Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes("SecretPass", new byte[]
        {
            53,102,17,120,42,37,121,133,138,121,110,106,168
        });
        aes.Key = rfc2898DeriveBytes.GetBytes(32);
        aes.IV = rfc2898DeriveBytes.GetBytes(16);
        MemoryStream memoryStream = new MemoryStream();
        
        CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write);
        cryptoStream.Write(array, 0, array.Length);
        cryptoStream.Close();

        cipherText = Encoding.Unicode.GetString(memoryStream.ToArray());
        return cipherText;
    }
}