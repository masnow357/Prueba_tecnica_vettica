namespace PruebaTecnica.Interfaces
{
    public interface ILoginUser
    {
        public string email { get; set; }
        public string password { get; set; }
    }

    public class LoginUser : ILoginUser
    {
        public string email { get; set; }
        public string password { get; set; }

    }
}
