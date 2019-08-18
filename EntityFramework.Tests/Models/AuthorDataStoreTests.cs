using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using SqLiteEntityFramework.Models;
using SqLiteEntityFramework.Entities;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Collections.Generic;

namespace SqLiteEntityFramework.Tests.Models
{
    [TestFixture]
    public class AuthorDataStoreTests
    {
        private DbContextOptions<EntityFrameworkBase<Author>> options;

#pragma warning disable IDE0051 // Rimuovi i membri privati inutilizzati
        private IEnumerable<TestCaseData> TestCasesItems()
        {
            yield return new TestCaseData(null, 0);
            yield return new TestCaseData(
                new Author() { Name = "name", Surname = "surname", BornDate = "01-01-1970", Country = "TEST" },
                1);
        }
#pragma warning restore IDE0051

        [SetUp]
        public void SetUp()
        {
            options = new DbContextOptionsBuilder<EntityFrameworkBase<Author>>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Test, TestCaseSource("TestCasesItems")]
        public void AddItemAsync_Success(Author item, int itemsCount)
        {
            // Run the test against one instance of the context
            using (var authorDataStore = new AuthorDataStore(options))
            {
                Task.FromResult(authorDataStore.AddItemAsync(item));
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var authorDataStore = new AuthorDataStore(options))
            {
                Assert.AreEqual(itemsCount, authorDataStore.GetItemsAsync().Result.Count);
            }
        }

        [Test, TestCaseSource("TestCasesItems")]
        public void UpdateItemAsync_Success(Author item, int itemsCount)
        {
            // update or insert item entry
            using (var authorDataStore = new AuthorDataStore(options))
            {
                Task.FromResult(authorDataStore.UpdateItemAsync(item));
            }

            // check for result
            using (var authorDataStore = new AuthorDataStore(options))
            {
                Assert.AreEqual(itemsCount, authorDataStore.GetItemsAsync().Result.Count);
            }
        }

        [Test]
        public void DeleteItemAsync_Success()
        {
            var item = new Author()
            {
                Name = "name",
                Surname = "surname",
                BornDate = "01-01-1970",
                Country = "TEST"
            };

            // first add item
            using (var authorDataStore = new AuthorDataStore(options))
            {
                Task.FromResult(authorDataStore.AddItemAsync(item));
            }

            // delete item entry
            using (var authorDataStore = new AuthorDataStore(options))
            {
                Task.FromResult(authorDataStore.DeleteItemAsync(item));
            }

            // check for result
            using (var authorDataStore = new AuthorDataStore(options))
            {
                Assert.AreEqual(0, authorDataStore.GetItemsAsync().Result.Count);
            }
        }
    }
}
