using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.AuthServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>(){

                new ApiResource("resource_api1"){ Scopes={ "api1.read", "api1.write", "api1.update" } },
                new ApiResource("resource_api2"){ Scopes={ "api1.read", "api1.write", "api1.update" } }
        };
        }
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>() {
                new ApiScope("api1.read","API 1 için okuma izni"),
                new ApiScope("api1.write","API 1 için yazma izni"),
                new ApiScope("api1.update","API 1 için güncelleme izni"),
                new ApiScope("api2.read","API 2 için okuma izni"),
                new ApiScope("api2.write","API 2 için yazma izni"),
                new ApiScope("api2.update","API 2 için güncelleme izni")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client(){
                ClientId="Client1",
                ClientName="Client 1 app uygulama",
                ClientSecrets=new[]{new Secret("secret".Sha256())},
                AllowedGrantTypes=GrantTypes.ClientCredentials,
                AllowedScopes= {"api1.read","api2.write","api2.update" }
                },
                new Client(){
                ClientId="Client2",
                ClientName="Client 2 app uygulama",
                ClientSecrets=new[]{new Secret("secret".Sha256())},
                AllowedGrantTypes=GrantTypes.ClientCredentials,
                AllowedScopes= {"api1.read","api2.write","api2.update" }
                },
                new Client(){
                ClientId="Client1-Mvc",
                RequirePkce=false,
                ClientName="Client MVC uygulama",
                ClientSecrets=new[]{new Secret("secret".Sha256())},
                AllowedGrantTypes=GrantTypes.Hybrid,
                RedirectUris=new List<string> { "https://localhost:44357/signin-oidc" }, //open id protolleri yüzündenmiş
                AllowedScopes= {IdentityServerConstants.StandardScopes.OpenId,                    IdentityServerConstants.StandardScopes.Profile,"api1.read",
                  IdentityServerConstants.StandardScopes.OfflineAccess},
                PostLogoutRedirectUris=new List<string>
                {
                    "https://localhost:44357/signout-callback-oidc"
                },
                 AccessTokenLifetime=2*60*60,
                    AllowOfflineAccess=true,//refresh token için
                    RefreshTokenUsage=TokenUsage.ReUse, //hep kullansın
                    RefreshTokenExpiration=TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds//kesin ömür veriyor -- 60 günlük
                },
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        }

        public static IEnumerable<TestUser> GetUsers()
        {
            return new List<TestUser>() {
            new TestUser {SubjectId="1",Username="ozerfa",Password="password",Claims=new List<Claim>(){
            new Claim("given_name","fuat"),
            new Claim("family_name","ozer")
            } } ,
            new TestUser {SubjectId="2",Username="ozerfatih",Password="password",Claims=new List<Claim>(){
            new Claim("given_name","fatih"),
            new Claim("family_name","ozer")
            } }
            };
        }
    }
}
