using System.Security.Claims;
using System.Text.Json;

namespace FrontCore.Helpers
{
    public static class JWTParser
    {
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(ParsearEnBase64SinMargen(jwt.Split('.')[1]));

            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value?.ToString())).ToList();
        }

        private static byte[] ParsearEnBase64SinMargen(string base64)
        {
            return Convert.FromBase64String(base64.PadRight(base64.Length + ((4 - base64.Length % 4) % 4), '='));
        }
    }
}
