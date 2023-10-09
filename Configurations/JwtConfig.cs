using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DriverAuth.Api.Configurations;
public class JwtConfig
{
    public string Secret{get;set;} = string.Empty;
}