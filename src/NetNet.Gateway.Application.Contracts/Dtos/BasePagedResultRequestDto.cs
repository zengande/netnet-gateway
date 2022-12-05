using Volo.Abp.Application.Dtos;

namespace NetNet.Gateway.Dtos;

public class BasePagedResultRequestDto : PagedResultRequestDto
{
    public string? SearchKey { get; set; }
}
