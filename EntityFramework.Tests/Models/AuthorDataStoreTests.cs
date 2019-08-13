using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using SqLiteEntityFramework.Models;
using SqLiteEntityFramework.Entities;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SqLiteEntityFramework.Tests.Models
{
    [TestFixture]
    public class AuthorDataStoreTests
    {
        private DbContextOptions<EntityFrameworkBase<Author>> options;

        [SetUp]
        public void SetUp()
        {
            options = new DbContextOptionsBuilder<EntityFrameworkBase<Author>>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Test]
        public void AddItemAsync_Null_NothingToAdd()
        {
            Author item = null;

            using (var authorDataStore = new AuthorDataStore(options))
            {
                Task.FromResult(authorDataStore.AddItemAsync(item));
            }

            using (var authorDataStore = new AuthorDataStore(options))
            {
                Assert.AreEqual(0, authorDataStore.GetItemsAsync().Result.Count);
            }
        }

        [Test]
        public void AddItemAsync_Success()
        {
            var item = new Author()
            {
                Name = "name",
                Surname = "surname",
                BornDate = "01-01-1970",
                Country = "USA"
            };

            // Run the test against one instance of the context
            using (var authorDataStore = new AuthorDataStore(options))
            {
                Task.FromResult(authorDataStore.AddItemAsync(item));
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var authorDataStore = new AuthorDataStore(options))
            {
                Assert.AreEqual(1, authorDataStore.GetItemsAsync().Result.Count);
                Assert.True(item.Equals(authorDataStore.GetItemAsync(item.Id).Result));
            }
        }

        [Test]
        public void UpdateItemAsync_Null_NothingToUpdate()
        {
            Author item = null;

            // update item entry
            using (var authorDataStore = new AuthorDataStore(options))
            {
                Task.FromResult(authorDataStore.UpdateItemAsync(item));
            }

            // check for result
            using (var authorDataStore = new AuthorDataStore(options))
            {
                Assert.AreEqual(0, authorDataStore.GetItemsAsync().Result.Count);
            }
        }

        [Test]
        public void UpdateItemAsync_Success()
        {
            var item = new Author()
            {
                Name = "name",
                Surname = "surname",
                BornDate = "01-01-1970",
                Country = "USA"
            };
            const string expectedName = "name_update";
            const string expectedSurname = "surname_update";
            const string expectedBornDate = "01-01-2000";
            const string expectedConuntry = "JAPAN";

            // first add item
            using (var authorDataStore = new AuthorDataStore(options))
            {
                Task.FromResult(authorDataStore.AddItemAsync(item));
            }

            // update item values
            item.Name = expectedName;
            item.Surname = expectedSurname;
            item.BornDate = expectedBornDate;
            item.Country = expectedConuntry;

            // update item entry
            using (var authorDataStore = new AuthorDataStore(options))
            {
                Task.FromResult(authorDataStore.UpdateItemAsync(item));
            }

            // check for result
            using (var authorDataStore = new AuthorDataStore(options))
            {
                Assert.AreEqual(1, authorDataStore.GetItemsAsync().Result.Count);
                Assert.True(item.Equals(authorDataStore.GetItemAsync(item.Id).Result));
            }
        }

        [Test]
        public void DeleteItemAsync_Null_NothingToUpdate()
        {
            Author item = null;

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

        [Test]
        public void DeleteItemAsync_Success()
        {
            var item = new Author()
            {
                Name = "name",
                Surname = "surname",
                BornDate = "01-01-1970",
                Country = "USA"
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
