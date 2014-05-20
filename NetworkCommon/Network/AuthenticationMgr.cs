using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using CommonUtils.Network.Packets;
//using System.Diagnostics;
using System.Collections;

namespace CommonUtils.Network
{
    [Serializable]
    public class AuthenticationMgr
    {
        public string username
        {
            get;
            private set;
        } // Utente server

        public byte[] salt
        {
            get;
            set;
        } // salt to avoid replay attacks

        [NonSerialized]
        private string password; // Dont' serialize, won't be included in serialized packet

        public byte[] pwdHash
        {
            get;
            private set;
        }

        public AuthenticationMgr(string username, string password)
        {
            this.password = password;
            this.username = username;
        }


        public AuthenticationMgr(string username, string password, byte[] salt)
        {
            this.pwdHash = this.GenerateHash(password, salt);
            this.username = username;
        }

        public bool Authenticate(byte[] cmppassword, byte[] salt)
        {
            byte[] pwdHash = this.GenerateHash(this.password, salt);
            return ByteArrayCompare(pwdHash, cmppassword);
        }

        private static bool ByteArrayCompare(byte[] a1, byte[] a2)
        {
            IStructuralEquatable eqa1 = a1;
            return eqa1.Equals(a2, StructuralComparisons.StructuralEqualityComparer);
        }

        byte[] GenerateHash(string password, byte[] salt)
        {
            byte[] pwdHash;
            using (SHA512 shaM = new SHA512Managed())
            {
                UnicodeEncoding UE = new UnicodeEncoding();

                byte[] bytePassword = UE.GetBytes(password);
                byte[] passWithSaltBytes = new byte[bytePassword.Length + salt.Length];
                
                Array.Copy(bytePassword, 0, passWithSaltBytes, 0, bytePassword.Length);
                Array.Copy(salt.ToArray(), 0, passWithSaltBytes, password.Length, salt.Length);

                pwdHash = shaM.ComputeHash(passWithSaltBytes);
            }
            //Debug.Print("hash generated: " + pwdHash.ToString());
            return pwdHash;
        }

        public bool VerifyResponse(AuthenticationResponse ar, byte[] salt)
        {
            return Authenticate(ar.passwordHash, salt);
        }

        /// <summary>
        /// Generate a random salt to use in the authentication phase
        /// </summary>
        /// <param name="saltSize">the size (number of bytes) of the salt we want to generate</param>
        /// <returns>return the generated salt</returns>
        public static byte[] GenerateSalt(int saltSize)
        {
            byte[] salt = new byte[saltSize];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetNonZeroBytes(salt); // Fill the salt with cryptographically strong byte values.
            }
            return salt;
        }


        /// <summary>
        /// used by the client to generate a authentication response packet
        /// after setting the salt
        /// </summary>
        /// <returns>an authentication response packet with password hash</returns>
        public AuthenticationResponse GenerateResponse()
        {
            return new AuthenticationResponse(this.username, GenerateHash(this.password,this.salt) );
        }
    }
}
