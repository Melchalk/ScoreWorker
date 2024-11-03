using ScoreWorker.Models.DTO;
using ScoreWorker.Models.Enum;

namespace ScoreWorker.Prompt.Interfaces;

public interface IPromptHandler
{
    public Task<string> GetSummary(PromptType promptType, List<ReviewInfo> reviews, CancellationToken cancellationToken);
}
