// Application/System/MasterData/Profession/Queries/ListProfessions.cs
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Profession.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Profession.Queries
{
    public static class ListProfessions
    {
        public record Query : IRequest<List<ProfessionDto>>;

        public class Handler : IRequestHandler<Query, List<ProfessionDto>>
        {
            private readonly IProfessionRepository _repo;
            private readonly IContextService _ContextService;

            public Handler(IProfessionRepository repo,  IContextService ContextService)
            {
                _repo = repo;
                _ContextService = ContextService;
            }

     

            public async Task<List<ProfessionDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = _ContextService.GetCurrentCompanyId();
                var lang = _ContextService.GetCurrentLanguage();

                var items = await _repo.GetAllAsync(companyId);

                return items.Select(x => new ProfessionDto(
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