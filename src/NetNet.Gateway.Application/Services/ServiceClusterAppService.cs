using NetNet.Gateway.AggregateModels.ServiceClusterAggregate;
using NetNet.Gateway.Dtos.ServiceClusters.Requests;
using NetNet.Gateway.Dtos.ServiceClusters.Responses;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace NetNet.Gateway.Services;

public class ServiceClusterAppService : GatewayAppService, IServiceClusterAppService
{
    private readonly IRepository<ServiceCluster, long> _clusterRepository;

    public ServiceClusterAppService(IRepository<ServiceCluster, long> clusterRepository)
    {
        _clusterRepository = clusterRepository;
    }

    public async Task<PagedResultDto<QueryServiceClusterRes>> QueryAsync(QueryServiceClusterReq req)
    {
        var clusterQueryable = await _clusterRepository.GetQueryableAsync();
        var queryable = from cluster in clusterQueryable
            orderby cluster.CreationTime descending
            select new QueryServiceClusterRes
            {
                Id = cluster.Id,
                Name = cluster.Name,
                Description = cluster.Description,
                DestinationCount = cluster.Destinations.Count,
                CreationTime = cluster.CreationTime
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

    public async Task<GetServiceClusterRes> GetAsync(long id)
    {
        var queryable = await _clusterRepository.WithDetailsAsync(x => x.Destinations);
        var cluster = await AsyncExecuter.SingleAsync(queryable.Where(x => x.Id == id));

        return ObjectMapper.Map<ServiceCluster, GetServiceClusterRes>(cluster);
    }

    public async Task<long> CreateAsync(InputServiceClusterReq req)
    {
        return 0;
    }

    public async Task<bool> UpdateAsync(long id, InputServiceClusterReq req)
    {
        return true;
    }
}
