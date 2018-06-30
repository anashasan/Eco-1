using Host.Business.IDbServices;
using Host.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Host.Business.DbServices
{
    public class AuditDbContext : EcoDbContext, IAuditDbContext
    {
        public AuditDbContext(DbContextOptions<EcoDbContext> options) : base(options)
        {
        }

        public int ApplicationUserId { get; set ; }

        public DateTime Now
        {
            get { return DateTime.Now; }
        }

        
        public override int SaveChanges()
        {

            // Perform the updates.
            try
            {
                return base.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries[0];
                Console.WriteLine(entry);
                Console.WriteLine(ex);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            

            // Perform the updates.
            try
            {
                return base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries[0];
                Console.WriteLine(entry);
                Console.WriteLine(ex);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
