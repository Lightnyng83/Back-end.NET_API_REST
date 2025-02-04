using Dot.Net.WebApi.Domain;

namespace P7CreateRestApi.Data.Services
{
    public interface ICurvePointService
    {
        Task AddAsync(CurvePoint curvePoint);
        Task<CurvePoint> FindByIdAsync(int id);
        Task<List<CurvePoint>> FindAllAsync();
        Task DeleteAsync(int id);
        Task UpdateAsync(CurvePoint curvePoint);
    }
}
