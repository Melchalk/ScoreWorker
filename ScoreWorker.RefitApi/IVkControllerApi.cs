using Refit;
using ScoreWorker.Models.DTO;

namespace ScoreWorker.RefitApi;

public interface IVkControllerApi
{
    public const string VkScoreWorkerApi = "https://vk-scoreworker-case-backup.olymp.innopolis.university";

    [Post("/generate")]
    public Task<string> GenerateScore([Body] GenerateScoreRequest request);
}