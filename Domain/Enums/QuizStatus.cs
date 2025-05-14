namespace Domain.Enums
{
    public enum QuizStatus
    {
        Draft,      // Черновик (не опубликован)
        Active,     // Активен
        Inactive,   // Скрыт
        Archived    // В архиве (нельзя изменить)
    }
}