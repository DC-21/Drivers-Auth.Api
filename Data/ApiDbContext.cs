using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DriverAuth.Api.Data;
public class ApiDbContext: IdentityDbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options)
    :base(options)
    {}
}
