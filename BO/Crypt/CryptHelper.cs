using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Reflection;

namespace BO
{
    public abstract class CryptHelper
    {
        public static CryptHelper Helper;

        public static event EventHandler KeyChanged;
        private static string _xKey = "";

        protected static string XKey
        {
            get { return CryptHelper._xKey; }
            set
            {
                if (_xKey != value)
                {
                    CryptHelper._xKey = value;
                    if (KeyChanged != null)
                        KeyChanged(Helper, EventArgs.Empty);
                }
            }
        }
        protected static bool keyIsValid = false;
        public const decimal HiddenValue = 999999.00M;
        public static bool KeyIsValid()
        {
            return keyIsValid;
        }
        public abstract void SetKey(string key);
        //{
        //    throw new Exception("Нужно переопределить метод SetKey в наследнике.");
        //}
        public abstract void ChangeKey(string oldKey, string newKey);
        //{
        //    throw new Exception("Нужно переопределить метод ChangeKey в наследнике.");
        //}

        // Encrypt a byte array into a byte array using a key and an IV 
        public static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            // Create a MemoryStream to accept the encrypted bytes 
            MemoryStream ms = new MemoryStream();

            // Create a symmetric algorithm. 
            // We are going to use Rijndael because it is strong and
            // available on all platforms. 
            // You can use other algorithms, to do so substitute the
            // next line with something like 
            //      TripleDES alg = TripleDES.Create(); 
            Rijndael alg = Rijndael.Create();

            // Now set the key and the IV. 
            // We need the IV (Initialization Vector) because
            // the algorithm is operating in its default 
            // mode called CBC (Cipher Block Chaining).
            // The IV is XORed with the first block (8 byte) 
            // of the data before it is encrypted, and then each
            // encrypted block is XORed with the 
            // following block of plaintext.
            // This is done to make encryption more secure. 

            // There is also a mode called ECB which does not need an IV,
            // but it is much less secure. 
            alg.Key = Key;
            alg.IV = IV;

            // Create a CryptoStream through which we are going to be
            // pumping our data. 
            // CryptoStreamMode.Write means that we are going to be
            // writing data to the stream and the output will be written
            // in the MemoryStream we have provided. 
            CryptoStream cs = new CryptoStream(ms,
                alg.CreateEncryptor(), CryptoStreamMode.Write);

            // Write the data and make it do the encryption 
            cs.Write(clearData, 0, clearData.Length);

            // Close the crypto stream (or do FlushFinalBlock). 
            // This will tell it that we have done our encryption and
            // there is no more data coming in, 
            // and it is now a good time to apply the padding and
            // finalize the encryption process. 
            cs.Close();

            // Now get the encrypted data from the MemoryStream.
            // Some people make a mistake of using GetBuffer() here,
            // which is not the right way. 
            byte[] encryptedData = ms.ToArray();

            return encryptedData;
        }

        // Encrypt a string into a string using a password 
        //    Uses Encrypt(byte[], byte[], byte[]) 

        public static string MD5Hash(string clearText)
        {
            return MD5Hash(System.Text.Encoding.Unicode.GetBytes(clearText));
        }

        public static string MD5Hash(byte[] clearBytes)
        {
            string ret = "";
            byte[] hashBytes = ((new MD5CryptoServiceProvider()).ComputeHash(clearBytes));

            foreach (byte b in hashBytes)
            {
                ret += b.ToString("x");
            }
            return ret;
        }

        public static Guid MD5HashGuid(byte[] clearBytes)
        {
            byte[] hashBytes = ((new MD5CryptoServiceProvider()).ComputeHash(clearBytes));
            return new Guid(hashBytes);
        }

        public static Guid MD5HashGuid(string clearText)
        {
            return MD5HashGuid(System.Text.Encoding.Unicode.GetBytes(clearText));
        }

