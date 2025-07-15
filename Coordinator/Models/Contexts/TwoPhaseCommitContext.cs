using Microsoft.EntityFrameworkCore;

namespace Coordinator.Models.Contexts
{
    public class TwoPhaseCommitContext:DbContext
    {
        public TwoPhaseCommitContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Node> Nodes { get; set; }
        public DbSet<NodeState> NodeStates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Node>().HasData(
             new Node("OrderAPI") { Id = Guid.Parse("ee9ab40b-cc1f-419c-a758-1f993157008f") },
             new Node("Payment.API") { Id = Guid.Parse("62648b9e-afa0-40f1-88dc-4c8787c1cbea") },
             new Node("Mail.API") { Id = Guid.Parse("4dcb74a2-39f1-4021-a3f8-a44153e14d37") },
             new Node("Stock.API") { Id = Guid.Parse("5a3a9a4b-8baf-4875-939a-ba3eafef63fd") }
            );
        }
    }
}
