﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SacramentPlanner.Models;

namespace SacramentPlanner.Data
{
    public class SacramentPlannerContext : DbContext
    {
        public SacramentPlannerContext (DbContextOptions<SacramentPlannerContext> options)
            : base(options)
        {
        }

        public DbSet<SacramentPlanner.Models.Meeting> Meeting { get; set; } = default!;
        public DbSet<SacramentPlanner.Models.Speaker> Speaker { get; set; } = default!;
        public DbSet<SacramentPlanner.Models.Topic> Topic { get; set; } = default!;
        public DbSet<SacramentPlanner.Models.Hymn> Hymn { get; set; } = default!;
        public DbSet<SacramentPlanner.Models.Member> Member { get; set; } = default!;
    }
}
