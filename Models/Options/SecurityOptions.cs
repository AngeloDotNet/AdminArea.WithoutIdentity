namespace AdminArea_Without_Identity.Models.Options
{
    public class SecurityOptions
    {
        public string PasswordSalt { get; set; }
        public int Iterations { get; set; }
    }
}