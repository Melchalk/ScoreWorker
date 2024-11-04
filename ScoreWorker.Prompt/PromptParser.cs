using ScoreWorker.Models.DTO;
using ScoreWorker.Models.Enum;
using ScoreWorker.Prompt.Interfaces;
using System.Text.RegularExpressions;

namespace ScoreWorker.Prompt;

public class PromptParser : IPromptParser
{
    public GetSummaryResponse ParseMainSummary(string summary)
    {
        var paragraphs = summary.Split("\n\n", StringSplitOptions.None)
            .Select(p => p.Trim(' ', '\n'))
            .ToDictionary(x => x, x => false);

        var scoreCriteria = GetScoreCriteria(paragraphs);

        (int all, int positive) countReview = GetReviewCount(paragraphs);

        (string positive, string negative) qualities = GetQualities(paragraphs);

        return new GetSummaryResponse()
        {
            Summary = paragraphs.First(p => !p.Value).Key,
            Score = scoreCriteria.Select(s => s.Score).Average(),
            ScoreCriteria = scoreCriteria,
            SelfSummary = string.Empty,
            SummaryByOwnReviews = string.Empty,
            PositiveQuality = qualities.positive,
            NegativeQuality = qualities.negative,
            PositiveReviewCount = countReview.positive,
            AllReviewCount = countReview.all
        };
    }

    #region Private

    private List<ScoreCriteriaInfo> GetScoreCriteria(
        Dictionary<string, bool> paragraphs)
    {
        var scoreCriteria = new List<ScoreCriteriaInfo>();

        var rex = new Regex(@"\w+:\s{1}[1-5]{1}");

        foreach (var paragraph in paragraphs.Keys)
        {
            var matches = rex.Matches(paragraph);

            if (paragraphs[paragraph] || matches.Count == 0)
            {
                continue;
            }

            var criteria = matches.First().Value;

            var spacesIndex = criteria.IndexOf(' ');

            var criteriaInfo = new ScoreCriteriaInfo()
            {
                Type = (ScoreCriteriaType)Enum.Parse(typeof(ScoreCriteriaType), criteria.AsSpan(0, spacesIndex - 1)),
                Score = int.Parse(criteria.AsSpan(spacesIndex + 1)),
                Description = paragraph.Substring(paragraph.IndexOf('\n') + 1)
            };

            scoreCriteria.Add(criteriaInfo);

            paragraphs[paragraph] = true;
        }

        return scoreCriteria;
    }

    private (int all, int positive) GetReviewCount(
        Dictionary<string, bool> paragraphs)
    {
        (int all, int positive) countReview = new();

        var rex = new Regex(@"[0-9]+:\s\w+");

        string reviewsTypes = string.Empty;

        foreach (var paragraph in paragraphs.Keys)
        {
            var matches = rex.Matches(paragraph);

            if (paragraphs[paragraph] || matches.Count == 0)
            {
                continue;
            }

            reviewsTypes = paragraph;

            paragraphs[paragraph] = true;
        }

        countReview.positive = new Regex(ReviewType.Positive.ToString())
            .Matches(reviewsTypes).Count;

        countReview.all = reviewsTypes.Count(c => c == '\n');

        return countReview;
    }
    
    private (string positive, string negative) GetQualities(
        Dictionary<string, bool> paragraphs)
    {
        (string positive, string negative) qualities = new();

        var rex = new Regex(@"\w+\s-\s");

        foreach (var paragraph in paragraphs.Keys)
        {
            var matches = rex.Matches(paragraph);

            if (paragraphs[paragraph] || matches.Count == 0)
            {
                continue;
            }

            var records = paragraph.Split('\n');

            var matchFirst = records[0][(records[0].IndexOf('-') + 2)..];
            var matchSecond = records[1][(records[1].IndexOf('-') + 2)..];

            qualities.positive = records[0].StartsWith("Pro") ? matchFirst : matchSecond;
            qualities.negative = records[0].StartsWith("Pro") ? matchSecond : matchFirst;

            paragraphs[paragraph] = true;
        }

        return qualities;
    }

    #endregion
}