using AutoMapper;
using ScoreWorker.DB.Interfaces;
using ScoreWorker.Models.DTO;
using ScoreWorker.Models.Enum;
using ScoreWorker.Prompt.Interfaces;

namespace ScoreWorker.Prompt;

public class PromptParser : IPromptParser
{
    private readonly IDataProvider _provider;
    private readonly IMapper _mapper;
    private readonly IPromptHandler _promptHandler;

    public PromptParser(
        IDataProvider provider,
        IMapper mapper,
        IPromptHandler promptHandler)
    {
        _provider = provider;
        _mapper = mapper;
        _promptHandler = promptHandler;
    }

    public GetSummaryResponse ParseMainSummary(string summary)
    {
        return new GetSummaryResponse()
        {
            Summary = "Крутой",
            Score = 5,
            NegativeQuality = "",
            PositiveQuality = "",
            NegativeReviewCount = 0,
            PositiveReviewCount = 1,
            ScoreCriteria = new()
            {
                [ScoreCriteriaType.Leadership] = new ScoreCriteriaInfo() { Summary = "Крутой", Score = 5}
            },
            SelfSummary = "Rhenj"
        };
    }
}
