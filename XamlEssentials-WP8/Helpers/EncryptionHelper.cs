using System;
using System.IO;
using System.Text;

#if WINRT
using Windows.Storage.Streams;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
#else
using System.Security.Cryptography;
#endif

namespace XamlEssentials.Helpers
{

    /// <summary>
    /// Helper functions for cross-platform data encryption.
    /// </summary>
    /// <remarks>Generously donated to the cause by Ginny Caughey and Jan Hannemann 
    /// from this blog post: http://janhannemann.wordpress.com/2013/11/10/simple-encryption-for-windows-winrt-and-windows-phone/. </remarks>
    public static class EncryptionHelper
    {

#if WINRT
        /// <summary>
        /// Encrypts a string with a given password and salt.
        /// </summary>
        /// <param name="plainText">The information to encrypt.</param>
        /// <param name="password">The password used to derive the key.</param>
        /// <param name="salt">An additional input </param>
        /// <returns>A byte array with the encrypted version of the plainText.</returns>
        public static byte[] Encrypt(string plainText, string password, string salt)
        {
            IBuffer pwBuffer = CryptographicBuffer.ConvertStringToBinary(password, BinaryStringEncoding.Utf8);
            IBuffer saltBuffer = CryptographicBuffer.ConvertStringToBinary(salt, BinaryStringEncoding.Utf16LE);
            IBuffer plainBuffer = CryptographicBuffer.ConvertStringToBinary(plainText, BinaryStringEncoding.Utf16LE);

            // Derive key material for password size 32 bytes for AES256 algorithm
            KeyDerivationAlgorithmProvider keyDerivationProvider = KeyDerivationAlgorithmProvider.OpenAlgorithm("PBKDF2_SHA1");
            // using salt and 1000 iterations
            KeyDerivationParameters pbkdf2Parms = KeyDerivationParameters.BuildForPbkdf2(saltBuffer, 1000);

            // create a key based on original key and derivation parmaters
            CryptographicKey keyOriginal = keyDerivationProvider.CreateKey(pwBuffer);
            IBuffer keyMaterial = CryptographicEngine.DeriveKeyMaterial(keyOriginal, pbkdf2Parms, 32);
            CryptographicKey derivedPwKey = keyDerivationProvider.CreateKey(pwBuffer);

            // derive buffer to be used for encryption salt from derived password key 
            IBuffer saltMaterial = CryptographicEngine.DeriveKeyMaterial(derivedPwKey, pbkdf2Parms, 16);

            // display the buffers – because KeyDerivationProvider always gets cleared after each use, they are very similar unforunately
            string keyMaterialString = CryptographicBuffer.EncodeToBase64String(keyMaterial);
            string saltMaterialString = CryptographicBuffer.EncodeToBase64String(saltMaterial);

            SymmetricKeyAlgorithmProvider symProvider = SymmetricKeyAlgorithmProvider.OpenAlgorithm("AES_CBC_PKCS7");
            // create symmetric key from derived password key
            CryptographicKey symmKey = symProvider.CreateSymmetricKey(keyMaterial);

            // encrypt data buffer using symmetric key and derived salt material
            IBuffer resultBuffer = CryptographicEngine.Encrypt(symmKey, plainBuffer, saltMaterial);
            byte[] result;
            CryptographicBuffer.CopyToByteArray(resultBuffer, out result);

            return result;
        }

        /// <summary>
        /// Decripts a byte array with a given password and salt.
        /// </summary>
        /// <param name="encryptedData">A byte array containing the data you would like decrypted.</param>
        /// <param name="password">The password used to derive the key.</param>
        /// <param name="salt">The key salt used to derive the key.</param>
        /// <returns>A string containing the decrypted data.</returns>
        public static string Decrypt(byte[] encryptedData, string password, string salt)
        {
            IBuffer pwBuffer = CryptographicBuffer.ConvertStringToBinary(password, BinaryStringEncoding.Utf8);
            IBuffer saltBuffer = CryptographicBuffer.ConvertStringToBinary(salt, BinaryStringEncoding.Utf16LE);
            IBuffer cipherBuffer = CryptographicBuffer.CreateFromByteArray(encryptedData);

            // Derive key material for password size 32 bytes for AES256 algorithm
            KeyDerivationAlgorithmProvider keyDerivationProvider = KeyDerivationAlgorithmProvider.OpenAlgorithm("PBKDF2_SHA1");
            // using salt and 1000 iterations
            KeyDerivationParameters pbkdf2Parms = KeyDerivationParameters.BuildForPbkdf2(saltBuffer, 1000);

            // create a key based on original key and derivation parmaters
            CryptographicKey keyOriginal = keyDerivationProvider.CreateKey(pwBuffer);
            IBuffer keyMaterial = CryptographicEngine.DeriveKeyMaterial(keyOriginal, pbkdf2Parms, 32);
            CryptographicKey derivedPwKey = keyDerivationProvider.CreateKey(pwBuffer);

            // derive buffer to be used for encryption salt from derived password key 
            IBuffer saltMaterial = CryptographicEngine.DeriveKeyMaterial(derivedPwKey, pbkdf2Parms, 16);

            // display the keys – because KeyDerivationProvider always gets cleared after each use, they are very similar unforunately
            string keyMaterialString = CryptographicBuffer.EncodeToBase64String(keyMaterial);
            string saltMaterialString = CryptographicBuffer.EncodeToBase64String(saltMaterial);

            SymmetricKeyAlgorithmProvider symProvider = SymmetricKeyAlgorithmProvider.OpenAlgorithm("AES_CBC_PKCS7");
            // create symmetric key from derived password material
            CryptographicKey symmKey = symProvider.CreateSymmetricKey(keyMaterial);

            // encrypt data buffer using symmetric key and derived salt material
            IBuffer resultBuffer = CryptographicEngine.Decrypt(symmKey, cipherBuffer, saltMaterial);
            string result = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf16LE, resultBuffer);
            return result;
        }

#else

