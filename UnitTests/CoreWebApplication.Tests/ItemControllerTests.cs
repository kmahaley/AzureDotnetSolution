using CoreWebApplication.Controllers;
using CoreWebApplication.Models;
using CoreWebApplication.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CoreWebApplication.Tests
{
    public class ItemControllerTests
    {
        [Fact]
        public void GetItemAsync_WithNonExistingItem_ReturnsNotFound()
        {
            //Arrage
            var repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Item)null);
            
            var loggerMock = new Mock<ILogger<ItemController>>();

            var repositoriesMock = new Mock<IList<IRepository>>();
            repositoriesMock
                .Setup(repos => repos.FirstOrDefault(It.Is<IRepository>(Func<IRepository, bool>)))
                .Returns(repositoryMock);

            var controller = new ItemController(repositoriesMock.Object, loggerMock.Object);
            //Act

            //Assert
        }
    }
}
