using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PlayerService.GrpcServices;
using ProtoBuf.Grpc.Client;
using Shared.Models.Dto.Args;
using Shared.Server.Exceptions;
using Shared.Server.Services;
using Shared.Tests;

namespace PlayerService.Tests;

[TestFixture]
public class BasicTests
{
    private WebApplicationFactory<Program> _factory;
    
    [SetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
    }

    [Test]
    public async Task CheckAddAndDeleteNewPlayer()
    {
        using var channel = _factory.CreateGrpcChannel();
        var service = channel.CreateGrpcService<IPlayerGrpcService>();

        var playerArgs = new PlayerIdArgs { PlayerId = 1234567 };
        
        var ex = Assert.ThrowsAsync<Grpc.Core.RpcException>(async () => await service.GetPlayerById(playerArgs));

        Assert.That(ex.Message, Is.EqualTo("Status(StatusCode=\"Unknown\", Detail=\"Exception was thrown by handler. PlayerNotFoundException: Player with UserId '1234567' not found\")"));

        await service.AddNewPlayer(playerArgs);
        
        var newPlayer = await service.GetPlayerById(playerArgs);
        
        Assert.IsNotNull(newPlayer);
        Assert.That(newPlayer.UserId, Is.EqualTo(1234567));

        await service.DeletePlayer(playerArgs);
        
        ex = Assert.ThrowsAsync<Grpc.Core.RpcException>(async () => await service.GetPlayerById(playerArgs));
        Assert.That(ex.Message, Is.EqualTo("Status(StatusCode=\"Unknown\", Detail=\"Exception was thrown by handler. PlayerNotFoundException: Player with UserId '1234567' not found\")"));
        
    }

    [Test]
    public async Task AddAndReturnNewPlayer()
    {
        using var channel = _factory.CreateGrpcChannel();
        var service = channel.CreateGrpcService<IPlayerGrpcService>();

        var playerArgs = new PlayerIdArgs { PlayerId = 1234567 };

        await service.AddNewPlayer(playerArgs);

        var newPlayer = await service.GetPlayerById(playerArgs);

        Assert.IsNotNull(newPlayer);
        Assert.That(newPlayer.UserId, Is.EqualTo(1234567));

        await service.DeletePlayer(playerArgs);
    }

    [Test]
    public async Task CreatePlayerAndAddSomeBtc()
    {
        using var channel = _factory.CreateGrpcChannel();
        var service = channel.CreateGrpcService<IPlayerGrpcService>();
        
        var createPlayerArgs = new PlayerIdArgs { PlayerId = 1234567 };

        await service.AddNewPlayer(createPlayerArgs);
        
        var newPlayer = await service.GetPlayerById(createPlayerArgs);
        
        Assert.IsNotNull(newPlayer);
        Assert.That(newPlayer.UserId, Is.EqualTo(1234567));

        var addBtcPlayerArgs = new PlayerAddBtcArgs { PlayerId = createPlayerArgs.PlayerId, BitcoinsCount = 1000 };

        await service.AddBitcoinsToPlayer(addBtcPlayerArgs);
        
        newPlayer = await service.GetPlayerById(createPlayerArgs);
        Assert.IsNotNull(newPlayer);
        Assert.That(newPlayer.UserId, Is.EqualTo(1234567));
        Assert.That(newPlayer.BitcoinCount, Is.EqualTo(1000));

        await service.DeletePlayer(createPlayerArgs);

    }
}