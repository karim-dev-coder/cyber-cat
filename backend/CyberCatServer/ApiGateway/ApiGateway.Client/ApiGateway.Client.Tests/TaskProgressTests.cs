using System.Threading.Tasks;
using ApiGateway.Client.Application;
using ApiGateway.Client.Tests.Extensions;
using NUnit.Framework;
using Shared.Models.Domain.Verdicts;

namespace ApiGateway.Client.Tests
{
    public class TaskProgressTests : ApiGatewayClientTestFixture
    {
        private IApiGatewayClient _client;

        public TaskProgressTests(ServerEnvironment serverEnvironment) : base(serverEnvironment)
        {
        }

        [SetUp]
        public void SetUp()
        {
            _client = GetTestPlayerClient();
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
        }

        [Test]
        public async Task GetSavedCode_AfterSaveAndDeleteCode()
        {
            var taskId = "tutorial";
            var sourceCode = "#include <stdio.h>\nint main() { printf(\"Hello world!\"); }";
            var sourceCodeToChange = "#include <stdio.h>\nint main() { printf(\"Hello cat!\"); }";

            var task = _client.Player.Tasks[taskId];

            // We are checking that there is no code initially.
            Assert.IsFalse(task.IsStarted);

            // We are saving the code.
            await task.SubmitSolution(sourceCode);

            Assert.IsTrue(task.IsStarted);
            Assert.AreEqual(sourceCode, task.LastSolution);

            // We are modifying the saved code.
            await task.SubmitSolution(sourceCodeToChange);

            Assert.IsTrue(task.IsStarted);
            Assert.IsTrue(task.IsComplete);
            Assert.AreEqual(sourceCodeToChange, task.LastSolution);
        }

        [Test]
        public async Task ChangeTaskStatus_WhenSubmitSolution()
        {
            var taskId = "tutorial";
            var errorCode = "#include <stdio.h>\nint main() { printf(\"Unexpected output!\"); }";
            var completeCode = "#include <stdio.h>\nint main() { printf(\"Hello cat!\"); }";

            var task = _client.Player.Tasks[taskId];

            Assert.IsFalse(task.IsStarted);

            var result = await task.SubmitSolution(errorCode);
            Assert.IsTrue(result.Value.IsFailure);

            Assert.IsTrue(task.IsStarted);
            Assert.AreEqual(errorCode, task.LastSolution);

            result = await task.SubmitSolution(completeCode);
            Assert.IsTrue(result.Value.IsSuccess);
            Assert.IsTrue(task.IsStarted);
            Assert.IsTrue(task.IsComplete);
            Assert.AreEqual(completeCode, task.LastSolution);
        }

        [Test]
        public async Task CompleteTaskStatus_WhenPassCompleteSolutionOnStart()
        {
            var taskId = "tutorial";
            var completeCode = "#include <stdio.h>\nint main() { printf(\"Hello cat!\"); }";

            var task = _client.Player.Tasks[taskId];

            Assert.IsFalse(task.IsStarted);

            var result = await task.SubmitSolution(completeCode);
            Assert.IsTrue(result.Value.IsSuccess);
            Assert.IsTrue(task.IsStarted);
            Assert.IsTrue(task.IsComplete);
            Assert.AreEqual(completeCode, task.LastSolution);
        }
    }
}