public class OnItemCountChangedSignal : ISignal
{
    public string Id;
    public int Value;

    public OnItemCountChangedSignal(string id, int value)
    {
        Id = id;
        Value = value;
    }


}
