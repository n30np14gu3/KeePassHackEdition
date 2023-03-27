using System;

namespace KeePassHackEdition.SDK.PassDb
{
    [Serializable]
    public class DatabaseEntry
    {
        public bool NameEncrypted { get; set; }
        public bool DataEncrypted { get; set; }
        public DatabaseEntryType EntryType { get; set; }
        public string EntryName { get; set; }
        public string EntryData { get; set; }
    }
}