using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models;
using EF.Common.Entities;
using EF.Common.Models;
using Common.ViewModels;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace EF.Common.Test.ViewModels
{
    [TestFixture]
    public class ItemsViewModelTests
    {
        Mock<IDataStore<Author>> mockAuthorModel;
        private IDataStore<Author> authorModel;
        private ItemsViewModel<Author> viewModel;
        DbContextOptions<EntityFrameworkBase<Author>> options;

        [TestFixtureSetUp]
        public void Init()
        {
            // create dataStore with "memory" connection option to test Add and then read operations
            options = new DbContextOptionsBuilder<EntityFrameworkBase<Author>>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [SetUp]
        public void Setup()
        {
            mockAuthorModel = new Mock<IDataStore<Author>>();
            authorModel = mockAuthorModel.Object;

            viewModel = new ItemsViewModel<Author>(authorModel);
        }

        [Test]
        public void Constructor_NotNullElements_Success()
        {
            // Arrange
            viewModel = new ItemsViewModel<Author>(authorModel);

            // Assert
            Assert.NotNull(viewModel.Items);
            Assert.AreEqual(0, viewModel.Items.Count);
            Assert.NotNull(viewModel.AddItemCommand);
            Assert.NotNull(viewModel.LoadItemsCommand);
            Assert.False(viewModel.IsBusy);
        }

        [Test]
        public void LoadItemsCommand_ReadTwoElements_Success()
        {
            // Arrange
            var expectedResult = new List<Author>
            {
                new Author() { Name = "name", Surname = "surname", BornDate = "01-01-1970", Country = "USA" },
                new Author() { Name = "name_2", Surname = "surname_2", BornDate = "02-02-1972", Country = "JAPAN" }
            };

            // mock expected result of GetItemsAsync() method
            mockAuthorModel.Setup(x => x.GetItemsAsync()).Returns(Task.FromResult(expectedResult));

            // Act
            viewModel.LoadItemsCommand.Execute(null);

            // Assert
            Assert.AreEqual(expectedResult, viewModel.Items);
            Assert.False(viewModel.IsBusy);
        }

        [Test]
        public void AddItemAsync_AddItemThenRead_Success()
        {
            // Arrange
            authorModel = new AuthorDataStore(options);
            viewModel = new ItemsViewModel<Author>(authorModel);

            var expectedResult = new Author() { Name = "name", Surname = "surname", BornDate = "01-01-1970", Country = "USA" };

            // Act
            viewModel.AddItemCommand.Execute(expectedResult);

            // Assert
            // reload items from database and check that first element is equal to the inserted
            viewModel.LoadItemsCommand.Execute(null);

            Assert.True(expectedResult.Equals(viewModel.Items.First()));
            Assert.False(viewModel.IsBusy);
        }

        [Test]
        public void DeleteAllAsync_AddItemsAndDeleteAll_Success()
        {
            // Arrange
            authorModel = new AuthorDataStore(options);
            viewModel = new ItemsViewModel<Author>(authorModel);

            var items = new List<Author>()
            {
                new Author() { Name = "name", Surname = "surname", BornDate = "01-01-1970", Country = "TEST" },
                new Author() { Name = "name1", Surname = "surname1", BornDate = "01-01-1970", Country = "TEST" },
                new Author() { Name = "name2", Surname = "surname2", BornDate = "01-01-1970", Country = "TEST" }
            };
            items.ForEach(x => viewModel.AddItemCommand.Execute(x));
            Assert.AreEqual(items.Count, viewModel.Items.Count);

            // Act
            viewModel.DeleteAllCommand.Execute(null);

            // Assert

            // items list of the view model should be empty
            Assert.AreEqual(0, viewModel.Items.Count);

            // reload items from database to check that table is empty
            viewModel.LoadItemsCommand.Execute(null);
            Assert.AreEqual(0, viewModel.Items.Count);
            Assert.False(viewModel.IsBusy);
        }
    }
}
