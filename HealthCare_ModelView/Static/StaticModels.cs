using System.Collections.Generic;

namespace HealthCare_Models.Static
{
    public static class StaticData
    {
        public static readonly List<int> TextAnswerQuestionType = new List<int>() { 3 };

        public static readonly List<int> MultipleChoiceQuestion = new List<int>() { 1, 2 };

        public static readonly List<string> SupportedSortColumns = new List<string>() { "CreatedUtc", "DueDateUtc" };
    }
}
