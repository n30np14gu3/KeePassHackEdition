using System;

namespace KeePassHackEdition.SDK.License
{

    [Serializable]
    public class LicenseConfig
    {
        public  string LicensePath { get; set; }
        public string ActivationResponse { get; set; }

    }
}