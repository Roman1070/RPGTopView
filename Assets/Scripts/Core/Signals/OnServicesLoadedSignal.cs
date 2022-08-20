using System.Collections.Generic;

public class OnServicesLoadedSignal : ISignal
{
    public List<LoadableService> Services;

    public OnServicesLoadedSignal(List<LoadableService> services)
    {
        Services = services;
    }
}
