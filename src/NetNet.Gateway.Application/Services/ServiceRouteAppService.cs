using NetNet.Gateway.AggregateModels.ServiceClusterAggregate;
using NetNet.Gateway.AggregateModels.ServiceRouteAggregate;
using NetNet.Gateway.Dtos.ServiceRoutes.Requests;
using NetNet.Gateway.Dtos.ServiceRoutes.Responses;
using Volo.Abp.Application.Dtos;

namespace NetNet.Gateway.Services;

public class ServiceRouteAppService : GatewayAppService, IServiceRouteAppService
{
    private readonly IServiceRouteRepository _routeRepository;
    private readonly IServiceClusterRepository _clusterRepository;

    public ServiceRouteAppService(IServiceRouteRepository routeRepository, IServiceClusterRepository clusterRepository)
    {
        _routeRepository = routeRepository;
        _clusterRepository = clusterRepository;
    }

    public async Task<PagedResultDto<QueryServiceRouteRes>> QueryAsync(QueryServiceRouteReq req)
    {
        var queryable = from route in await _routeRepository.GetQueryableAsync()
            join cluster in await _clusterRepository.GetQueryableAsync() on route.ServiceClusterId equals cluster.Id
            orderby route.Order, route.CreationTime descending
            select new QueryServiceRouteRes
            {
                Id = route.Id,
                Name = route.Name,
                Path = route.Match == null ? null : route.Match.Path,
                CreationTime = route.CreationTime,
                ServiceClusterName = cluster.Name
            };

        var queryableWrapper = QueryableWrapperFactory.CreateWrapper(queryable)
            .AsNoTracking();

        var count = await queryableWrapper.CountAsync();
        var items = new List<QueryServiceRouteRes>(req.MaxResultCount);
        if (count > 0)
        {
            items = await queryableWrapper.PageBy(req.SkipCount, req.MaxResultCount).ToListAsync();
        }

        return new(count, items);
    }

    public async Task<GetServiceRouteRes> GetAsync(Guid id)
    {
        var route = await _routeRepository.GetAsync(id);

        return ObjectMapper.Map<ServiceRoute, GetServiceRouteRes>(route);
    }

    public async Task<Guid> CreateAsync(InputServiceRouteReq req)
    {
        var match = new ServiceRouteMatch(req.MatchHosts?.JoinAsString(GatewayConstant.Separator),
            req.MatchMethods?.JoinAsString(GatewayConstant.Separator), req.MatchPath);

        var transforms = req.Transforms
            .SelectMany(x => x.Value
                .Select(y => new ServiceRouteTransform() { GroupIndex = x.Key, Key = y.Key, Value = y.Value })).ToList();

        var route = new ServiceRoute(req.Name, req.ServiceClusterId, req.AuthorizationPolicy, req.CorsPolicy, req.Order, match, transforms);

        await _routeRepository.InsertAsync(route);

        return route.Id;
    }

    public async Task<bool> UpdateAsync(Guid id, InputServiceRouteReq req)
    {
        var route = await _routeRepository.GetAsync(id);
        // TODO update

        await _routeRepository.UpdateAsync(route);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        await _routeRepository.DeleteAsync(id);

        return true;
    }
}