        public static string Encrypt(string clearText, string Password)
        {
            // First we need to turn the input string into a byte array. 
            byte[] clearBytes =
                System.Text.Encoding.Unicode.GetBytes(clearText);

            // Then, we need to turn the password into Key and IV 
            // We are using salt to make it harder to guess our key
            // using a dictionary attack - 
            // trying to guess a password by enumerating all possible words. 
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
                               0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

            // Now get the key/IV and do the encryption using the
            // function that accepts byte arrays. 
            // Using PasswordDeriveBytes object we are first getting
            // 32 bytes for the Key 
            // (the default Rijndael key length is 256bit = 32bytes)
            // and then 16 bytes for the IV. 
            // IV should always be the block size, which is by default
            // 16 bytes (128 bit) for Rijndael. 
            // If you are using DES/TripleDES/RC2 the block size is
            // 8 bytes and so should be the IV size. 
            // You can also read KeySize/BlockSize properties off
            // the algorithm to find out the sizes. 
            byte[] encryptedData = Encrypt(clearBytes,
                pdb.GetBytes(32), pdb.GetBytes(16));

            // Now we need to turn the resulting byte array into a string. 
            // A common mistake would be to use an Encoding class for that.
            //It does not work because not all byte values can be
            // represented by characters. 
            // We are going to be using Base64 encoding that is designed
            //exactly for what we are trying to do. 

            return Convert.ToBase64String(encryptedData);

            //return System.Text.Encoding.ASCII.GetString(encryptedData); 

        }
        public static string Encrypt(decimal number)
        {
            return Encrypt(number.ToString());
        }
        public static string Encrypt(string clearText)
        {
            string ret = "";
            //if (string.IsNullOrEmpty(xKey))
            //    throw new Exception("Не задан ключ для зашифрования (CryptHelper.xKey)");

            try
            {
                ret = Encrypt(clearText, XKey);
            }
            catch (Exception Ex)
            {
                //xKey = "";
                string s = Ex.Message;
                //throw new Exception("Ошибка при зашифровании: " + Ex.Message);
            }
            return ret;
        }

        // Encrypt bytes into bytes using a password 
        //    Uses Encrypt(byte[], byte[], byte[]) 

        public static byte[] Encrypt(byte[] clearData, string Password)
        {
            // We need to turn the password into Key and IV. 
            // We are using salt to make it harder to guess our key
            // using a dictionary attack - 
            // trying to guess a password by enumerating all possible words. 
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
                               0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

            // Now get the key/IV and do the encryption using the function
            // that accepts byte arrays. 
            // Using PasswordDeriveBytes object we are first getting
            // 32 bytes for the Key 
            // (the default Rijndael key length is 256bit = 32bytes)
            // and then 16 bytes for the IV. 
            // IV should always be the block size, which is by default
            // 16 bytes (128 bit) for Rijndael. 
            // If you are using DES/TripleDES/RC2 the block size is 8
            // bytes and so should be the IV size. 
            // You can also read KeySize/BlockSize properties off the
            // algorithm to find out the sizes. 
            return Encrypt(clearData, pdb.GetBytes(32), pdb.GetBytes(16));

        }

        // Encrypt a file into another file using a password 
        public static void Encrypt(string fileIn,
            string fileOut, string Password)
        {

            // First we are going to open the file streams 
            FileStream fsIn = new FileStream(fileIn,
                FileMode.Open, FileAccess.Read);
            FileStream fsOut = new FileStream(fileOut,
                FileMode.OpenOrCreate, FileAccess.Write);

            // Then we are going to derive a Key and an IV from the
            // Password and create an algorithm 
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
                               0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

            Rijndael alg = Rijndael.Create();
            alg.Key = pdb.GetBytes(32);
            alg.IV = pdb.GetBytes(16);

            // Now create a crypto stream through which we are going
            // to be pumping data. 
            // Our fileOut is going to be receiving the encrypted bytes. 
            CryptoStream cs = new CryptoStream(fsOut,
                alg.CreateEncryptor(), CryptoStreamMode.Write);

            // Now will will initialize a buffer and will be processing
            // the input file in chunks. 
            // This is done to avoid reading the whole file (which can
            // be huge) into memory. 
            int bufferLen = 4096;
            byte[] buffer = new byte[bufferLen];
            int bytesRead;

            do
            {
                // read a chunk of data from the input file 
                bytesRead = fsIn.Read(buffer, 0, bufferLen);

                // encrypt it 
                cs.Write(buffer, 0, bytesRead);
            } while (bytesRead != 0);

            // close everything 

            // this will also close the unrelying fsOut stream
            cs.Close();
            fsIn.Close();
        }

