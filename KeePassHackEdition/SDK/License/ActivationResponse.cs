namespace KeePassHackEdition.SDK.License
{
    public class ActivationResponse
    {
        public bool Success { get; set; }
        public ulong ResponseId { get; set; }
        public ulong LicenseVersion { get; set; }
        public string Magic { get; set; }
    }
}