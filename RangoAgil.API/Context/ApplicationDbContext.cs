﻿using Microsoft.EntityFrameworkCore;
using RangoAgil.API.Entities;

namespace RangoAgil.API.Context
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        DbSet<Ingredient> Ingredients { get; set; } = null!;
        DbSet<Rango> Rangos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Rango>()
                .HasMany(r => r.Ingredients)
                .WithMany(i => i.Rangos)
                .UsingEntity(j => j.ToTable("rangoAndIngredient"));
        }
    }
}