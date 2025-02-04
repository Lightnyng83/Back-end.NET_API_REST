using Dot.Net.WebApi.Domain;

namespace P7CreateRestApi.Data.Repositories
{
    public interface ICurvePointRepository
    {
        Task AddAsync(CurvePoint curvePoint);
        Task<CurvePoint> FindByIdAsync(int id);
        Task<List<CurvePoint>> FindAll();
        Task DeleteAsync(int id);
        Task Update(CurvePoint curvePoint);
    }
}
