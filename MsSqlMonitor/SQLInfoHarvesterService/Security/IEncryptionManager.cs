using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLInfoCollectorService.Security
{
    public interface IEncryptionManager
    {
        string Encrypt(out string key, string text);
        //void Encrypt(out string encryptedText, out string key, string text);
        string Decrypt(string key, string encryptedText);
        //void Decrypt(out string decryptedText, string key, string encryptedText);

    }
}
