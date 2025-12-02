public class ApplicationDbContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    // other DbSets...
}