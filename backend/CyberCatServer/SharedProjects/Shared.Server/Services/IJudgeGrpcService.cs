using System.Threading.Tasks;
using ProtoBuf.Grpc.Configuration;
using Shared.Models.Models.Verdicts;
using Shared.Server.ProtoHelpers;

namespace Shared.Server.Services;

[Service]
public interface IJudgeGrpcService
{
    Task<Response<Verdict>> GetVerdict(GetVerdictArgs args);
}