public class OnItemCountChangedSignal : ISignal
{
    public string Id;
    public int Delta;

    public OnItemCountChangedSignal(string id, int value)
    {
        Id = id;
        Delta = value;
    }
}
