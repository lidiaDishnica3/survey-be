using InternalSurvey.Api.Entities;
using InternalSurvey.Api.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Dtos
{
    public class SubmittedAnswerDto
    {
        public int QuestionId { get; set; }
        public int QuestionOptionId { get; set; }
        public int[] QuestionOptionIds { get; set; }
        public string Other { get; set; }

        public bool IsEmpty(Question question)
        {
            if (question.QuestionType == QuestionTypeEnum.RadioButtons)
            {
                if (QuestionOptionId == 0)
                {
                    return true;
                }
            }
            else if (question.QuestionType == QuestionTypeEnum.CheckBox)
            {
                var checkedOptions = QuestionOptionIds.Where(optionId => optionId != 0);
                if (checkedOptions.Any())
                {
                    return false;
                }
                if (question.HasOthers && QuestionOptionId != 0 && Other != null)
                {
                    return false;
                }
                return true;
            }
            else if (question.QuestionType == QuestionTypeEnum.ImageChoice)
            {
                if (QuestionOptionId == 0)
                {
                    return true;
                }
            }
            else if (question.QuestionType == QuestionTypeEnum.NumberChoice || question.QuestionType == QuestionTypeEnum.TextChoice)
            {
                if (QuestionOptionId == 0 || string.IsNullOrWhiteSpace(Other))
                {
                    return true;
                }
            }            
            return false;
        }
    }
}
