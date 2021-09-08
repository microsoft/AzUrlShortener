using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeaconServices.VINDecoderService.FunctionsApp
{
    public class OpenApiConfigurationOptions : DefaultOpenApiConfigurationOptions
    {
        public override OpenApiInfo Info { get; set; } = new OpenApiInfo()
        {
            Version = "1.0.0",
            Title = "Beacon URL Shortener",
            Description = @"### Overview
The Beacon URL Shortener API provides a simple way to turn a long url into a short one. View and test the API methods at the bottom of this page.

An API key is required to use this API. Enter your API key in authorize/api key box to enable testing on this page.

### URL Redirect

A short url is provided as part of the response to the /UrlShortener endpoint. To manually reconstruct a short url, append the vanity to the domain name: `https://beac.co/{vanity}`. [Example short url](https://beac.co/da)

Custom domains names can be configured upon request."
        };

        public override List<OpenApiServer> Servers { get; set; } = new List<OpenApiServer>()
        {
            new OpenApiServer() { Url = "https://beac.co/api/", Description = "Production" },
        };

        public override OpenApiVersionType OpenApiVersion { get; set; } = OpenApiVersionType.V3;

    }
}
