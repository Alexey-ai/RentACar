using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Data
{
    public class RentAuthContext : IdentityDbContext
    {
        public RentAuthContext(DbContextOptions<RentAuthContext> options)
            : base(options)
        {
        }
    }
}