        /// <summary>
        /// Encrypts a string with a given password and salt.
        /// </summary>
        /// <param name="plainText">The information to encrypt.</param>
        /// <param name="password">The password used to derive the key.</param>
        /// <param name="salt">The key salt used to derive the key.</param>
        /// <exception cref="System.ArgumentException">
        /// The specified salt size is smaller than 8 bytes or the iteration count is less than 1.
        /// </exception>
        /// <returns>A byte array with the encrypted version of the plainText.</returns>
        public static byte[] Encrypt(string plainText, string password, string salt)
        {
            AesManaged aes = null;
            MemoryStream memoryStream = null;
            CryptoStream cryptoStream = null;

            try
            {
                //Generate a Key based on a Password, Salt and HMACSHA1 pseudo-random number generator 
                var rfc2898 = new Rfc2898DeriveBytes(password, Encoding.Unicode.GetBytes(salt));

                //Create AES algorithm with 256 bit key and 128-bit block size 
                aes = new AesManaged();
                aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
                rfc2898.Reset(); //needed for WinRT compatibility
                aes.IV = rfc2898.GetBytes(aes.BlockSize / 8);

                //Create Memory and Crypto Streams 
                memoryStream = new MemoryStream();
                cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

                //Encrypt Data 
                var data = Encoding.Unicode.GetBytes(plainText);
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();

                //Return encrypted data 
                return memoryStream.ToArray();

            }
            catch (Exception eEncrypt)
            {
                return null;
            }
            finally
            {
                if (cryptoStream != null)
                    cryptoStream.Close();

                if (memoryStream != null)
                    memoryStream.Close();

                if (aes != null)
                    aes.Clear();

            }
        }

        /// <summary>
        /// Decripts a byte array with a given password and salt.
        /// </summary>
        /// <param name="encryptedData">A byte array containing the data you would like decrypted.</param>
        /// <param name="password">The password used to derive the key.</param>
        /// <param name="salt">The key salt used to derive the key.</param>
        /// <returns>A string containing the decrypted data.</returns>
        public static string Decrypt(byte[] encryptedData, string password, string salt)
        {
            AesManaged aes = null;
            MemoryStream memoryStream = null;
            CryptoStream cryptoStream = null;
            string decryptedText = "";
            try
            {
                //Generate a Key based on a Password, Salt and HMACSHA1 pseudo-random number generator 
                var rfc2898 = new Rfc2898DeriveBytes(password, Encoding.Unicode.GetBytes(salt));

                //Create AES algorithm with 256 bit key and 128-bit block size 
                aes = new AesManaged();
                aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
                rfc2898.Reset(); //neede to be WinRT compatible
                aes.IV = rfc2898.GetBytes(aes.BlockSize / 8);

                //Create Memory and Crypto Streams 
                memoryStream = new MemoryStream();
                cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write);

                //Decrypt Data 
                cryptoStream.Write(encryptedData, 0, encryptedData.Length);
                cryptoStream.FlushFinalBlock();

                //Return Decrypted String 
                var decryptBytes = memoryStream.ToArray();
                decryptedText = Encoding.Unicode.GetString(decryptBytes, 0, decryptBytes.Length);
            }
            catch (Exception eDecrypt)
            {

            }
            finally
            {
                if (cryptoStream != null)
                    cryptoStream.Close();

                if (memoryStream != null)
                    memoryStream.Close();

                if (aes != null)
                    aes.Clear();
            }
            return decryptedText;
        }
#endif

    }

}
