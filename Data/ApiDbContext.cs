using System;
using Microsoft.EntityFrameworkCore;

namespace DriverAuth.Api.Data;
public class ApiDbContext: DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options)
    :base(options)
    {}
}
