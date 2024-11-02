using AutoMapper;
using ScoreWorker.Models.Db;
using ScoreWorker.Models.DTO;

namespace ScoreWorker.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DbReview, ReviewInfo>();
        CreateMap<ReviewInfo, DbReview>()
            .ForMember(db => db.Id, opt => opt.MapFrom(_ => Guid.NewGuid()));
    }
}
