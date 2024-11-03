using Refit;
using ScoreWorker.Models.DTO;
using ScoreWorker.Models.Enum;
using ScoreWorker.Prompt.Interfaces;
using ScoreWorker.RefitApi;
using System.Text;

namespace ScoreWorker.Prompt;

public class PromptHandler : IPromptHandler
{
    private const string MAIN_PROMPT = "MainPrompt.txt";
    private const string SELF_PROMPT = "SelfPrompt.txt";
    private const string OPINION_PROMPT = "OpinionPrompt.txt";

    public async Task<string> GetSummary(
        PromptType promptType,
        List<ReviewInfo> reviews,
        CancellationToken cancellationToken)
    {
        var filePath = SwitchPromptPath(promptType);

        var prompt = await PreparePrompt(filePath, reviews, cancellationToken);

        return await EvaluateReviewsWithLLM(prompt, cancellationToken);
    }

    #region Private

    private async Task<string> PreparePrompt(
        string filePrompt, List<ReviewInfo> reviews, CancellationToken cancellationToken)
    {
        StringBuilder builder = new();

        for (int i = 1; i <= reviews.Count; i++)
            builder.AppendLine($"Review {i}:\n{reviews[i - 1].Review}");

        string samplePrompt = (await File.ReadAllTextAsync(filePrompt, cancellationToken))
            .Replace("\\n", "\n");

        return string.Format(samplePrompt, builder.ToString());
    }

    private async Task<string> EvaluateReviewsWithLLM(
        string prompt, CancellationToken cancellationToken)
    {
        var apiService = RestService.For<IVkControllerApi>(IVkControllerApi.VkScoreWorkerApi);

        var request = new GenerateScoreRequest()
        {
            Prompt = prompt,
            ApplyChatTemplate = true,
            SystemPrompt = "You are a helpful assistant of an HR speacialist that rates employees of the company they work at.",
            N = 1,
            Temperature = 0.3
        };

        return await apiService.GenerateScore(request);
    }

    private string SwitchPromptPath(PromptType promptType)
    {
        return promptType switch
        {
            PromptType.Main => MAIN_PROMPT,
            PromptType.Self => SELF_PROMPT,
            PromptType.Opinion => OPINION_PROMPT,
            _ => throw new NotImplementedException(),
        };
    }

    #endregion
}
