using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Data.Repositories;

namespace P7CreateRestApi.Data.Services
{
    public class CurvePointService : ICurvePointService
    {
        private readonly ICurvePointRepository _curvePointRepository;

        public CurvePointService(ICurvePointRepository curvePointRepository)
        {
            _curvePointRepository = curvePointRepository;
        }

        public async Task AddAsync(CurvePoint curvePoint)
        {
            await _curvePointRepository.AddAsync(curvePoint);
        }

        public async Task<List<CurvePoint>> FindAllAsync()
        {
            return await _curvePointRepository.FindAll();
        }

        public async Task DeleteAsync(int id)
        {
            await _curvePointRepository.DeleteAsync(id);
        }

        public async Task<CurvePoint> FindByIdAsync(int id)
        {
            return await _curvePointRepository.FindByIdAsync(id);
        }

        public async Task UpdateAsync(CurvePoint curvePoint)
        {
            await _curvePointRepository.Update(curvePoint);
        }


    }
}
