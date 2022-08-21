public class OnItemCountChangedSignal : ISignal
{
    public EnumerableItem[] Items;

    public OnItemCountChangedSignal(EnumerableItem[] items)
    {
        Items = items;
    }
}
