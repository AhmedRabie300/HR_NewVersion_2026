using Application.UARbac.Modules.Dtos;
using Application.UARbac.Abstractions;
using MediatR;

namespace Application.UARbac.Modules.Queries
{
    public static class ListModules
    {
        public record Query : IRequest<List<GetModuleDto>>;

        public class Handler : IRequestHandler<Query, List<GetModuleDto>>
        {
            private readonly IModuleRepository _repo;

            public Handler(IModuleRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<GetModuleDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                 var modules = await _repo.GetAllAsync();

                return modules.Select(m => new GetModuleDto(
                    Id: m.Id,
                    Code: m.Code,
                    Prefix: m.Prefix,
                    EngName: m.EngName,
                    ArbName: m.ArbName,
                    ArbName4S: m.ArbName4S,
                    FormId: m.FormId,
                    IsRegistered: m.IsRegistered,
                    FiscalYearDependant: m.FiscalYearDependant,
                    Rank: m.Rank,
                    Remarks: m.Remarks,
                    IsAR: m.IsAR,
                    IsAP: m.IsAP,
                    IsGL: m.IsGL,
                    IsFA: m.IsFA,
                    IsINV: m.IsINV,
                    IsHR: m.IsHR,
                    IsMANF: m.IsMANF,
                    IsSYS: m.IsSYS,
                    RegUserId: m.RegUserId,
                    regComputerId: m.regComputerId,
                    RegDate: m.RegDate,
                    CancelDate: m.CancelDate,
                    IsActive: m.IsActive()
                )).ToList();
            }
        }
    }
}