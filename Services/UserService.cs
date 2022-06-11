using AuthProject.Models;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthProject.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> users;
        private readonly string  key;

        public UserService(IDatabaseSettings settings, IMongoClient mongoClient, IConfiguration configuration)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            users = database.GetCollection<User>(settings.CollectionName);
            this.key = configuration.GetSection("JwtKey").ToString();

        }


        public string Authenticate(string email, string password)
        {
            var user = this.users.Find(x => x.Email == email && x.Password == password).FirstOrDefault();
            if (user == null) return null;


            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenKey = Encoding.ASCII.GetBytes(key);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim (ClaimTypes.Email, email),
                }),

                Expires = DateTime.UtcNow.AddHours(1),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature
                    )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public User Create(User user)
        {
           users.InsertOne(user);
            return user;
        }

        public User GetUser(string id)
        {
            User user = users.Find<User>(user => user.Id == id).FirstOrDefault();
            return user;
        }

        public List<User> GetUsers()
        {
            return users.Find<User>(user => true).ToList();
        }
    }
}
