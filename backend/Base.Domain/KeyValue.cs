namespace Base.Domain;

public class KeyValue : KeyValue<Guid, string>
{
}

public class KeyValue<TValue> : KeyValue<Guid, TValue>
{
}

public class KeyValue<TKey, TValue>
{
    public TKey Key { get; set; } = default!;
    public TValue Value { get; set; } = default!;
}


