﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiResource> Apis =>
            new[] {
                new ApiResource("socialnetwork", "Social Network")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client() {
                     ClientId = "TrustedInternalApi",
                    ClientSecrets = new [] { new Secret("secret".Sha256()) },
                     AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new [] {
                        IdentityServerConstants.StandardScopes.OpenId,
        IdentityServerConstants.StandardScopes.Profile,
        IdentityServerConstants.StandardScopes.Email,
                        "socialnetwork" }
                },
                new Client() {
                     ClientId = "UntrustedExternalClientBrowser",
                     ClientName = "Untrusted External Client Browser",
                     AllowedGrantTypes = GrantTypes.Code,
                     RequireConsent = false,
                    AllowRememberConsent = false,
                    AllowAccessTokensViaBrowser = true,
                    RequireClientSecret = false,
                    RequirePkce = true,
                    RedirectUris =
                      new List<string> {
                          "https://localhost:44339/",
                          "http://localhost:4200/auth-callback"
                      },
                    PostLogoutRedirectUris =
                      new List<string> {
                          "http://localhost:4200/"
                      },
                    AllowedScopes = new [] {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            IdentityServerConstants.StandardScopes.Email,
                            "socialnetwork"
                    },
                    AccessTokenType = AccessTokenType.Jwt,
                    AllowedCorsOrigins =
                    {
                        "http://localhost:4200"
                    },
                     
            }
        };

        public static IEnumerable<TestUser> Users()
        {
            return new[] {
                new TestUser{SubjectId = "818727", Username = "alice", Password = "alice",
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "Alice Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Alice"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
                }
            }
            };
        }
    }
}