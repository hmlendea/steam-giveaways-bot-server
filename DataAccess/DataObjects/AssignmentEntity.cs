namespace SteamAccountDistributor.DataAccess.DataObjects
{
    public sealed class AssignmentEntity
    {
        public string Hostname { get; set; }

        public string Password { get; set; }

        public string AssignedSteamAccount { get; set; }
    }
}
