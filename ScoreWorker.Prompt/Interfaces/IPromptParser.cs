using ScoreWorker.Models.DTO;

namespace ScoreWorker.Prompt.Interfaces;

public interface IPromptParser
{
    public GetSummaryResponse ParseMainSummary(string summary);
}