        // Decrypt a byte array into a byte array using a key and an IV 
        public static byte[] Decrypt(byte[] cipherData,
            byte[] Key, byte[] IV)
        {
            // Create a MemoryStream that is going to accept the
            // decrypted bytes 
            MemoryStream ms = new MemoryStream();

            // Create a symmetric algorithm. 
            // We are going to use Rijndael because it is strong and
            // available on all platforms. 
            // You can use other algorithms, to do so substitute the next
            // line with something like 
            //     TripleDES alg = TripleDES.Create(); 
            Rijndael alg = Rijndael.Create();

            // Now set the key and the IV. 
            // We need the IV (Initialization Vector) because the algorithm
            // is operating in its default 
            // mode called CBC (Cipher Block Chaining). The IV is XORed with
            // the first block (8 byte) 
            // of the data after it is decrypted, and then each decrypted
            // block is XORed with the previous 
            // cipher block. This is done to make encryption more secure. 
            // There is also a mode called ECB which does not need an IV,
            // but it is much less secure. 
            alg.Key = Key;
            alg.IV = IV;

            // Create a CryptoStream through which we are going to be
            // pumping our data. 
            // CryptoStreamMode.Write means that we are going to be
            // writing data to the stream 
            // and the output will be written in the MemoryStream
            // we have provided. 
            CryptoStream cs = new CryptoStream(ms,
                alg.CreateDecryptor(), CryptoStreamMode.Write);

            // Write the data and make it do the decryption 
            cs.Write(cipherData, 0, cipherData.Length);

            // Close the crypto stream (or do FlushFinalBlock). 
            // This will tell it that we have done our decryption
            // and there is no more data coming in, 
            // and it is now a good time to remove the padding
            // and finalize the decryption process. 
            cs.Close();

            // Now get the decrypted data from the MemoryStream. 
            // Some people make a mistake of using GetBuffer() here,
            // which is not the right way. 
            byte[] decryptedData = ms.ToArray();

            return decryptedData;
        }

        // Decrypt a string into a string using a password 
        //    Uses Decrypt(byte[], byte[], byte[]) 

        public static string Decrypt(string cipherText, string Password)
        {
            // First we need to turn the input string into a byte array. 
            // We presume that Base64 encoding was used 

            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            //byte[] cipherBytes = System.Text.Encoding.ASCII.GetBytes(cipherText); 

            // Then, we need to turn the password into Key and IV 
            // We are using salt to make it harder to guess our key
            // using a dictionary attack - 
            // trying to guess a password by enumerating all possible words. 
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 
                               0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

            // Now get the key/IV and do the decryption using
            // the function that accepts byte arrays. 
            // Using PasswordDeriveBytes object we are first
            // getting 32 bytes for the Key 
            // (the default Rijndael key length is 256bit = 32bytes)
            // and then 16 bytes for the IV. 
            // IV should always be the block size, which is by
            // default 16 bytes (128 bit) for Rijndael. 
            // If you are using DES/TripleDES/RC2 the block size is
            // 8 bytes and so should be the IV size. 
            // You can also read KeySize/BlockSize properties off
            // the algorithm to find out the sizes. 
            byte[] decryptedData = Decrypt(cipherBytes,
                pdb.GetBytes(32), pdb.GetBytes(16));

            // Now we need to turn the resulting byte array into a string. 
            // A common mistake would be to use an Encoding class for that.
            // It does not work 
            // because not all byte values can be represented by characters. 
            // We are going to be using Base64 encoding that is 
            // designed exactly for what we are trying to do. 
            return System.Text.Encoding.Unicode.GetString(decryptedData);
        }
        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(XKey))
                throw new Exception("Не задан ключ для расшифрования (CryptHelper.xKey)");

            if (string.IsNullOrEmpty(cipherText))
                return "";

            try
            {
                return Decrypt(cipherText, XKey);
            }
            catch (Exception Ex)
            {
                //xKey = "";
                throw new Exception("Ошибка при расшифровании: " + Ex.Message);
            }
        }

        public static decimal DecryptDecimal(string cipherText)
        {
            decimal ret = 0;

            if (string.IsNullOrEmpty(cipherText))
                return 0;

            try
            {
                ret = decimal.Parse(Decrypt(cipherText));
            }
            catch (Exception Ex)
            {
                string s = Ex.Message;
                ret = HiddenValue;
                //throw new Exception(string.Format("Ошибка при расшифровании: не могу преобразовать строку \"{0}\" в число", retS));
            }
            return ret;
        }
        // Decrypt bytes into bytes using a password 
        //    Uses Decrypt(byte[], byte[], byte[]) 

        public static byte[] Decrypt(byte[] cipherData, string Password)
        {
            // We need to turn the password into Key and IV. 
            // We are using salt to make it harder to guess our key
            // using a dictionary attack - 
            // trying to guess a password by enumerating all possible words. 
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
                               0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

            // Now get the key/IV and do the Decryption using the 
            //function that accepts byte arrays. 
            // Using PasswordDeriveBytes object we are first getting
            // 32 bytes for the Key 
            // (the default Rijndael key length is 256bit = 32bytes)
            // and then 16 bytes for the IV. 
            // IV should always be the block size, which is by default
            // 16 bytes (128 bit) for Rijndael. 
            // If you are using DES/TripleDES/RC2 the block size is
            // 8 bytes and so should be the IV size. 

            // You can also read KeySize/BlockSize properties off the
            // algorithm to find out the sizes. 
            return Decrypt(cipherData, pdb.GetBytes(32), pdb.GetBytes(16));
        }

        // Decrypt a file into another file using a password 
        public static void Decrypt(string fileIn,
            string fileOut, string Password)
        {

            // First we are going to open the file streams 
            FileStream fsIn = new FileStream(fileIn,
                FileMode.Open, FileAccess.Read);
            FileStream fsOut = new FileStream(fileOut,
                FileMode.OpenOrCreate, FileAccess.Write);

            // Then we are going to derive a Key and an IV from
            // the Password and create an algorithm 
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
                               0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
            Rijndael alg = Rijndael.Create();

            alg.Key = pdb.GetBytes(32);
            alg.IV = pdb.GetBytes(16);

            // Now create a crypto stream through which we are going
            // to be pumping data. 
            // Our fileOut is going to be receiving the Decrypted bytes. 
            CryptoStream cs = new CryptoStream(fsOut,
                alg.CreateDecryptor(), CryptoStreamMode.Write);

            // Now will will initialize a buffer and will be 
            // processing the input file in chunks. 
            // This is done to avoid reading the whole file (which can be
            // huge) into memory. 
            int bufferLen = 4096;
            byte[] buffer = new byte[bufferLen];
            int bytesRead;

            do
            {
                // read a chunk of data from the input file 
                bytesRead = fsIn.Read(buffer, 0, bufferLen);

                // Decrypt it 
                cs.Write(buffer, 0, bytesRead);

            } while (bytesRead != 0);

            // close everything 
            cs.Close(); // this will also close the unrelying fsOut stream 
            fsIn.Close();
        }
    }
