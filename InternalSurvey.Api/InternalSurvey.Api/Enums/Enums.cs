using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Enums
{
    public enum QuestionTypeEnum
    {
        [Description("Single Choice")]
        RadioButtons = 1,

        [Description("Multiple Choice")]
        CheckBox = 2,

        [Description("Image")]
        ImageChoice = 3,

        [Description("Number")]
        NumberChoice = 4,

        [Description("Text")]
        TextChoice = 5
    }

}
