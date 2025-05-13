using RatingAPI.Interfaces;
using RatingAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace RatingAPI.Services
{
    public class UserService : IUserService
    {

        private readonly string _Secrets;
        //ivate readonly OurHeroDbContext db;
        private string connectionString = "Server=DESKTOP-G9FEQCM;Database=RatingSystem;Integrated Security=true;";

        //public UserService(IOptions<AppSettings> appSettings, OurHeroDbContext _db)
        //{
        //    _appSettings = appSettings.Value;
        //    db = _db;
        //}
        public UserService(string Secrets)
        {
            _Secrets = Secrets;
        }


        public  string Authenticate(string username, string password)
        {

            using (IDbConnection db = new SqlConnection(ConfigModel.ConnectionString))
            {
                // Modify the SQL query to select the login info based on the input username
                LoginInfo? loginInfo = db.QuerySingleOrDefault<LoginInfo>(
                    $"SELECT LoginUsername, LoginPassword FROM LoginTable WHERE LoginUsername = @Username AND LoginPassword = @Password",
                    new { Username = username, Password = password }
                );

                if (loginInfo != null)
                {
                    var token =  generateJwtToken(loginInfo);

                    return token;
                }
                else
                {
                    return null;
                }


            }





            //var user = await db.Users.SingleOrDefaultAsync(x => x.Username == model.Username && x.Password == model.Password);
            
            //// return null if user not found
            //if (user == null) return null;

            //// authentication successful so generate jwt token
            //var token = await generateJwtToken(user);

            //return new AuthenticateResponse(user, token);
        }

        //public async Task<IEnumerable<User>> GetAll()
        //{
        //    return await db.Users.Where(x => x.isActive == true).ToListAsync();
        //}

        //public async Task<User?> GetById(int id)
        //{
        //    return await db.Users.FirstOrDefaultAsync(x => x.Id == id);
        //}

        //public async Task<User?> AddAndUpdateUser(User userObj)
        //{
        //    bool isSuccess = false;
        //    if (userObj.Id > 0)
        //    {
        //        var obj = await db.Users.FirstOrDefaultAsync(c => c.Id == userObj.Id);
        //        if (obj != null)
        //        {
        //            // obj.Address = userObj.Address;
        //            obj.FirstName = userObj.FirstName;
        //            obj.LastName = userObj.LastName;
        //            db.Users.Update(obj);
        //            isSuccess = await db.SaveChangesAsync() > 0;
        //        }
        //    }
        //    else
        //    {
        //        await db.Users.AddAsync(userObj);
        //        isSuccess = await db.SaveChangesAsync() > 0;
        //    }

        //    return isSuccess ? userObj : null;
        //}




        // helper methods
        private string generateJwtToken(LoginInfo user)
        {
            //Generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            

                var key = Encoding.ASCII.GetBytes(_Secrets);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                    //Expires = DateTime.UtcNow.AddDays(7),
                    //Expires = DateTime.UtcNow.AddMinutes(1),
                    Expires = DateTime.UtcNow.AddHours(2),


                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            

            return tokenHandler.WriteToken(token);
        }
    }
}