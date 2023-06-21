using ProtoBuf;
using Shared.Models.Models;

namespace Shared.Models.Dto
{
    [ProtoContract]
    public class VerdictDto : IVerdict
    {
        [ProtoMember(1)] public VerdictStatus Status { get; set; }
        [ProtoMember(2)] public string Error { get; set; }
        [ProtoMember(3)] public int TestsPassed { get; set; }
    }
}