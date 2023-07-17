using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace BlazorWalletTrackerFE.Shared
{
    public class CustomAuthProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _localStorage;

        public CustomAuthProvider(IJSRuntime localStorage)
        {
            _localStorage = localStorage;
        }




        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var accessToken = await _localStorage.InvokeAsync<string>("localStorage.getItem", "JWTToken");

            if (string.IsNullOrEmpty(accessToken))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            else
            {
                var claims = ParseClaimsFromJwt(accessToken);
                var identity = new ClaimsIdentity(claims, "jwtAuth");
                var user = new ClaimsPrincipal(identity);
                return new AuthenticationState(user);
            }
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];



            var jsonBytes = ParseBase64WithoutPadding(payload);



            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            claims.AddRange(keyValuePairs?.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            return claims;
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
        public void NotifyAuthState()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }




        //public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        //{
        //    var accessToken = await _localStorage.InvokeAsync<string>("localStorage.getItem", "JWTToken");

        //    if (string.IsNullOrEmpty(accessToken))
        //    {
        //        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        //    }

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var token = tokenHandler.ReadJwtToken(accessToken);
        //    var identity = new ClaimsIdentity(token.Claims, "Bearer");
        //    var user = new ClaimsPrincipal(identity);
        //    return new AuthenticationState(user);
        //}
        //public void NotifyAuthenticationStateChangedWrapper(Task<AuthenticationState> task)
        //{
        //    NotifyAuthenticationStateChanged(task);
        //}
        //public void MarkUserAsAuthenticated(string email)
        //{
        //    var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, email) }, "apiauth"));
        //    var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        //    NotifyAuthenticationStateChanged(authState);
        //}

        //public void MarkUserAsLoggedOut()
        //{
        //    var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        //    var authState = Task.FromResult(new AuthenticationState(anonymousUser));
        //    NotifyAuthenticationStateChanged(authState);
        //}

        //public void MarkUserAsLoggedOut(string email)
        //{
        //    var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, email) }, "apiauth"));
        //    var authState = Task.FromResult(new AuthenticationState(anonymousUser));
        //    NotifyAuthenticationStateChanged(authState);
        //}

        //public void MarkUserAsLoggedOut(ClaimsPrincipal user)
        //{
        //    var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        //    var authState = Task.FromResult(new AuthenticationState(anonymousUser));
        //    NotifyAuthenticationStateChanged(authState);
        //}


    }
}