//    public static class SignHelper
//    {
//        static bool Verify(string text, byte[] signature, string certPath)
//        {
//            // Load the certificate we'll use to verify the signature from a file 
//            X509Certificate2 cert = new X509Certificate2(certPath);
//            // Note: 
//            // If we want to use the client cert in an ASP.NET app, we may use something like this instead:
//            // X509Certificate2 cert = new X509Certificate2(Request.ClientCertificate.Certificate);

//            // Get its associated CSP and public key
//            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)cert.PublicKey.Key;

//            // Hash the data
//            SHA1Managed sha1 = new SHA1Managed();
//            UnicodeEncoding encoding = new UnicodeEncoding();
//            byte[] data = encoding.GetBytes(text);
//            byte[] hash = sha1.ComputeHash(data);

//            // Verify the signature with the hash
//            return csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), signature);
//        }

//        static byte[] Sign(string text, string certSubject)
//        {
//            // Access Personal (MY) certificate store of current user
//            X509Store my = new X509Store(StoreName.My, StoreLocation.CurrentUser);
//            my.Open(OpenFlags.ReadOnly);

//            // Find the certificate we'll use to sign            
//            RSACryptoServiceProvider csp = null;
//            foreach (X509Certificate2 cert in my.Certificates)
//            {
//                if (cert.Subject.Contains(certSubject))
//                {
//                    // We found it. 
//                    // Get its associated CSP and private key
//                    csp = (RSACryptoServiceProvider)cert.PrivateKey;
//                }
//            }
//            if (csp == null)
//            {
//                throw new Exception("No valid cert was found");
//            }

//            // Hash the data
//            SHA1Managed sha1 = new SHA1Managed();
//            UnicodeEncoding encoding = new UnicodeEncoding();
//            byte[] data = encoding.GetBytes(text);
//            byte[] hash = sha1.ComputeHash(data);

//            // Sign the hash
//            return csp.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));
//        }

//        public static bool VerifySignature(byte[] body, X509Certificate2 cert)
//        {
//            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)cert.PublicKey.Key;
//            SHA1Managed sha1 = new SHA1Managed();
//            byte[] hash = sha1.ComputeHash(body);

