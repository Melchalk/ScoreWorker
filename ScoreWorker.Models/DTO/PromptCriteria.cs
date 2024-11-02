namespace ScoreWorker.Models.DTO;

public class PromptCriteria
{
    // Обязательный параметр
    public object Prompt { get; set; } // string или array<string>

    // Дополнительное системное сообщение
    public string? SystemPrompt { get; set; }

    // Параметр случайности
    public double? Temperature { get; set; }

    // Применение шаблона чата
    public bool? ApplyChatTemplate { get; set; }

    // Стоп-токены
    public object? Stop { get; set; } // string или array<string>

    // Включение стоп-токенов в вывод
    public bool? IncludeStopStrInOutput { get; set; }

    // Максимальное количество токенов
    public int? MaxTokens { get; set; }

    // Минимальное количество токенов
    public int? MinTokens { get; set; }

    // Количество вариантов ответов
    public int N { get; set; }

    // Количество ответов для выбора лучших
    public int? BestOf { get; set; }

    // Штраф за частое повторение
    public double? FrequencyPenalty { get; set; }

    // Штраф за наличие новых токенов
    public double? PresencePenalty { get; set; }

    // Штраф за повторение токенов
    public double? RepetitionPenalty { get; set; }

    // Штраф за длину ответа
    public double? LengthPenalty { get; set; }

    // Случайное значение для воспроизводимости
    public int? Seed { get; set; }

    // Стриминг ответа
    public bool? Stream { get; set; }

    // Ограничение выбора следующего токена
    public double? TopP { get; set; }

    // Минимальная вероятность токена
    public double? MinP { get; set; }

    // Ограничение выбора токена по вероятности
    public int? TopK { get; set; }

    // Идентификаторы токенов-стопов
    public List<int>? StopTokenIds { get; set; }

    // Игнорировать токен конца последовательности
    public bool? IgnoreEOS { get; set; }

    // Пропускать специальные токены
    public bool? SkipSpecialTokens { get; set; }

    // Добавлять пробелы между специальными токенами
    public bool? SpacesBetweenSpecialTokens { get; set; }
}