using ProtoBuf;

namespace Shared.Dto;

[ProtoContract]
public class VerdictResponse
{
    [ProtoMember(1)] public VerdictStatus Status { get; set; }
    [ProtoMember(2)] public string Error { get; set; }
    [ProtoMember(3)] public string Output { get; set; }
}