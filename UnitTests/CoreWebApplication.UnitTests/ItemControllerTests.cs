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
            listOfRepositoriesMock
                .Setup(repoList => repoList.FirstOrDefault(It.IsAny<Func<IRepository, bool>>()))
                .Returns(repositoryMock.Object);

            controller = new ItemController(listOfRepositoriesMock.Object, loggerMock.Object);
            //controller = new ItemController(repositoryMock.Object, loggerMock.Object);
        }

        [TestMethod]
        public async Task GetItemAsync_WithNonExistingItem_ReturnsNotFoundAsync()
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
            var result = await controller.GetItemAsync(Guid.NewGuid());
            var actualItem = result.Value;
            //Assert
            Assert.AreEqual(item.Name, actualItem.Name);

        }
    }
}
