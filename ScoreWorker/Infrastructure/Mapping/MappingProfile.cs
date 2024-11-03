using AutoMapper;
using ScoreWorker.Models.Db;
using ScoreWorker.Models.DTO;

namespace ScoreWorker.Infrastructure.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DbReview, ReviewInfo>();
        CreateMap<ReviewInfo, DbReview>()
            .ForMember(db => db.Id, opt => opt.MapFrom(_ => Guid.NewGuid()));

        CreateMap<DbSummary, GetSummaryResponse>();
        CreateMap<GetSummaryResponse, DbSummary>()
            .ForMember(db => db.Id, opt => opt.MapFrom(_ => Guid.NewGuid()));

        CreateMap<DbScoreCriteria, GetSummaryResponse>();
        CreateMap<GetSummaryResponse, DbScoreCriteria>()
            .ForMember(db => db.Id, opt => opt.MapFrom(_ => Guid.NewGuid()));

        CreateMap<ScoreCriteriaInfo, DbScoreCriteria>().ReverseMap();

    }
}
