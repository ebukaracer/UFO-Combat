using UnityEngine;

namespace Racer.SaveManager
{
    public class CcEncrypter : MonoBehaviour
    {
        // Maintain few chars for faster iteration.
        private static char[] AvailableChars { get; } =
        {
            'a', 'b', 'c', 'd', 'e', 'F', 'G', 'H',
            'I', 'J', '0', '9', '8', '7', '6', '5',
            '4', '3', '2', '1', '?', '#', '$', '%',
        };

        public static string Encrypt(string text, int offset)
        {
            var plain = text.ToCharArray();

            for (var i = 0; i < plain.Length; i++)
            {
                for (var j = 0; j < AvailableChars.Length; j++)
                {
                    if (plain[i] != AvailableChars[j]) continue;

                    plain[i] = AvailableChars[(j + offset) % AvailableChars.Length];

                    break;
                }
            }
            return new string(plain);
        }

        public static string Decrypt(string text, int offset)
        {
            var plain = text.ToCharArray();

            for (var i = 0; i < plain.Length; i++)
            {
                for (var j = 0; j < AvailableChars.Length; j++)
                {
                    if (plain[i] != AvailableChars[j]) continue;

                    plain[i] = AvailableChars[(j + (AvailableChars.Length - offset)) % AvailableChars.Length];

                    break;
                }
            }
            return new string(plain);
        }

        // Assuming we're dealing with just integers.
        public static string SaveAndEncrypt(int saveValue, int offset)
        {
            return Encrypt(saveValue.ToString(), offset);
        }

        public static int DecryptAndGet(string encryptId, int saveValue, int offset)
        {
            var decryptValue = Decrypt(SaveManager.GetString(encryptId), offset);

            if (string.IsNullOrEmpty(decryptValue))
                return saveValue;

            if (int.TryParse(decryptValue, out var result))
                return saveValue == result ? result : 0;

            Logging.LogWarning("Type conversion failed!");
            return -1;
        }
    }
}
