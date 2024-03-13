﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ThoBayMau_ASM.Models;

namespace ThoBayMau_ASM.Data
{
    public class ThoBayMau_ASMContext : DbContext
    {
        public ThoBayMau_ASMContext (DbContextOptions<ThoBayMau_ASMContext> options)
            : base(options)
        {
        }

        public DbSet<ThoBayMau_ASM.Models.SanPham> SanPham { get; set; } = default!;
    }
}