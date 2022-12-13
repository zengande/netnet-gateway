using System.Collections;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NetNet.Gateway.SeedWork;

public class MetadataKeyValuePair
{
    public string Key { get; set; }
    public string? Value { get; set; }
}

public class Metadata : ValueObject, ICollection<MetadataKeyValuePair>, IEquatable<Metadata>
{
    private readonly List<MetadataKeyValuePair> _items;

    public Metadata(List<MetadataKeyValuePair> items)
    {
        _items = items;
    }

    public Metadata() : this(new(16))
    {
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        foreach (var item in _items)
        {
            yield return item.Key;
            yield return item.Value;
        }
    }

    public IEnumerator<MetadataKeyValuePair> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(MetadataKeyValuePair item)
    {
        Check.NotNullOrWhiteSpace(item.Key, nameof(MetadataKeyValuePair.Key));

        if (Contains(item))
        {
            throw new AbpException($"{item.Key} 已存在");
        }

        _items.Add(item);
    }

    public void Clear()
    {
        _items.Clear();
    }

    public bool Contains(MetadataKeyValuePair item)
    {
        return _items.Any(x => x.Key == item.Key);
    }

    public void CopyTo(MetadataKeyValuePair[] array, int arrayIndex)
    {
        _items.CopyTo(array, arrayIndex);
    }

    public bool Remove(MetadataKeyValuePair item)
    {
        return _items.Remove(item);
    }

    public int Count => _items.Count;
    public bool IsReadOnly => false;

    public bool Equals(Metadata? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return _items.SequenceEqual(other._items);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != this.GetType())
        {
            return false;
        }

        return Equals((Metadata)obj);
    }

    public override int GetHashCode()
    {
        return _items.GetHashCode();
    }

    public static implicit operator Dictionary<string, string?>(Metadata metadata)
    {
        return metadata._items.ToDictionary(x => x.Key, x => x.Value);
    }

    public static explicit operator Metadata(Dictionary<string, string?> dictionary)
    {
        return new(dictionary
            .Select(x => new MetadataKeyValuePair { Key = x.Key, Value = x.Value })
            .ToList());
    }

    public T GetValueOrDefault<T>(string key, T defaultValue)
    {
        var item = _items.FirstOrDefault(x => x.Key == key);
        if (item?.Value is not null)
        {
            return (T)Convert.ChangeType(item.Value, typeof(T));
        }

        return defaultValue;
    }
}
