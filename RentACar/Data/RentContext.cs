using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RentACar.Models;

namespace RentACar.Data
{
    public class RentContext:DbContext
    {
        public RentContext(DbContextOptions<RentContext> options) : base(options) { }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<PictureModel> Pictures { get; set; }


    }
}
