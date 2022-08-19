public class OnInputDataRecievedSignal : ISignal
{
    public InputDataPack Data;

    public OnInputDataRecievedSignal(InputDataPack data)
    {
        Data = data;
    }
}
