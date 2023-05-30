using ProtoBuf;

namespace Shared.Dto.Args;

[ProtoContract]
public sealed class LaunchCodeArgs
{
    [ProtoMember(1)] public string SourceCode { get; set; }
    [ProtoMember(2)] public string Input { get; set; }
}