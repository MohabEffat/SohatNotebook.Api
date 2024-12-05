﻿using Microsoft.Extensions.Options;
using SohatNotebook.Authentication.Configuration.Options;

namespace SohatNotebook.Api.Configuration.OptionsSetup
{
    public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
    {
        private readonly IConfiguration _configuration;

        public JwtOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Configure(JwtOptions options)
        {
            _configuration.GetSection("JWT").Bind(options);
        }
    }
}
