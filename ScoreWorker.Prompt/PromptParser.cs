using AutoMapper;
using ScoreWorker.DB.Interfaces;
using ScoreWorker.Models.DTO;
using ScoreWorker.Models.Enum;
using ScoreWorker.Prompt.Interfaces;

namespace ScoreWorker.Prompt;

public class PromptParser : IPromptParser
{
    private readonly IDataProvider _provider;
    private readonly IMapper _mapper;
    private readonly IPromptHandler _promptHandler;

    public PromptParser(
        IDataProvider provider,
        IMapper mapper,
        IPromptHandler promptHandler)
    {
        _provider = provider;
        _mapper = mapper;
        _promptHandler = promptHandler;
    }

    public GetSummaryResponse ParseMainSummary(string summary)
    {
        return new GetSummaryResponse()
        {
            Summary = "Этот сотрудник является отличным руководителем и командным игроком, обладает высокими коммуникативными навыками и способен решать сложные проблемы. Он всегда готов помочь и поддержать коллег, и его адаптивность позволяет ему легко справляться с изменениями и новыми задачами.",
            Score = 5,
            NegativeQuality = "",
            PositiveQuality = "",
            NegativeReviewCount = 0,
            PositiveReviewCount = 56,
            ScoreCriteria = new()
            {
                [ScoreCriteriaType.Leadership] = new ScoreCriteriaInfo()
                {
                    Summary = "Руководитель, который умеет замечать проблему в зародыше и решать ее до возникновения инцидентов. Обладает высоким уровнем профессионализма и умением работать в команде. Всегда готова помочь своим подчиненным и дать советы по работе. (\"Молодой руководитель с горящими глазами и желанием делать мир лучше.\", \"Руководитель, который готов брать на себя ответственность и принимать сложные решения.\")",
                    Score = 5
                },
                [ScoreCriteriaType.CommunicationSkills] = new ScoreCriteriaInfo()
                {
                    Summary = "Обладает высокими коммуникативными навыками, всегда готова помочь и дать обратную связь. (\"Всегда дает обратную связь по всем вопросам связанным по работе, графику, модерации и тд.\", \"Очень оперативно помогает с возникающими трудностями, предлагает классные идеи по оптимизации/доработками работы [ПРОДУКТ], бота\")",
                    Score = 5
                },
                [ScoreCriteriaType.ProblemSolving] = new ScoreCriteriaInfo()
                {
                    Summary = "Умеет решать сложные проблемы, подходит к задачам конструктивно, спокойно и с глубоким пониманием процесса и механик. (\"Подходит к задачам конструктивно, спокойно и с глубоким пониманием процесса и механик.\", \"Очень оперативно помогает с возникающими трудностями, предлагает классные идеи по оптимизации/доработками работы [ПРОДУКТ], бота\")",
                    Score = 5
                },

                [ScoreCriteriaType.Teamwork] = new ScoreCriteriaInfo()
                {
                    Summary = "Отличные навыки командной работы, всегда готова помочь и поддержать коллег. (\"Отличный руководитель, с ней очень легко и приятно работать.\", \"Всегда идет на встречу, можем пообщаться, посмеяться и поговорить не о работе, что очень радует.\")",
                    Score = 5
                },

                [ScoreCriteriaType.Adaptability] = new ScoreCriteriaInfo()
                {
                    Summary = "Гибкий и адаптивный сотрудник, всегда готова к изменениям и новым задачам. (\"Всегда готова к изменениям и новым задачам.\", \"Очень оперативно помогает с возникающими трудностями, предлагает классные идеи по оптимизации/доработками работы [ПРОДУКТ], бота\")",
                    Score = 5
                },
            },
            SelfSummary = "Rhenj"
        };
    }
}
