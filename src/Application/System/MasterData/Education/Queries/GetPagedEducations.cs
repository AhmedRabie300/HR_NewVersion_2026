// Application/System/MasterData/Education/Queries/GetPagedEducations.cs
using Application.Common.Abstractions;
using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Education.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Education.Queries
{
    public static class GetPagedEducations
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm)
            : IRequest<PagedResult<EducationDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<EducationDto>>
        {
            private readonly IEducationRepository _repo;
            private readonly IContextService _ContextService;

            public Handler(IEducationRepository repo, IContextService ContextService)
            {
                _repo = repo;
                _ContextService = ContextService;
            }

            public async Task<PagedResult<EducationDto>> Handle(Query request, CancellationToken cancellationToken)
            {
              //  var companyId = _ContextService.GetCurrentCompanyId();

                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm 
                );

                var items = pagedResult.Items.Select(x => new EducationDto(
                    Id: x.Id,
                    Code: x.Code,
                    CompanyId: x.CompanyId,
                    CompanyName: x.Company?.EngName ?? x.Company?.ArbName,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    Level: x.Level,
                    RequiredYears: x.RequiredYears,
                    Remarks: x.Remarks,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive()
                )).ToList();

                return new PagedResult<EducationDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}