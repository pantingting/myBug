using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CHEER.Common
{
    public class Encryption
    {
        private static string EncryptionKey = typeof(BinaryReader).ToString() + "-w9" +
            typeof(System.Xml.NameTable).ToString() + "sdf3f" + typeof(Random).ToString() + "jsow23j235ay2s" +
            typeof(Encryption).ToString() + "a2skwp230a" + typeof(System.Collections.Queue).ToString() + "黶dadjm" +
            typeof(NullReferenceException).ToString();

        public string DESEnCrypt(string ValueToEnCrypt)
        {
            DESCryptoServiceProvider DesProv = new DESCryptoServiceProvider();
            return EnCrypt(DesProv, ValueToEnCrypt);
        }

        public string DESDeCrypt(string ValueToDeCrypt)
        {
            DESCryptoServiceProvider DesProv = new DESCryptoServiceProvider();
            return DeCrypt(DesProv, ValueToDeCrypt);
        }

        private string EnCrypt(SymmetricAlgorithm Algorithm, string ValueToEnCrypt)
        {
            byte[] InputbyteArray = Encoding.UTF8.GetBytes(ValueToEnCrypt);
            byte[] Key = Encoding.UTF8.GetBytes(EncryptionKey);
            Algorithm.Key = (byte[])ReDim(Key, Algorithm.Key.Length);
            Algorithm.IV = (byte[])ReDim(Key, Algorithm.IV.Length);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, Algorithm.CreateEncryptor(Algorithm.Key, Algorithm.IV), CryptoStreamMode.Write))
                {
                    cs.Write(InputbyteArray, 0, InputbyteArray.Length);
                    cs.FlushFinalBlock();
                }
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < ms.ToArray().Length; i++)
                {
                    byte Actualbyte = ms.ToArray()[i];
                    stringBuilder.AppendFormat("{0:X2}", Actualbyte);
                }
                return stringBuilder.ToString();
            }
        }

        private string DeCrypt(SymmetricAlgorithm Algorithm, string ValueToDeCrypt)
        {
            byte[] InputbyteArray = new byte[ValueToDeCrypt.Length / 2];
            for (int i = 0; i < ValueToDeCrypt.Length / 2; i++)
            {
                int Value = (Convert.ToInt32(ValueToDeCrypt.Substring(i * 2, 2), 16));
                InputbyteArray[i] = (byte)Value;
            }
            byte[] Key = Encoding.UTF8.GetBytes(EncryptionKey);
            Algorithm.Key = (byte[])ReDim(Key, Algorithm.Key.Length);
            Algorithm.IV = (byte[])ReDim(Key, Algorithm.IV.Length);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, Algorithm.CreateDecryptor(Algorithm.Key, Algorithm.IV), CryptoStreamMode.Write))
                {
                    cs.Write(InputbyteArray, 0, InputbyteArray.Length);
                    cs.FlushFinalBlock();
                }
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        private Array ReDim(Array OriginalArray, int NewSize)
        {
            Type ArrayElementsType = OriginalArray.GetType().GetElementType();
            Array newArray = Array.CreateInstance(ArrayElementsType, NewSize);
            Array.Copy(OriginalArray, 0, newArray, 0, Math.Min(OriginalArray.Length, NewSize));
            return newArray;
        }
    }
}
