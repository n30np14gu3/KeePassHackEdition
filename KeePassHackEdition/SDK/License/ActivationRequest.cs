using System;

namespace KeePassHackEdition.SDK.License
{
    [Serializable]
    public class ActivationRequest
    {
        public ulong RequestId { get; set; }
        public ulong LicenseVersion { get; set; }
        public string UserId { get; set; }
    }
}