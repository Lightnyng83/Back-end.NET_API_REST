using AutoMapper;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Controllers.Model;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Model;

namespace P7CreateRestApi.Mapping
{
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Auto Mapper va s'occuper du changement des données entre les objets sans altérer les objets dont la nouvelle valeur est null
        /// </summary>
        public MappingProfile()
        {

            #region  ----- BidList -----

            CreateMap<BidList, BidList>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                    srcMember != null && !srcMember.Equals(dest.GetType().GetProperty(opts.DestinationMember.Name)?.GetValue(dest))));

            CreateMap<BidList, BidListViewModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                    srcMember != null && !srcMember.Equals(dest.GetType().GetProperty(opts.DestinationMember.Name)?.GetValue(dest))));
            #endregion

            #region  ----- CurvePoint -----
            CreateMap<CurvePoint, CurvePoint>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                    srcMember != null && !srcMember.Equals(dest.GetType().GetProperty(opts.DestinationMember.Name)?.GetValue(dest))));

            CreateMap<CurvePoint, CurvePointViewModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                    srcMember != null && !srcMember.Equals(dest.GetType().GetProperty(opts.DestinationMember.Name)?.GetValue(dest))));

            #endregion

            #region  ----- Rating -----
            CreateMap<Rating, Rating>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                    srcMember != null && !srcMember.Equals(dest.GetType().GetProperty(opts.DestinationMember.Name)?.GetValue(dest))));

            CreateMap<Rating, RatingViewModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                    srcMember != null && !srcMember.Equals(dest.GetType().GetProperty(opts.DestinationMember.Name)?.GetValue(dest))));

            #endregion
        }
    }
}
