using System.Collections.Generic;

public class QueueUpdatePlayerWidgetSignals : ISignal
{
    public Queue<UpdatePlayerUiWidgetSignal> Queue;

    public QueueUpdatePlayerWidgetSignals(Queue<UpdatePlayerUiWidgetSignal> queue)
    {
        Queue = queue;
    }
}