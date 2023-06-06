using System.Threading.Tasks;
using ApiGateway.Client.Tests.Core.Abstracts;
using NUnit.Framework;

namespace ApiGateway.Client.Tests.Core.Tests
{
    [TestFixture]
    public abstract class AuthenticateTests : ClientTests
    {
        [Test]
        public async Task Authenticate_WhenPassValidCredentials()
        {
            var token = await Client.Authenticate(TestEmail, TestUserPassword);
            Assert.IsNotEmpty(token);
        }

        [Test]
        public async Task AuthorizePlayer_WhenPassValidCredentials()
        {
            var token = await Client.Authenticate(TestEmail, TestUserPassword);
            var name = await Client.AuthorizePlayer(token);

            Assert.AreEqual(TestUserName, name);
        }

        [Test]
        public async Task ExistsDefaultUser_WhenPassValidCredentials()
        {
            var token = await Client.Authenticate("cyber@cat", "Cyber_Cat123@");
            var name = await Client.AuthorizePlayer(token);

            Assert.AreEqual("CyberCat", name);
        }
    }
}