//            // Verify the signature with the hash
//            //return csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), signature);
//            //csp.
//            return csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), body);
//        }

//        public static bool VerifySignature(byte[] body, byte[] pubKey)
//        {
//            X509Certificate2 cert = new X509Certificate2(pubKey);
//            return VerifySignature(body, cert);
//        }

//        public static bool VerifyAssemblySignature(byte[] body, out List<string> subject)
//        {
//            subject = new List<string>();
//            Assembly a = Assembly.Load(body);
//            X509Certificate c0 = a.ManifestModule.GetSignerCertificate();
//            X509Certificate2 cert = new X509Certificate2(c0);

//            X509Chain chain = new X509Chain();

//            chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
//            //chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
//            chain.ChainPolicy.ExtraStore.Add(cert);

//            bool ret = chain.Build(cert);
//            foreach (X509ChainElement item in chain.ChainElements)
//            {
//                subject.Add(item.Certificate.Subject);
//            }
//            return ret;
//        }

//        public static byte[] VerifyAndRemoveSignature(byte[] data)
//        {

//            SignedCms signedMessage = null;

//            // deserialize PKCS #7 byte array
//            try
//            {
//                // create SignedCms

//                //ContentInfo contentInfo = new ContentInfo(data);
//                //signedMessage = new SignedCms(contentInfo, false);
//                //signedMessage.deta
//                signedMessage = new SignedCms();
//                //signedMessage.Detached = false;
//                signedMessage.Decode(data);


//                // check signature
//                // false checks signature and certificate
//                // true only checks signature
//                signedMessage.CheckSignature(true);
//                signedMessage.CheckSignature(false);

//                // access signature certificates (if needed)
//                foreach (SignerInfo signer in signedMessage.SignerInfos)
//                {
//                    Console.WriteLine("Subject: {0}", signer.Certificate.Subject);
//                }

//                // return plain data without signature
//            }
//            catch (System.Exception Ex)
//            {
//            }
//            return signedMessage.ContentInfo.Content;
//        }


//        public static bool VerifyCertChain(X509Certificate2 cert, out List<string> subject)
//        {
//            subject = new List<string>();
//            //X509Certificate2Collection certificates = new X509Certificate2Collection(cert);

//            X509Chain chain = new X509Chain();

//            chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
//            //chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
//            chain.ChainPolicy.ExtraStore.Add(cert);

//            bool ret = chain.Build(cert);
//            foreach (X509ChainElement item in chain.ChainElements)
//            {
//                subject.Add(item.Certificate.Subject);
//            }
//            return ret;
//        }
//        public static bool VerifyCert(string filename)
//        {
//            // verify the chain
//            bool valid = CertVerify.WinTrust.VerifyEmbeddedSignature(filename, CertVerify.WinTrust.WINTRUST_ACTION_GENERIC_VERIFY_V2, CertVerify.WinTrustDataRevocationChecks.WholeChain);

//            //If the chain is valid, the final step is to publisher

//            if (valid)
//            {
//                X509Certificate cert = X509Certificate.CreateFromSignedFile(filename); // this will throw an exception if the file does not have a cert
//                X509Certificate2 x509 = new X509Certificate2(cert);

//                // check the subject
//                valid &= x509.Subject.Contains("ROSBANK");
//            }
//            return valid;
//        }

//        public static bool IsTimestamped(string filename)
//        {
//            try
//            {
//                int encodingType;
//                int contentType;
//                int formatType;
//                IntPtr certStore = IntPtr.Zero;
//                IntPtr cryptMsg = IntPtr.Zero;
//                IntPtr context = IntPtr.Zero;

//                if (!WinCrypt.CryptQueryObject(
//                    WinCrypt.CERT_QUERY_OBJECT_FILE,
//                    //WinCrypt.CERT_QUERY_OBJECT_BLOB,
//                    Marshal.StringToHGlobalUni(filename),
//                    WinCrypt.CERT_QUERY_CONTENT_FLAG_ALL,
//                    WinCrypt.CERT_QUERY_FORMAT_FLAG_ALL,
//                    0,
//                    out encodingType,
//                    out contentType,
//                    out formatType,
//                    ref certStore,
//                    ref cryptMsg,
//                    ref context))
//                {
//                    throw new Win32Exception(Marshal.GetLastWin32Error());
//                }

