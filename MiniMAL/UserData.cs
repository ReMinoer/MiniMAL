﻿using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace MiniMAL
{
    public struct UserData
    {
        public string Username;
        public byte[] Password;

        [XmlIgnore]
        public string DecryptedPassword
        {
            get
            {
                return Encoding.GetString(ProtectedData.Unprotect(Password, null, DataProtectionScope.CurrentUser));
            }
            set
            {
                Password = ProtectedData.Protect(Encoding.GetBytes(value), null, DataProtectionScope.CurrentUser);
            }
        }

        private static ASCIIEncoding Encoding = new ASCIIEncoding();

        public static void Save(UserData data, string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UserData));
            StreamWriter sw = new StreamWriter(filename);
            serializer.Serialize(sw, data);
            sw.Close();
        }

        public static UserData Load(string filename)
        {
            if (!File.Exists(filename))
            {
                UserData newData = new UserData();
                Save(newData, filename);
                return newData;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(UserData));
            StreamReader sr = new StreamReader(filename);
            UserData data = (UserData)serializer.Deserialize(sr);

            sr.Close();
            return data;
        }
    }
}