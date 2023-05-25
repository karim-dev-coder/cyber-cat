using Microsoft.Extensions.Options;
using MongoDbGenericRepository;
using Shared.Models;
using TaskService.Repositories.InternalModels;

namespace TaskService.Repositories;

public class TestMongoRepository : BaseMongoRepository<string>, ITestRepository
{
    public TestMongoRepository(IOptions<TaskServiceAppSettings> appSettings)
        : base(appSettings.Value.MongoRepository.ConnectionString, appSettings.Value.MongoRepository.DatabaseName)
    {
    }

    public async Task<List<ITest>?> GetTests(string taskId)
    {
        var task = await GetOneAsync<TaskModel>(task => task.Id == taskId);
        return task.Tests.Select(t => (ITest) new TestChallenge
        {
            Input = t.Input,
            ExpectedOutput = t.ExpectedOutput
        }).ToList();
    }
}