//                //expecting contentType=10; CERT_QUERY_CONTENT_PKCS7_SIGNED_EMBED 
//                //Logger.LogInfo(string.Format("Querying file '{0}':", filename));
//                //Logger.LogInfo(string.Format("  Encoding Type: {0}", encodingType));
//                //Logger.LogInfo(string.Format("  Content Type: {0}", contentType));
//                //Logger.LogInfo(string.Format("  Format Type: {0}", formatType));
//                //Logger.LogInfo(string.Format("  Cert Store: {0}", certStore.ToInt32()));
//                //Logger.LogInfo(string.Format("  Crypt Msg: {0}", cryptMsg.ToInt32()));
//                //Logger.LogInfo(string.Format("  Context: {0}", context.ToInt32()));


//                // Get size of the encoded message.
//                int cbData = 0;
//                if (!WinCrypt.CryptMsgGetParam(
//                    cryptMsg,
//                    WinCrypt.CMSG_ENCODED_MESSAGE,//Crypt32.CMSG_SIGNER_INFO_PARAM,
//                    0,
//                    IntPtr.Zero,
//                    ref cbData))
//                {
//                    throw new Win32Exception(Marshal.GetLastWin32Error());
//                }

//                var vData = new byte[cbData];

//                // Get the encoded message.
//                if (!WinCrypt.CryptMsgGetParam(
//                    cryptMsg,
//                    WinCrypt.CMSG_ENCODED_MESSAGE,//Crypt32.CMSG_SIGNER_INFO_PARAM,
//                    0,
//                    vData,
//                    ref cbData))
//                {
//                    throw new Win32Exception(Marshal.GetLastWin32Error());
//                }

//                var signedCms = new SignedCms();
//                signedCms.Decode(vData);

//                foreach (var signerInfo in signedCms.SignerInfos)
//                {
//                    foreach (var unsignedAttribute in signerInfo.UnsignedAttributes)
//                    {
//                        if (unsignedAttribute.Oid.Value == WinCrypt.szOID_RSA_counterSign)
//                        {
//                            //Note at this point we assume this counter signature is the timestamp
//                            //refer to http://support.microsoft.com/kb/323809 for the origins

//                            //TODO: extract timestamp value, if required
//                            return true;
//                        }

//                    }
//                }
//            }
//            catch (Exception)
//            {
//                // no logging
//            }

//            return false;
//        }
//    }
//    public static class XmlSignHelper
//    {

//        //public static void Main(String[] args)
//        //{
//        //    string Certificate = "CN=XMLDSIG_Test";
//        //    try
//        //    {
//        //        // Create an XML file to sign.
//        //        CreateSomeXml("Example.xml");
//        //        Console.WriteLine("New XML file created.");

//        //        // Sign the XML that was just created and save it in a  // new file.
//        //        SignXmlFile("Example.xml", "SignedExample.xml", Certificate);
//        //        Console.WriteLine("XML file signed.");

//        //        if (VerifyXmlFile("SignedExample.xml", Certificate))
//        //        {
//        //            Console.WriteLine("The XML signature is valid.");
//        //        }
//        //        else
//        //        {
//        //            Console.WriteLine("The XML signature is not valid.");
//        //        }
//        //    }
//        //    catch (CryptographicException e)
//        //    {
//        //        Console.WriteLine(e.Message);
//        //    }
//        //}

//        // Sign an XML file and save the signature in a new file. 
//        public static void SignXmlFile(string FileName, string SignedFileName, string SubjectName)
//        {
//            if (null == FileName)
//                throw new ArgumentNullException("FileName");
//            if (null == SignedFileName)
//                throw new ArgumentNullException("SignedFileName");
//            if (null == SubjectName)
//                throw new ArgumentNullException("SubjectName");

//            // Load the certificate from the certificate store.
//            X509Certificate2 cert = GetCertificateBySubject(SubjectName);

//            // Create a new XML document.
//            XmlDocument doc = new XmlDocument();

//            // Format the document to ignore white spaces.
//            doc.PreserveWhitespace = false;

//            // Load the passed XML file using it's name.
//            doc.Load(new XmlTextReader(FileName));

//            // Create a SignedXml object.
//            SignedXml signedXml = new SignedXml(doc);

//            // Add the key to the SignedXml document. 
//            signedXml.SigningKey = cert.PrivateKey;

