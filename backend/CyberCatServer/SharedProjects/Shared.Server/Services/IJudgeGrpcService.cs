using System.Threading.Tasks;
using ProtoBuf.Grpc.Configuration;
using Shared.Models.Dto;

namespace Shared.Server.Services;

[Service]
public interface IJudgeGrpcService
{
    Task<VerdictDto> GetVerdict(SolutionDto solution);
}