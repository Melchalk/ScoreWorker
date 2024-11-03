using ScoreWorker.Models.DTO;
using ScoreWorker.Models.Enum;

namespace ScoreWorker.Prompt.Interfaces;

/// <summary>
/// Interface for creating, processing and sending a prompt
/// </summary>
public interface IPromptHandler
{
    public Task<string> GetSummary(PromptType promptType, List<ReviewInfo> reviews, CancellationToken cancellationToken);
}
