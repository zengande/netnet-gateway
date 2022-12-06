using Microsoft.AspNetCore.Components;

namespace NetNet.Gateway.Admin.Components;

public partial class DictionaryInput<TKey, TValue> where TKey : notnull
{
    [Parameter]
    public Dictionary<TKey, TValue>? Data { get; set; }
}
