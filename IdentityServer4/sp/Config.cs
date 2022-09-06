// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using IdentityServer4;
using Rsk.Saml;
using Rsk.Saml.Models;

namespace sp
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
                new ApiResource("api1", "My API #1")
            };
        }

        public static string GetLicenseKey() {
            return "eyJTb2xkRm9yIjowLjAsIktleVByZXNldCI6NiwiU2F2ZUtleSI6ZmFsc2UsIkxlZ2FjeUtleSI6ZmFsc2UsIlJlbmV3YWxTZW50VGltZSI6IjAwMDEtMDEtMDFUMDA6MDA6MDAiLCJhdXRoIjoiREVNTyIsImV4cCI6IjIwMjItMTAtMDZUMDE6MjE6MjYuNjQzNTUxMiswMDowMCIsImlhdCI6IjIwMjItMDktMDZUMDE6MjE6MjYiLCJvcmciOiJERU1PIiwiYXVkIjoyfQ==.pWTdA5V5dkexQYRhs89Vjfj7UK2sDMc/STCPfx4hV1H5jLXyDOJtlW9PoxlhpdPjeIno/SVUYXmYePYTlQD48oaXGdl/+mLgX1UnP8tpPHO1Cpk+fLJz4b/6YWmj1efIAhok5OvETKdODvqHwZkSS+c0OVOBJqWJZ7hWImZ31evGOIb7bgQkWm8uRRSHQHt8RE2hCQ5o2zRnvI7Eu8vbsPHI4nRg3sykwsZe3XdxUW4L419c52lsLl/8hm73bTtg1zlfixWd/zWVhCTfVuDAzzjcOFQSqVc7tMOvRhRob4+/6+EjOtTi+K+rZHoFp6EjGPhtP3KiBJdUD0g730+8l1v23lUVuvmilWQ5qCqD6kNDvtE+WDB+tdevKEHsry0kLkiTlVlOV36l8Bh+j6UPhrLDxhRt6r4csDz/xTLd8bwyEaWGmaD3zKUJOGYEqWd8ffGPjZOTiwryhhesB8Z2IzmmEBkykMlFhbMr+rximkbkEKClalVKSNjNapopLX3yvMuYFuQTWDWAg4eEpGKHaohURPEf3UALC0mtHnWOg4JW2MhlRav8rg2kLQXRy2ys+7Fqj+ovrmE+f9+8wWP5q3lXPODjwAb7iFFzASUrsH0cH1owYis2Ff7Plam9ZDNHGYLryOP6gLOAh3h2qDcINl90VJVV/CEwF8+2inp+OOc=";
        }

        public static string GetLicensee() {
            return "DEMO";
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                // client credentials flow client
                new Client
                {
                    ClientId = "client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = {new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256())},

                    AllowedScopes = {"api1"}
                },

                // MVC client using hybrid flow
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",

                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    ClientSecrets = {new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256())},

                    RedirectUris = {"https://localhost:5001/signin-oidc"},
                    FrontChannelLogoutUri = "https://localhost:5001/signout-oidc",
                    PostLogoutRedirectUris = {"https://localhost:5001/signout-callback-oidc"},

                    AllowOfflineAccess = true,
                    AllowedScopes = {"openid", "profile", "api1"}
                },

                // SPA client using implicit flow
                new Client
                {
                    ClientId = "spa",
                    ClientName = "SPA Client",
                    ClientUri = "https://identityserver.io",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris =
                    {
                        "https://localhost:5002/index.html",
                        "https://localhost:5002/callback.html",
                        "https://localhost:5002/silent.html",
                        "https://localhost:5002/popup.html",
                    },

                    PostLogoutRedirectUris = {"https://localhost:5002/index.html"},
                    AllowedCorsOrigins = {"https://localhost:5002"},

                    AllowedScopes = {"openid", "profile", "api1"}
                },

                // SAML client
                new Client
                {
                    ClientId = "https://localhost:5001/saml",
                    ClientName = "RSK SAML2P Test Client",
                    ProtocolType = IdentityServerConstants.ProtocolTypes.Saml2p,
                    AllowedScopes = {"openid", "profile"}
                },
                new Client
                {
                    ClientId = "https://localhost:5002/saml",
                    ClientName = "RSK SAML2P Test Client - Multiple SP",
                    ProtocolType = IdentityServerConstants.ProtocolTypes.Saml2p,
                    AllowedScopes = {"openid", "profile"}
                }
            };
        }
        public static IEnumerable<ServiceProvider> GetServiceProviders()
        {
            return new[]
            {
                new ServiceProvider
                {
                    EntityId = "https://localhost:5001/saml",
                    AssertionConsumerServices =
                        {new Service(SamlConstants.BindingTypes.HttpPost, "https://localhost:5001/signin-saml")},
                    SigningCertificates = {new X509Certificate2("testclient.cer")}
                },
                new ServiceProvider
                {
                    EntityId = "https://localhost:5002/saml",
                    AssertionConsumerServices =
                        {new Service(SamlConstants.BindingTypes.HttpPost, "https://localhost:5002/signin-saml-2")}
                }
            };
        }
    }
}