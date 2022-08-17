using System;
using System.Collections.Generic;
using UnityEngine;

internal abstract class SignalSubscriptionWrapper
{
    public abstract object Identifier { get; }
}

internal class SignalSubscription<T> : SignalSubscriptionWrapper
{
    public readonly Action<T> Callback;
    public override object Identifier { get; }

    public SignalSubscription(Action<T> callback, object identifier)
    {
        Callback = callback;
        Identifier = identifier;
    }
}

public class SignalBus : MonoBehaviour
{
    private readonly Dictionary<Type, List<SignalSubscriptionWrapper>> _subscriptionsMap = new Dictionary<Type, List<SignalSubscriptionWrapper>>();


    public void Subscribe<TSignal>(Action<TSignal> callback, object identifier) 
    {
        List<SignalSubscriptionWrapper> subscriptions;
        if (!_subscriptionsMap.TryGetValue(typeof(TSignal), out subscriptions))
        {
            subscriptions = new List<SignalSubscriptionWrapper>();
            _subscriptionsMap.Add(typeof(TSignal), subscriptions);
        }
        subscriptions.Add(new SignalSubscription<TSignal>(callback, identifier));
    }

    public void UnSubscribe<TSignal>(object identifier)
    {
        List<SignalSubscriptionWrapper> subscriptions;
        if (!_subscriptionsMap.TryGetValue(typeof(TSignal), out subscriptions))
        {
            Debug.LogWarning($"{identifier} not subscribed to signal {typeof(TSignal)}");
            return;
        }
        subscriptions.RemoveAll(_ => _.Identifier == identifier);
    }

    public void UnSubscribeFromAll(object identifier)
    {
        foreach (var signalsSubscriptions in _subscriptionsMap)
        {
            signalsSubscriptions.Value.RemoveAll(_ => _.Identifier == identifier);
        }
    }

    public void FireSignal<TSignal>(TSignal signal)
    {
        var subscriptions = GetSignalSubscriptions<TSignal>();
        var count = subscriptions?.Count;
        for (int i = 0; i < count; i++)
        {
            ((SignalSubscription<TSignal>)subscriptions[i]).Callback.Invoke(signal);
        }
    }

    private List<SignalSubscriptionWrapper> GetSignalSubscriptions<TSignal>()
    {
        if (!_subscriptionsMap.TryGetValue(typeof(TSignal), out var subscriptions))
        {
            Debug.LogWarning($"no subscriptions to signal {typeof(TSignal)}");
            return null;
        }
        return subscriptions;
    }
}
