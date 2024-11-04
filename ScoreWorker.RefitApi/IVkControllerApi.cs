using Refit;
using ScoreWorker.Models.DTO;

namespace ScoreWorker.RefitApi;

/// <summary>
/// Interface for interacting with the generation API
/// </summary>
public interface IVkControllerApi
{
    public const string VkScoreWorkerApi = "https://vk-scoreworker-case.olymp.innopolis.university";

    [Post("/generate")]
    public Task<string> GenerateScore([Body] GenerateScoreRequest request);
}