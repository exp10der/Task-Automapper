using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;


namespace TaskAutomapper.data
{
    class SqliteContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Position> Positions { get; set; }

        private readonly string _dbPath;

        public SqliteContext(string path)
            : base(new SQLiteConnection
            {
                ConnectionString = new SQLiteConnectionStringBuilder
                {
                    DataSource = path,
                    ForeignKeys = true,
                    BinaryGUID = false,
                }.ConnectionString
            }, true)
        {
            _dbPath = path;
            Database.Log = Console.Write;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            Database.SetInitializer(new SqliteContextInitializer<SqliteContext>(_dbPath, modelBuilder));
        }
    }
}
