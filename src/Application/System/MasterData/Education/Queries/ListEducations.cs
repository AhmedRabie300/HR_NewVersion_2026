// Application/System/MasterData/Education/Queries/ListEducations.cs
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
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(IEducationRepository repo, IHttpContextAccessor httpContextAccessor)
            {
                _repo = repo;
                _httpContextAccessor = httpContextAccessor;
            }

            private int GetRequiredCompanyId()
            {
                var context = _httpContextAccessor.HttpContext;
                var companyId = context?.Items["CompanyId"] as int?;
                if (!companyId.HasValue)
                    throw new UnauthorizedAccessException("Company ID is required in request header (X-CompanyId)");
                return companyId.Value;
            }

            public async Task<List<EducationDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = GetRequiredCompanyId();

                var items = await _repo.GetAllAsync(companyId);

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