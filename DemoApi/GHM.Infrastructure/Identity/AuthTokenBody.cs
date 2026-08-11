namespace GHM.Infrastructure.Identity
{ 
    public class AuthTokenBody
    {
      public string token { get; set; }
    }

    public class AuthTokenResponse
    {
        public bool Active { get; set; }
        public string Scope { get; set; }
        public string Realm { get; set; }
        public string Client_id { get; set; }
        public string User_id { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Avatar { get; set; }
    }
}
