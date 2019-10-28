using System;
using System.IO;
using EF.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace EF.Common.Models
{
    public class EFDataContext : DbContext
    {
        private const string databaseName = "dbEF.db";

        #region Constructors

        public EFDataContext()
        {
            Init();
        }

        public EFDataContext(DbContextOptions options)
            : base(options)
        {
            Init();
        }

        #endregion

        #region Public Propeties

        public DbSet<Author> Authors { get; set; }

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