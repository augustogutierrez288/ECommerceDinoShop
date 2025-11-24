using Blazored.LocalStorage;
using ECommerceDinoShop.DTO;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;


namespace ECommerceDinoShop.WebAssembly.Extensions
{
    public class AuthenticationExtension : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private ClaimsPrincipal noInformation = new ClaimsPrincipal(new ClaimsIdentity());

        public AuthenticationExtension(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task UpdateAuthenticationStatus(SesionDTO sesionUser)
        {
            ClaimsPrincipal claimsPrincipal;

            if (sesionUser != null)
            {
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, sesionUser.IdUser.ToString()),
                    new Claim(ClaimTypes.Name, sesionUser.FullName),
                    new Claim(ClaimTypes.Email, sesionUser.Email),
                    new Claim(ClaimTypes.Role, sesionUser.Role)

                }, "JwtAuth"));

                await _localStorage.SetItemAsync("sesionUsuario", sesionUser);
            }
            else
            {
                claimsPrincipal = noInformation;
                await _localStorage.RemoveItemAsync("sesionUsuario");
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var sesionUser = await _localStorage.GetItemAsync<SesionDTO>("sesionUsuario");

            if (sesionUser == null)
                return await Task.FromResult(new AuthenticationState(noInformation));

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, sesionUser.IdUser.ToString()),
                    new Claim(ClaimTypes.Name, sesionUser.FullName),
                    new Claim(ClaimTypes.Email, sesionUser.Email),
                    new Claim(ClaimTypes.Role, sesionUser.Role)

                }, "JwtAuth"));

            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }

    }
}
