using System;
using System.IO;
using SQLite;
using SqLitePcl.Common.Entities;

namespace SqLitePcl.Common.Models
{
    public abstract class SqLiteBase : IDisposable
    {
        private const string databaseName = "dbSqLiteNetPcl.db";

        protected SQLiteAsyncConnection db;

        protected SqLiteBase(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath ?? GetDatabasePath());
            Init();
        }

        private void Init()
        {
            // create table if not exist
            db.CreateTableAsync<Author>();
        }

        public void Dispose()
        {
            db.CloseAsync();
            db = null;

            // Must be called as the disposal of the connection is not released until the GC runs.
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private string GetDatabasePath()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            return Path.Combine(documentsPath, databaseName);
        }
    }
}
