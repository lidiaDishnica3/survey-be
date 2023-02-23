using InternalSurvey.Api.Entities;
using InternalSurvey.Api.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Dtos
{
    public class SurveyAnalitsicsDto
    {
        public SurveyDto Survey { get; set; }
        public List<QuestionAnalytics> Questions { get; set; }
        public int Respondents { get; set; }
        public string SurveyTitle { get; set; }
        public string SurveyRespondents { get; set; }
    }

    public class QuestionAnalytics
    {
        public int QuestionId { get; set; }
        public string QuestionTitle { get; set; }
        public QuestionTypeEnum QuestionType { get; set; }
        public int TotalVoters { get; set; }
        public bool HasOthers { get; set; }
        public List<OptionsAnalytics> Options { get; set; }
    }
    public class OptionsAnalytics {
        public int OptionId { get; set; }
        public int OptionVoters { get; set; }
        public string Option { get; set; }
        public Dictionary<string,int> Responses { get; set; }
    }
}
