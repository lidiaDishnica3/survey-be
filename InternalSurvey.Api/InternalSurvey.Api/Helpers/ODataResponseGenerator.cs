using AutoMapper;
using InternalSurvey.Api.Dtos;
using Microsoft.AspNet.OData.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Helpers
{
    public class ODataResponseGenerator<Entity, DTO> where DTO : class
    {
        private int totalRecords;

        public BaseResponeDto<DTO> GenerateResponseDto(ODataQueryOptions<Entity> queryOptions, IQueryable<Entity> serviceResult, IMapper mapper)
        {
            var mappedResult = GetMappedResult(queryOptions, serviceResult, mapper);

            var result = GetResponeDto(mappedResult.Result, queryOptions);

            return result;
        }

        public GeneratorResponse<DTO> GetMappedResult(ODataQueryOptions<Entity> queryOptions, IQueryable<Entity> serviceResult, IMapper mapper)
        {
            var result = queryOptions.ApplyTo(serviceResult) as IQueryable<Entity>;

            if (queryOptions.Filter != null)
            {
                totalRecords = result.Count();
            }
            else
            {
                totalRecords = serviceResult.Count();
            }

            var mappedResult = mapper.Map<List<DTO>>(result);

            var response = new GeneratorResponse<DTO>
            {
                TotalRecords = totalRecords,
                Result = mappedResult
            };

            return response;
        }

        public BaseResponeDto<DTO> GetResponeDto(List<DTO> mappedResult, ODataQueryOptions<Entity> queryOptions)
        {
            var pageSize = queryOptions.Top?.Value;
            var skip = queryOptions.Skip?.Value;

            var pageNumber = pageSize == null ? 1 : (int)skip / (int)pageSize + 1;

            var response = new BaseResponeDto<DTO>
            {
                TotalRecords = totalRecords,
                PageSize = skip == null ? totalRecords : (int)pageSize,
                PageNumber = pageNumber,
                Body = mappedResult
            };

            return response;
        }
    }

    public class GeneratorResponse<DTO> where DTO : class
    {
        public int TotalRecords { get; set; }
        public List<DTO> Result { get; set; }
    }
}