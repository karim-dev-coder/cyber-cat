using ProtoBuf;

namespace Shared.Dto;

[ProtoContract]
public class ListProto<T>
{
    [ProtoMember(1)] public List<T> Values { get; set; } = new();

    public List<T>.Enumerator GetEnumerator()
    {
        return Values.GetEnumerator();
    }

    public static implicit operator List<T>(ListProto<T> proto)
    {
        return proto.Values;
    }

    public static implicit operator ListProto<T>(List<T> values)
    {
        return new ListProto<T>
        {
            Values = values
        };
    }
}