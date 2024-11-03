using ScoreWorker.Models.DTO;

namespace ScoreWorker.Prompt.Interfaces;

public interface IPromptHandler
{
    public Task<string> GetSummary(List<ReviewInfo> reviews, CancellationToken cancellationToken, bool isMain = true);
}
