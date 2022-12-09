using NetNet.Gateway.AggregateModels.ServiceClusterAggregate;
using NetNet.Gateway.Dtos.ServiceClusters;
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
        var clusterQueryable = await _clusterRepository.WithDetailsAsync(x => x.Destinations, x => x.HealthCheckConfig);
        var queryable = from cluster in clusterQueryable
            orderby cluster.CreationTime descending
            select new QueryServiceClusterRes
            {
                Id = cluster.Id,
                Name = cluster.Name,
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
        var cluster = await _clusterRepository.GetAsync(id);

        return ObjectMapper.Map<ServiceCluster, GetServiceClusterRes>(cluster);
    }

    public async Task<Guid> CreateAsync(InputServiceClusterReq req)
    {
        var httpRequestConfig = ObjectMapper.Map<ServiceClusterHttpRequestConfigDto, ServiceClusterHttpRequestConfig>(req.HttpRequestConfig);
        var httpClientConfig = ObjectMapper.Map<ServiceClusterHttpClientConfigDto, ServiceClusterHttpClientConfig>(req.HttpClientConfig);
        var healthCheckConfig = ObjectMapper.Map<ServiceClusterHealthCheckConfigDto, ServiceClusterHealthCheckConfig>(req.HealthCheckConfig);
        var destinations = req.Destinations
            .Select(x => new ServiceDestination(x.Key, x.Address, x.Health, x.Metadata))
            .ToList();

        var cluster = new ServiceCluster(req.Name, req.LoadBalancingPolicy, httpRequestConfig, httpClientConfig, healthCheckConfig, destinations);

        await _clusterRepository.InsertAsync(cluster);

        return cluster.Id;
    }

    public async Task<bool> UpdateAsync(Guid id, InputServiceClusterReq req)
    {
        var httpRequestConfig = ObjectMapper.Map<ServiceClusterHttpRequestConfigDto, ServiceClusterHttpRequestConfig>(req.HttpRequestConfig);
        var httpClientConfig = ObjectMapper.Map<ServiceClusterHttpClientConfigDto, ServiceClusterHttpClientConfig>(req.HttpClientConfig);
        var healthCheckConfig = ObjectMapper.Map<ServiceClusterHealthCheckConfigDto, ServiceClusterHealthCheckConfig>(req.HealthCheckConfig);
        var destinations = req.Destinations
            .Select(x => ObjectMapper.Map<InputServiceDestinationReq, ServiceDestination>(x))
            .ToList();

        var cluster = await _clusterRepository.GetAsync(id);

        cluster.Update(req.Name, req.LoadBalancingPolicy, httpRequestConfig, httpClientConfig, healthCheckConfig, destinations);
        await _clusterRepository.UpdateAsync(cluster);

        return true;
    }
}
