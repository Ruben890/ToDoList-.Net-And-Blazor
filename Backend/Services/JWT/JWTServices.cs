using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Services.JWT
{
    public class JWTServices
    {
        private readonly string _jwtToken;

        // Constructor que recibe el token JWT como parámetro
        public JWTServices(string jwtToken)
        {
            _jwtToken = jwtToken;
        }

        // Método para crear un token JWT con datos específicos
        public string CreateJwt(string data)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            
            // Codifica el token JWT en bytes
            var byteKey = Encoding.UTF8.GetBytes(_jwtToken);

            // Configura los detalles del token JWT
            var tokenDes = new SecurityTokenDescriptor
            {
                // Especifica los claims que se incluirán en el token
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, data) // Claim con el email proporcionado
                }),

                // Define la fecha de expiración del token (1 día desde ahora)
                Expires = DateTime.UtcNow.AddDays(1),

                // Especifica la clave y el algoritmo de firma para el token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(byteKey),
                                                            SecurityAlgorithms.HmacSha256Signature),
            };

            // Crea el token JWT basado en los detalles especificados
            var token = tokenHandler.CreateToken(tokenDes);

            // Devuelve el token JWT como una cadena
            return tokenHandler.WriteToken(token);
        }
    }
}
