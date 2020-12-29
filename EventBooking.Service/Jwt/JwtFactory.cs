using Microsoft.Extensions.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Collections.Generic;
using Serilog;
using EventBooking.Model;


namespace EventBooking.Service
{
    public class JwtFactory : IJwtFactory
    {
        private readonly MvJwtIssuerOptions _jwtOptions;

        public JwtFactory(IOptions<MvJwtIssuerOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
        }

        private async Task<string> GenerateEncodedToken(string username, ClaimsIdentity identity)
        {
            var claims = new[]
         {
                 new Claim(JwtRegisteredClaimNames.Sub, username),
                 new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                 new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                 identity.FindFirst(Constants.Strings.JwtClaimIdentifiers.PersonId),
                 identity.FindFirst("name"),
                 identity.FindFirst("role"),
                 identity.FindFirst("dateOfBirth"),
                 identity.FindFirst("gender"),
                 identity.FindFirst("username"),
                 identity.FindFirst("email"),
                 identity.FindFirst("phone"),
             };

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,// _jwtOptions.NotBefore,
                expires: DateTime.Now.AddDays(5),//_jwtOptions.Expiration, change later on the basis of requirement
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public ClaimsIdentity GenerateClaimsIdentity(MvPerson person)
        {
            try
            {
                var claims = new List<Claim> {
                new Claim(Constants.Strings.JwtClaimIdentifiers.PersonId, person.PersonId.ToString()),
                //new Claim(Constants.Strings.JwtClaimIdentifiers.Rol, person.Role /*Constants.Strings.JwtClaims.ApiAccess*/),
                new Claim("username", person.Username),
                new Claim("role", person.Role),
                };
                if (person.Name != null) claims.Add(new Claim("name", person.Name));
                if (person.DateOfBirth != null) claims.Add(new Claim("dateOfBirth", person.DateOfBirth.ToString()));
                if (person.Gender != null) claims.Add(new Claim("gender", person.Gender));
                if (person.Email != null) claims.Add(new Claim("email", person.Email));
                return new ClaimsIdentity(new GenericIdentity(person.Username, "Token"), claims);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return null;
            }
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);

        private static void ThrowIfInvalidOptions(MvJwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= 0 /*TimeSpan.Zero*/ )
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(MvJwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(MvJwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(MvJwtIssuerOptions.JtiGenerator));
            }
        }

        public async Task<object> GenerateJwt(ClaimsIdentity identity, string username, MvPersonResult personResult)
        {
            personResult.accessToken = await GenerateEncodedToken(username, identity);
            return personResult;
        }

        public async Task<object> GenerateJwt(MvPersonResult personResult)
        {
            MvPerson person = new MvPerson();
            foreach (var personObj in personResult.person)
            {
                person = personObj;
            }
            return await GenerateJwt(GenerateClaimsIdentity(person), person.Username, personResult);
        }
    }
}
