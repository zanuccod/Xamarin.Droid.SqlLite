using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Collections.Generic;
using EF.Common.Entities;
using EF.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace EF.Common.Test.Models
{
    [TestFixture]
    public class AuthorDataStoreTests
    {
        private DbContextOptions<EFDataContext> options;
        private const string dbPath = "dbEFTest";
        private AuthorDataStore db;

#pragma warning disable IDE0051 // remove warning of unsed private members
        private IEnumerable<TestCaseData> TestCasesItems()
        {
            yield return new TestCaseData(
                new Author() { Name = "name", Surname = "surname", BornDate = "01-01-1970", Country = "TEST" },
                1);
        }

#pragma warning restore IDE0051

        [TestFixtureSetUp]
        public void BeforeAllTests()
        {
            options = new DbContextOptionsBuilder<EFDataContext>()
                .UseSqlite($"Filename={dbPath}.db3")
                .Options;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            db = new AuthorDataStore(options);
        }

        [TearDown]
        public void AfterEachTest()
        {
            // delete all database files generated for test
            var files = Directory.GetFiles(Path.GetDirectoryName(Path.GetFullPath(dbPath)), dbPath + ".*");
            foreach (var file in files)
                File.Delete(file);
        }

        [Test, TestCaseSource("TestCasesItems")]
        public async Task AddItemAsync_Success(Author item, int itemsCount)
        {
            // Act
            await db.AddItemAsync(item);

            // Assert
            Assert.AreEqual(itemsCount, db.GetItemsAsync().Result.Count);
        }

        [Test, TestCaseSource("TestCasesItems")]
        public async Task UpdateItemAsync_Success(Author item, int itemsCount)
        {
            // Act
            await db.UpdateItemAsync(item);

            // Assert
            Assert.AreEqual(itemsCount, db.GetItemsAsync().Result.Count);
        }

        [Test]
        public async Task DeleteItemAsync_Success()
        {
            // Arrange
            var item = new Author() { Name = "name", Surname = "surname", BornDate = "01-01-1970", Country = "TEST" };
            await db.AddItemAsync(item);
            Assert.AreEqual(1, db.GetItemsAsync().Result.Count);

            // Act
            await db.DeleteItemAsync(item);

            // Assert
            Assert.AreEqual(0, db.GetItemsAsync().Result.Count);
        }

        [Test]
        public async Task DeleteAllAsync_Success()
        {
            // Assert
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