//            // Create a reference to be signed.
//            Reference reference = new Reference();
//            reference.Uri = "";

//            // Add an enveloped transformation to the reference.
//            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
//            reference.AddTransform(env);

//            // Add the reference to the SignedXml object.
//            signedXml.AddReference(reference);

//            // Create a new KeyInfo object.
//            KeyInfo keyInfo = new KeyInfo();

//            // Load the certificate into a KeyInfoX509Data object 
//            // and add it to the KeyInfo object.
//            keyInfo.AddClause(new KeyInfoX509Data(cert));

//            // Add the KeyInfo object to the SignedXml object.
//            signedXml.KeyInfo = keyInfo;

//            // Compute the signature.
//            signedXml.ComputeSignature();

//            // Get the XML representation of the signature and save 
//            // it to an XmlElement object.
//            XmlElement xmlDigitalSignature = signedXml.GetXml();

//            // Append the element to the XML document.
//            doc.DocumentElement.AppendChild(doc.ImportNode(xmlDigitalSignature, true));


//            if (doc.FirstChild is XmlDeclaration)
//            {
//                doc.RemoveChild(doc.FirstChild);
//            }

//            // Save the signed XML document to a file specified 
//            // using the passed string. 
//            using (XmlTextWriter xmltw = new XmlTextWriter(SignedFileName, new UTF8Encoding(false)))
//            {
//                doc.WriteTo(xmltw);
//                xmltw.Close();
//            }

//        }

//        // Verify the signature of an XML file against an asymetric  // algorithm and return the result. 
//        public static Boolean VerifyXmlFile(String FileName, String CertificateSubject)
//        {
//            // Check the args. 
//            if (null == FileName)
//                throw new ArgumentNullException("FileName");
//            if (null == CertificateSubject)
//                throw new ArgumentNullException("CertificateSubject");

//            // Load the certificate from the store.
//            X509Certificate2 cert = GetCertificateBySubject(CertificateSubject);

//            // Create a new XML document.
//            XmlDocument xmlDocument = new XmlDocument();

//            // Load the passed XML file into the document. 
//            xmlDocument.Load(FileName);

//            // Create a new SignedXml object and pass it 
//            // the XML document class.
//            SignedXml signedXml = new SignedXml(xmlDocument);

//            // Find the "Signature" node and create a new // XmlNodeList object.
//            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("Signature");

//            // Load the signature node.
//            signedXml.LoadXml((XmlElement)nodeList[0]);

//            // Check the signature and return the result. 
//            return signedXml.CheckSignature(cert, true);

//        }


//        public static X509Certificate2 GetCertificateBySubject(string CertificateSubject)
//        {
//            // Check the args. 
//            if (null == CertificateSubject)
//                throw new ArgumentNullException("CertificateSubject");


//            // Load the certificate from the certificate store.
//            X509Certificate2 cert = null;

//            X509Store store = new X509Store("My", StoreLocation.CurrentUser);

//            try
//            {
//                // Open the store.
//                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

//                // Get the certs from the store.
//                X509Certificate2Collection CertCol = store.Certificates;

//                // Find the certificate with the specified subject. 
//                foreach (X509Certificate2 c in CertCol)
//                {
//                    if (c.Subject == CertificateSubject)
//                    {
//                        cert = c;
//                        break;
//                    }
//                }

//                // Throw an exception of the certificate was not found. 
//                if (cert == null)
//                {
//                    throw new CryptographicException("The certificate could not be found.");
//                }
//            }
//            finally
//            {
//                // Close the store even if an exception was thrown.
//                store.Close();
//            }

//            return cert;
//        }

//        // Create example data to sign. 
//        public static void CreateSomeXml(string FileName)
//        {
//            // Check the args. 
//            if (null == FileName)
//                throw new ArgumentNullException("FileName");

//            // Create a new XmlDocument object.
//            XmlDocument document = new XmlDocument();

//            // Create a new XmlNode object.
//            XmlNode node = document.CreateNode(XmlNodeType.Element, "", "MyElement", "samples");

//            // Add some text to the node.
//            node.InnerText = "Example text to be signed.";

//            // Append the node to the document.
//            document.AppendChild(node);

//            // Save the XML document to the file name specified. 
//            using (XmlTextWriter xmltw = new XmlTextWriter(FileName, new UTF8Encoding(false)))
//            {
//                document.WriteTo(xmltw);

//                xmltw.Close();
//            }


//        }

//    }
}
