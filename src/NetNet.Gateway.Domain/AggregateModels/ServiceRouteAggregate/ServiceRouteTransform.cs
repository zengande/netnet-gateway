using Volo.Abp.Domain.Values;

namespace NetNet.Gateway.AggregateModels.ServiceRouteAggregate;

public class ServiceRouteTransform : ValueObject
{
    public int GroupIndex { get; set; }

    public string Key { get; set; }

    public string Value { get; set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return GroupIndex;
        yield return Key;
        yield return Value;
    }
}
