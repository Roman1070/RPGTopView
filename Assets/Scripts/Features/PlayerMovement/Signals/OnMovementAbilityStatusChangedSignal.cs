public class OnMovementAbilityStatusChangedSignal: ISignal
{ 
    public bool Available;

    public OnMovementAbilityStatusChangedSignal(bool available)
    {
        Available = available;
    }
}
