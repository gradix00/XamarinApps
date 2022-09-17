namespace TestBDonline.Scripts.Structs
{
    public enum Status
    {
        admin,
        user,
        banned
    }
    public struct UserData
    {
        public int ID { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public int Points { get; set; }
        public Status Status { get; set; }
    }
}
