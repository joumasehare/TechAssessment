﻿using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace AcmeProduct.Data;

public class DataContext(DbContextOptions contextOptions) : DbContext(contextOptions)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(DataContext)));
        base.OnModelCreating(modelBuilder);
    }
}