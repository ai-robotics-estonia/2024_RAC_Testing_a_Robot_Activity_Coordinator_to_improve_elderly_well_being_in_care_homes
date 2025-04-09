namespace WebApp.Areas.Admin.ViewModels;

public class KeyValue<TKey, TValue>
{
    public TKey Key { get; set; } = default!;
    public TValue Value { get; set; } = default!;
}