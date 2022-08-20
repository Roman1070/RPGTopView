public class UpdatePlayerUiWidgetSignal : ISignal
{
    public float NormalizedExp;
    public int Level;
    public bool LevelChanged;

    public UpdatePlayerUiWidgetSignal(float normalizedExp, int level, bool levelChanged)
    {
        NormalizedExp = normalizedExp;
        Level = level;
        LevelChanged = levelChanged;
    }
}
