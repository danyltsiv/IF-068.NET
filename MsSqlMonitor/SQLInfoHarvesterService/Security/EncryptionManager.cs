using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;

namespace SQLInfoCollectorService.Security
{
    public class EncryptionManager: IEncryptionManager
    {
        private IEncryption encryptor;
        private ISLogger logger;
        private string encryptionPassword = "#bw34kdds0%bL";
        public EncryptionManager (IEncryption encryptor, ISLogger logger)
        {
            this.encryptor = encryptor;
            this.logger = logger;
        }

        public string Encrypt(out string key, string text)
        {
            key = encryptor.GetRandomKey();
            return encryptor.Encrypt(text, key, encryptionPassword);
        }   

        public string Decrypt(string key, string encryptedText)
        {
            if (key != null)
            {
                //logger.Debug(($"SecurityManager Decrypt({encryptedText}, {key})"));
                return encryptor.Decrypt(encryptedText, key, encryptionPassword);
            }
            else
            {
                logger.Error(($"SecurityManager Decrypt({encryptedText}, key==null)"));
                return encryptedText;
            }

            
            
        }
    }
}
