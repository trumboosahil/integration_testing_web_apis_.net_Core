﻿using Microsoft.EntityFrameworkCore;


namespace IntegrationTesting.Web.Model
{
    public class CustomerDbContext: DbContext
    {
       
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; } = null!;
    }
}
