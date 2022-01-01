using CoreWebApplication.Models;
using CoreWebApplication.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Moq;
using CoreWebApplication.Controllers;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging.Console;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;

namespace CoreWebApplication.UnitTests
{
    [TestClass]
    public class ItemControllerTests
    {

        private readonly Mock<List<IRepository>> listOfRepositoriesMock = new Mock<List<IRepository>>();

        private readonly Mock<IRepository> repositoryMock = new Mock<IRepository>();

        private readonly Mock<ILogger<ItemController>> loggerMock = new Mock<ILogger<ItemController>>();

        private ItemController controller;

        public static bool GetPredicate()
        {
            return true;
        }

        [TestInitialize]
        public void Setup()
        {           
            controller = new ItemController(repositoryMock.Object, loggerMock.Object);
        }

        [TestMethod]
        public async Task GetItemAsync_WithNonExistingItem_ReturnsNotFoundAsync()
        {
            //Arrage
            var id = Guid.NewGuid();
            repositoryMock
                .Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            //Act
            var actionResult  = await controller.GetItemAsync(Guid.NewGuid());

            //Assert
            var actualNotFound = actionResult.Result as NotFoundResult;
            Assert.AreEqual(404, actualNotFound.StatusCode);
        }

        [TestMethod]
        public async Task GetItemAsync_WithExistingItem_ReturnsItem()
        {
            //Arrage
            var id = Guid.NewGuid();
            var item = new Item
            {
                Id = id,
                Name = "apple",
                Price = 2344,
                CreatedDate = DateTimeOffset.UtcNow,
            };

            repositoryMock
                .Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => item);

            //Act
            var actionResult  = await controller.GetItemAsync(Guid.NewGuid());

            //Assert
            var actualOkItem = actionResult.Result as OkObjectResult;
            var actualItem = (Item)actualOkItem.Value;
            Assert.AreEqual(item.Name, actualItem.Name);

        }

        [TestMethod]
        public async Task GetItemsAsync_ReturnsListOf2Items()
        {
            //Arrage
            var id1 = Guid.NewGuid();
            var item1 = new Item
            {
                Id = id1,
                Name = "apple",
                Price = 2344,
                CreatedDate = DateTimeOffset.UtcNow,
            };
            var id2 = Guid.NewGuid();
            var item2 = new Item
            {
                Id = id2,
                Name = "apple",
                Price = 2344,
                CreatedDate = DateTimeOffset.UtcNow,
            };
            var expectedList = new List<Item>() { item1, item2 };
            repositoryMock
                .Setup(repo => repo.GetItemsAsync())
                .ReturnsAsync(() => expectedList);

            //Act
            var actionResult  = await controller.GetItemsAsync();

            //Assert
            var actualOkItem = actionResult.Result as OkObjectResult;
            var actualItems = (List<Item>)actualOkItem.Value;
            Assert.AreEqual(expectedList, actualItems);

        }
    }
}
