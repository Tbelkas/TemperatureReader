using iSun.Domain.Models;

namespace iSun.Cmd.Services;

public interface IQueryingService
{
    public Task<IResponse> StartQuerying(List<string> cities);
}