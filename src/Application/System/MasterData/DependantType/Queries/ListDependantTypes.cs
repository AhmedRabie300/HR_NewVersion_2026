// Application/System/MasterData/DependantType/Queries/ListDependantTypes.cs
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.DependantType.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.DependantType.Queries
{
    public static class ListDependantTypes
    {
        public record Query : IRequest<List<DependantTypeDto>>;

        public class Handler : IRequestHandler<Query, List<DependantTypeDto>>
        {
            private readonly IDependantTypeRepository _repo;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(IDependantTypeRepository repo, IHttpContextAccessor httpContextAccessor)
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

            public async Task<List<DependantTypeDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = GetRequiredCompanyId();

                var items = await _repo.GetAllAsync(companyId);

                return items.Select(x => new DependantTypeDto(
                    Id: x.Id,
                    Code: x.Code,
                    CompanyId: x.CompanyId,
                    CompanyName: x.Company?.EngName ?? x.Company?.ArbName,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    Remarks: x.Remarks,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive()
                )).ToList();
            }
        }
    }
}