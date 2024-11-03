using ScoreWorker.Models.DTO;

namespace ScoreWorker.Prompt.Interfaces;

/// <summary>
/// Interface for converting the received prompt into a model
/// </summary>
public interface IPromptParser
{
    public GetSummaryResponse ParseMainSummary(string summary);
}
