// Application/System/MasterData/Education/Queries/ListEducations.cs
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Education.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Education.Queries
{
    public static class ListEducations
    {
        public record Query : IRequest<List<EducationDto>>;

        public class Handler : IRequestHandler<Query, List<EducationDto>>
        {
            private readonly IEducationRepository _repo;
            private readonly IContextService _ContextService;

            public Handler(IEducationRepository repo, IContextService ContextService)
            {
                _repo = repo;
                _ContextService = ContextService;
            }

            public async Task<List<EducationDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                //var companyId = _ContextService.GetCurrentCompanyId();

                var items = await _repo.GetAllAsync();

                return items.Select(x => new EducationDto(
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
            }
        }
    }
}