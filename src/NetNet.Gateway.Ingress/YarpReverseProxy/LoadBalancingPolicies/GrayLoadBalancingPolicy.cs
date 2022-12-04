using Yarp.ReverseProxy.LoadBalancing;
using Yarp.ReverseProxy.Model;

namespace NetNet.Gateway.Ingress.YarpReverseProxy.LoadBalancingPolicies;

public class GrayLoadBalancingPolicy : ILoadBalancingPolicy
{
    public DestinationState? PickDestination(HttpContext context, ClusterState cluster, IReadOnlyList<DestinationState> availableDestinations)
    {
        throw new NotImplementedException();
    }

    public string Name => "Gray";
}
