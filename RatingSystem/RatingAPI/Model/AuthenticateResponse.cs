using RatingAPI.Model;

namespace RatingAPI.Model
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(LoginInfo user, string token)
        {
            Id = user.Id;
            Username = user.username;
            Token = token;
        }
    }
}