using ProtoBuf;

namespace Shared.Models.Dto.Args
{
    [ProtoContract]
    public class GetAccessTokenArgs
    {
        [ProtoMember(1)] public string Email { get; set; }
        [ProtoMember(2)] public string Password { get; set; }
    }
}