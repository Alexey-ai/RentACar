using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentACar.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Data
{
    public class RentAuthContext : IdentityDbContext<RentUser>
    {
        public RentAuthContext(DbContextOptions<RentAuthContext> options)
            : base(options)
        {
        }
    }
}
