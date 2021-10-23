using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.IdentityModel;
using System.Globalization;

namespace IdentityServer.Client1.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            //User.Claims olarak alabilirsin.
            return View();
        }

        public async Task LogOut()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");

            //User.Claims olarak alabilirsin.
            //   return View();
        }

        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            HttpClient httpClient = new HttpClient();
            RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest();
            var disco = await httpClient.GetDiscoveryDocumentAsync("https://localhost:44397");

            refreshTokenRequest.ClientId = "Client1-Mvc";
            refreshTokenRequest.ClientSecret = "secret";
            refreshTokenRequest.RefreshToken = refreshToken;
            refreshTokenRequest.Address = disco.TokenEndpoint;

            var token = await httpClient.RequestRefreshTokenAsync(refreshTokenRequest);
            if (token.IsError)
            {
                //yönlendirme
            }
            var tokens = new List<AuthenticationToken>()
            { new AuthenticationToken {

                Name=OpenIdConnectParameterNames.IdToken,Value=token.IdentityToken,

            },new AuthenticationToken {

                Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken,

            },new AuthenticationToken {

                Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken,

            },new AuthenticationToken {

                Name=OpenIdConnectParameterNames.ExpiresIn,Value=DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)

            } };

            var authResult = await HttpContext.AuthenticateAsync();
            var properties = authResult.Properties;

            properties.StoreTokens(tokens);
            await HttpContext.SignInAsync("Cookies", authResult.Principal, properties);

            return RedirectToAction("Index");
        }
    }
}
