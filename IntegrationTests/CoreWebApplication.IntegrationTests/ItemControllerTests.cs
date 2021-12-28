using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;

namespace CoreWebApplication.IntegrationTests
{
    [TestClass]
    public class ItemControllerTests
    {
        private readonly HttpClient client;

        public void Setup()
        {
            new WebApplicationFactory<Startup>();
        }

        [TestMethod]
        public void GetItemAsync_WithNonExistingItem_ReturnsNotFoundAsync()
        {
        }
    }
}
