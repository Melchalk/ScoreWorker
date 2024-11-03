using ScoreWorker.Models.Enum;

namespace ScoreWorker.PromptHelpers;

/// <summary>
/// Static class for storing and defining the required prompt file
/// </summary>
public static class PromptPathContainer
{
    private const string MAIN_PROMPT = "MainPrompt.txt";
    private const string SELF_PROMPT = "SelfPrompt.txt";
    private const string OWN_REVIEWS_PROMPT = "OwnReviewsPrompt.txt";
    private const string OPINION_PROMPT = "OpinionPrompt.txt";

    public static string SwitchPromptPath(PromptType promptType)
    {
        return promptType switch
        {
            PromptType.Main => MAIN_PROMPT,
            PromptType.Self => SELF_PROMPT,
            PromptType.Opinion => OPINION_PROMPT,
            PromptType.OwnReviews => OWN_REVIEWS_PROMPT,
            _ => throw new NotImplementedException(),
        };
    }

}
