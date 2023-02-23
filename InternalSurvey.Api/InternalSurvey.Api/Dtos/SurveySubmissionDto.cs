using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Dtos
{
    public class SurveySubmissionDto
    {
        public int SurveyId { get; set; }
        public string RespondentEmail { get; set; }
        public SubmittedAnswerDto[] Answers { get; set; }

    }
}
