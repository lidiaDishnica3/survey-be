using AutoMapper;
using InternalSurvey.Api.Dtos;
using InternalSurvey.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace InternalSurvey.Api.Automapper
{
  public class Mapping : Profile
  {
    public Mapping()
    {
      CreateMap<SurveyDto, Survey>().ReverseMap()
          .ForMember(dto => dto.QuestionDtos, opt => opt.MapFrom(y => y.Questions))
          .ForMember(dto => dto.CommentDtos, opt => opt.MapFrom(y => y.Comments));
      CreateMap<QuestionDto, Question>().ReverseMap()
          .ForMember(dto => dto.SurveyQuestionOptions, opt => opt.MapFrom(y => y.SurveyQuestionOptions));
      CreateMap<UserDto, AspNetUsers>().ReverseMap();


      CreateMap<RespondentDto, Respondent>().ReverseMap();
      CreateMap<RespondentTokenDto, RespondentTokenData>().ReverseMap();
      CreateMap<ResponseDto, Response>().ReverseMap();
      CreateMap<CommentDto, Comment>().ReverseMap()
          .ForMember(dto => dto.Respondent, opt => opt.MapFrom(y => y.Respondent)); 
      CreateMap<SurveyQuestionOptionsDto, SurveyQuestionOptions>().ReverseMap();
    }
  }
}
