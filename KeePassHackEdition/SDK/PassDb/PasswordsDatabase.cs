using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace KeePassHackEdition.SDK.PassDb
{
    [Serializable]
    public class PasswordsDatabase
    {
        public uint Magic { get; set; }

        public string DatabasePasswordHash { get; set; }

        //[XmlArray("EntriesList"), XmlArrayItem(typeof(DatabaseEntry), ElementName = "Entry")]
        public List<DatabaseEntry> Entries { get; set; }
    }
}