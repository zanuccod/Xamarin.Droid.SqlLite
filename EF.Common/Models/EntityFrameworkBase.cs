using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace EF.Common.Models
{
    public class EntityFrameworkBase<T> : DbContext where T : class
    {
        private const string databaseName = "dbEF.db";

        // used for unit test: to have always same context for queries
        private readonly DbContextOptions<EntityFrameworkBase<T>> options;

        #region Constructors

        public EntityFrameworkBase()
        { }

        public EntityFrameworkBase(DbContextOptions<EntityFrameworkBase<T>> options)
            : base(options)
        {
            this.options = options;
        }

        #endregion

        #region Public Propeties

        public DbSet<T> Table { get; set; }

        #endregion

        #region Public Methods

        public override void Dispose()
        {
            base.Dispose();
            Table = null;
        }

        #endregion

        #region Private Methods

        protected EntityFrameworkBase<T> CreateContext()
        {
            var db = options != null ?  new EntityFrameworkBase<T>(options) : new EntityFrameworkBase<T>();
            db.Database.EnsureCreated();

            return db;
        }

        #endregion

        #region OnConfiguring

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), databaseName);

                // Specify that we will use sqlite and the path of the database here
                optionsBuilder.UseSqlite($"Filename={databasePath}");
            }
        }

        #endregion
    }
}
