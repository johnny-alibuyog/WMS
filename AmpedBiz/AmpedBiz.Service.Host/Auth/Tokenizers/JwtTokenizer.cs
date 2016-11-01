using AmpedBiz.Common.Configurations;
using AmpedBiz.Service.Host.Auth.Models;
using JWT;

namespace AmpedBiz.Service.Host.Auth.Tokenizers
{
    public class JwtTokenizer : ITokenizer<User>
    {
        public User Decode(string token)
        {
            return JsonWebToken.DecodeToObject<User>(token, AuthConfig.Instance.Secret);
        }

        public string Encode(User identity)
        {
            return JsonWebToken.Encode(identity, AuthConfig.Instance.Secret, JwtHashAlgorithm.HS256);
        }
    }
}