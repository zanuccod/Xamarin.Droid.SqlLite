﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SqLitePcl.Common.Entities;
using SqLitePcl.Common.Models;

namespace SqLitePcl.Common.Test.Models
{
    [TestFixture]
    public class AuthorDataStoreSqlTest
    {
        private AuthorDataStoreSql db;
        private const string dbPath = "dbSqLiteTest";

        [SetUp]
        public void BeforeEachTest()
        {
            db = new AuthorDataStoreSql(dbPath + ".db3");
        }

        [TearDown]
        public void AfterEachTest()
        {
            // delete all database files generated for test
            var files = Directory.GetFiles(Path.GetDirectoryName(Path.GetFullPath(dbPath)), dbPath + ".*");
            foreach (var file in files)
                File.Delete(file);
        }

        [Test]
        public async Task InsertOneElement_Succes()
        {
            var item = new Author() { Name = "name", Surname = "surname", BornDate = "01-01-1970", Country = "TEST" };
            await db.AddItemAsync(item);

            Assert.AreEqual(1, db.GetItemsAsync().Result.Count);
        }

        [Test]
        public async Task UpdateElement_Succes()
        {
            var item = new Author() { Name = "name", Surname = "surname", BornDate = "01-01-1970", Country = "TEST" };
            await db.AddItemAsync(item);

            // reload element to get item with given Id
            item = db.GetItemsAsync().Result.FirstOrDefault();
            item.Name = "name1";
            await db.UpdateItemAsync(item);

            Assert.True(item.Equals(db.GetItemsAsync().Result.FirstOrDefault()));
        }

        [Test]
        public async Task DeleteElement_Success()
        {
            var item = new Author() { Name = "name", Surname = "surname", BornDate = "01-01-1970", Country = "TEST" };
            await db.AddItemAsync(item);

            // reload element to get item with given Id
            item = db.GetItemsAsync().Result.FirstOrDefault();
            await db.DeleteItemAsync(item);

            Assert.AreEqual(0, db.GetItemsAsync().Result.Count);
        }

        [Test]
        public async Task DeleteAllAsync_Success()
        {
            // Arrange
            var items = new List<Author>()
            {
                new Author() { Name = "name", Surname = "surname", BornDate = "01-01-1970", Country = "TEST" },
                new Author() { Name = "name1", Surname = "surname1", BornDate = "01-01-1970", Country = "TEST" },
                new Author() { Name = "name2", Surname = "surname2", BornDate = "01-01-1970", Country = "TEST" }
            };

            items.ForEach(x => db.AddItemAsync(x).ConfigureAwait(true));
            Assert.AreEqual(items.Count, db.GetItemsAsync().Result.Count);


            // Act
            await db.DeleteAllAsync();

            // Assert
            Assert.AreEqual(0, db.GetItemsAsync().Result.Count);
        }
    }
}
