using AutoMapper;
using Dot.Net.WebApi.Domain;

namespace P7CreateRestApi.Mapping
{
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Auto Mapper va s'occuper du changement des données entre les objets sans altérer les objets dont la nouvelle valeur est null
        /// </summary>
        public MappingProfile()
        {
            CreateMap<BidList, BidList>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                    srcMember != null && !srcMember.Equals(dest.GetType().GetProperty(opts.DestinationMember.Name)?.GetValue(dest))));
        }
    }
}
