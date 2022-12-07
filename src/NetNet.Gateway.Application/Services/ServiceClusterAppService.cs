using NetNet.Gateway.AggregateModels.ServiceClusterAggregate;
using NetNet.Gateway.Dtos.ServiceClusters.Requests;
using NetNet.Gateway.Dtos.ServiceClusters.Responses;
using Volo.Abp.Application.Dtos;

namespace NetNet.Gateway.Services;

public class ServiceClusterAppService : GatewayAppService, IServiceClusterAppService
{
    private readonly IServiceClusterRepository _clusterRepository;

    public ServiceClusterAppService(IServiceClusterRepository clusterRepository)
    {
        _clusterRepository = clusterRepository;
    }

    public async Task<PagedResultDto<QueryServiceClusterRes>> QueryAsync(QueryServiceClusterReq req)
    {
        var clusterQueryable = await _clusterRepository.WithDetailsAsync(x => x.Destinations);
        var queryable = from cluster in clusterQueryable
            orderby cluster.CreationTime descending
            select new QueryServiceClusterRes
            {
                Id = cluster.Id,
                Name = cluster.Name,
                Description = cluster.Description,
                DestinationCount = cluster.Destinations.Count,
                CreationTime = cluster.CreationTime,
                Destinations = cluster.Destinations
                    .Select(x => new ServiceDestinationRes { Key = x.Key, Address = x.Address, Health = x.Health })
                    .ToList()
            };
        var queryableWrapper = QueryableWrapperFactory.CreateWrapper(queryable)
            .SearchByKey(req.SearchKey, x => x.Name)
            .AsNoTracking();

        var count = await queryableWrapper.CountAsync();
        var items = new List<QueryServiceClusterRes>(req.MaxResultCount);
        if (count > 0)
        {
            items = await queryableWrapper.PageBy(req.SkipCount, req.MaxResultCount)
                .ToListAsync();
        }

        return new(count, items);
    }

    public async Task<GetServiceClusterRes> GetAsync(Guid id)
    {
        var queryable = await _clusterRepository.WithDetailsAsync(x => x.Destinations);
        var cluster = await AsyncExecuter.SingleAsync(queryable.Where(x => x.Id == id));

        return ObjectMapper.Map<ServiceCluster, GetServiceClusterRes>(cluster);
    }

    public async Task<Guid> CreateAsync(InputServiceClusterReq req)
    {
        var cluster = new ServiceCluster(req.Name, req.Description, req.LoadBalancingPolicy, null, null);
        foreach (var destination in req.Destinations)
        {
            cluster.AddDestination(destination.Key, destination.Address, destination.Health, destination.Metadata);
        }

        await _clusterRepository.InsertAsync(cluster);

        return cluster.Id;
    }

    public async Task<bool> UpdateAsync(Guid id, InputServiceClusterReq req)
    {
        return true;
    }
}
