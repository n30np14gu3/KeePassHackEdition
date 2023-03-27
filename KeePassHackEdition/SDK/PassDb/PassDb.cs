using KeePassHackEdition.SDK.Crypto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace KeePassHackEdition.SDK.PassDb
{
    public class PassDb
    {
        private string _path;
        private string _password;
        private string _databaseName;

        private PasswordsDatabase _database;
        private const uint DatabaseMagic = 0x13377331;
        private Usca _sessionUsca;
        private Usca _masterUsca;


        public PassDb(string path, string password)
        {
            _password = password;
            _path = path;
            _database = null;
        }

        public void InitDb(string dbName)
        {
            if (_database != null)
                throw new Exception("Database already created");

            _database = new PasswordsDatabase();
            _database.Magic = DatabaseMagic;
            _database.DatabasePasswordHash = SimpleTools.Sha256(Encoding.UTF8.GetBytes(_password));
            _database.Entries = new List<DatabaseEntry>();

            InitSessionKey();

            //Generate master key password
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] rngPassword = new byte[128];
                byte[] salt = new byte[32];
                rng.GetBytes(rngPassword);
                using (Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(rngPassword, salt, new Random((int)DateTime.Now.Ticks).Next(0x1337)))
                {
                    byte[] key = rfc.GetBytes(32);
                    byte[] iv = rfc.GetBytes(16);
                    _masterUsca = new Usca(key, iv);
                }
            }

            _databaseName = dbName;

        }

        public void LoadDb()
        {
            if (_database != null)
                throw new Exception("Database already init");

            if (!File.Exists(_path))
                throw new Exception("File not exists");

            try
            {
                byte[] dbBytes = File.ReadAllBytes(_path);
                CryptDatabase(dbBytes);
                string xml = Encoding.ASCII.GetString(dbBytes);
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(PasswordsDatabase));
                    _database = xmlSerializer.Deserialize(sr) as PasswordsDatabase;
                }
            }
            catch
            {
                throw new Exception("Invalid database format");
            }

            if (_database == null)
                throw new Exception("Can't load database");

            if (_database.DatabasePasswordHash != SimpleTools.Sha256(Encoding.ASCII.GetBytes(_password)))
                throw new Exception("Invalid password");

            InitSessionKey();
            
            //Init master key
            DecryptEntryData(_database.Entries[0]);
            byte[] key = Convert.FromBase64String(Encoding.ASCII.GetString(Convert.FromBase64String(_database.Entries[0].EntryData)));
            _masterUsca = new Usca(key.Take(32).ToArray(), key.Skip(32).Take(16).ToArray());
            DecryptEntryData(_database.Entries[1]);
            _databaseName = Encoding.ASCII.GetString(Convert.FromBase64String(_database.Entries[1].EntryData));

            //Remove master key & db name
            _database.Entries.RemoveAt(0);
            _database.Entries.RemoveAt(0);

            foreach (DatabaseEntry entry in _database.Entries)
            {
                DecryptEntryName(entry);
            }
        }

        public void AddPassword(string entryName, string password)
        {
            if (_database == null)
                throw new Exception("Database not init");

            DatabaseEntry entry = new DatabaseEntry
            {
                DataEncrypted = false,
                NameEncrypted = false,
                EntryData = Convert.ToBase64String(Encoding.ASCII.GetBytes(password)),
                EntryName = entryName,
                EntryType = DatabaseEntryType.EntryTypePassword,
            };
            EncryptEntryData(entry);
            _database.Entries.Add(entry);
        }

        public string GetPassword(int index)
        {
            if (_database == null)
                throw new Exception("Database not init");

            if (index >= _database.Entries.Count || index < 0)
                throw new Exception("Invalid index");

            if (_database.Entries[index].EntryType != DatabaseEntryType.EntryTypePassword)
                throw new Exception("Invalid entry type");

            DecryptEntryData(_database.Entries[index]);
            string password = Encoding.ASCII.GetString(Convert.FromBase64String(Encoding.ASCII.GetString(Convert.FromBase64String(_database.Entries[index].EntryData))));
            EncryptEntryData(_database.Entries[index]);

            return password;
        }

        public void AddFile(string fileName, string filePath)
        {
            if (_database == null)
                throw new Exception("Database not init");

            if (!File.Exists(filePath))
                throw new Exception("File not exists");

            DatabaseEntry entry = new DatabaseEntry
            {
                DataEncrypted = false,
                NameEncrypted = false,
                EntryName = fileName,
                EntryData = Convert.ToBase64String(File.ReadAllBytes(filePath)),
                EntryType = DatabaseEntryType.EntryTypeFile,
            };

            EncryptEntryData(entry);
            _database.Entries.Add(entry);
        }

        public void ExportFile(int index, string savePath)
        {
            if (_database == null)
                throw new Exception("Database not init");

            if (index >= _database.Entries.Count || index < 0)
                throw new Exception("Invalid index");

            if (_database.Entries[index].EntryType != DatabaseEntryType.EntryTypeFile)
                throw new Exception("Invalid entry type");

            DecryptEntryData(_database.Entries[index]);
            byte[] fileData = Convert.FromBase64String(Encoding.ASCII.GetString(Convert.FromBase64String(_database.Entries[index].EntryData)));
            using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(fileData, 0, fileData.Length);
            }
            EncryptEntryData(_database.Entries[index]);
        }

        public void Save()
        {
            if (_database == null)
                throw new Exception("Database not init");

            _database.Entries.Insert(0, new DatabaseEntry
                {
                    DataEncrypted = false,
                    EntryData = Convert.ToBase64String(_masterUsca.Key().Concat(_masterUsca.Iv()).ToArray()),
                    EntryType = DatabaseEntryType.EntryTypeMasterKey,
                    EntryName = null,
                    NameEncrypted = false
                }
            );

            _database.Entries.Insert(1, new DatabaseEntry
                {
                    DataEncrypted = false,
                    EntryData = Convert.ToBase64String(Encoding.ASCII.GetBytes(_databaseName)),
                    EntryType = DatabaseEntryType.EntryTypeDbName,
                    EntryName = null,
                    NameEncrypted = false
                }
            );

            foreach (DatabaseEntry entry in _database.Entries)
            {
                EncryptEntryData(entry);
                EncryptEntryName(entry);
            }

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PasswordsDatabase));
            using (StringWriter sw = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sw))
                {
                    xmlSerializer.Serialize(writer, _database);
                    using (FileStream fs = new FileStream(_path, FileMode.Create, FileAccess.Write))
                    {
                        byte[] xml = Encoding.ASCII.GetBytes(sw.ToString());
                        CryptDatabase(xml);
                        fs.Write(xml, 0, xml.Length);
                    }
                }
            }
        }

        private void InitSessionKey()
        {
            using (Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(_password, Encoding.UTF8.GetBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes(_password)))))
            {
                byte[] key = rfc.GetBytes(32);
                byte[] iv = rfc.GetBytes(16);
                SecretAlg alg = new SecretAlg(_password);
                alg.Crypt(key);
                alg.Crypt(iv);
                _sessionUsca = new Usca(key, iv);
            }
        }

        private void CryptDatabase(byte[] databaseBytes)
        {
            string cryptKey = "oQPnRzA1EjnwG94oGuu3IaT4bFTVxhLzLY1vevlf3H6qvYcg3TbHIjfPGsugk3bRDnHR3MSeWbwQ1kHJy1LYbrtbI9ONSMfsD9KGVicPuaYSX";
            SecretAlg alg = new SecretAlg(cryptKey);
            alg.Crypt(databaseBytes);
        }

        private void EncryptEntryName(DatabaseEntry entry)
        {
            if(entry.NameEncrypted || string.IsNullOrEmpty(entry.EntryName))
                return;

            entry.EntryName = Convert.ToBase64String(
                (entry.EntryType == DatabaseEntryType.EntryTypeMasterKey) ?
                    _sessionUsca.Encrypt(Encoding.ASCII.GetBytes(entry.EntryName)) :
                    _masterUsca.Encrypt(Encoding.ASCII.GetBytes(entry.EntryName))
            );
            entry.NameEncrypted = true;
        }

        private void DecryptEntryName(DatabaseEntry entry)
        {
            if (!entry.NameEncrypted || string.IsNullOrEmpty(entry.EntryName))
                return;


            entry.EntryName = Encoding.ASCII.GetString(
                (entry.EntryType == DatabaseEntryType.EntryTypeMasterKey) ?
                    _sessionUsca.Decrypt(Convert.FromBase64String(entry.EntryName)) :
                    _masterUsca.Decrypt(Convert.FromBase64String(entry.EntryName))
            );
            entry.NameEncrypted = false;
        }

        private void EncryptEntryData(DatabaseEntry entry)
        {
            if (entry.DataEncrypted || string.IsNullOrEmpty(entry.EntryData))
                return;

            entry.EntryData = Convert.ToBase64String(
                (entry.EntryType == DatabaseEntryType.EntryTypeMasterKey) ?
                    _sessionUsca.Encrypt(Encoding.ASCII.GetBytes(entry.EntryData)) : 
                    _masterUsca.Encrypt(Encoding.ASCII.GetBytes(entry.EntryData))
                    );
            entry.DataEncrypted = true;
        }

        private void DecryptEntryData(DatabaseEntry entry)
        {
            if (!entry.DataEncrypted || string.IsNullOrEmpty(entry.EntryData))
                return;


            entry.EntryData = Convert.ToBase64String(
                (entry.EntryType == DatabaseEntryType.EntryTypeMasterKey) ? 
                    _sessionUsca.Decrypt(Convert.FromBase64String(entry.EntryData)) : 
                    _masterUsca.Decrypt(Convert.FromBase64String(entry.EntryData))
                    );
            entry.DataEncrypted = false;
        }
    }
}