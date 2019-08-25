using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace EF.Common.Models
{
    public class EntityFrameworkBase<T> : DbContext where T : class
    {
        private const string databaseName = "dbEF.db";

        #region Constructors

        public EntityFrameworkBase()
        {
            Init();
        }

        public EntityFrameworkBase(DbContextOptions<EntityFrameworkBase<T>> options = null)
            : base(options)
        {
            Init();
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

        private void Init()
        {
            Database.EnsureCreated();
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
