using Application.Common.Abstractions;
using Application.UARbac.Abstractions;
using Application.UARbac.Modules.Dtos;
using FluentValidation;
using MediatR;

namespace Application.UARbac.Modules.Queries
{
    public static class GetModulesByType
    {
        public record Query(string ModuleType) : IRequest<List<GetModuleDto>>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.ModuleType)
                    .NotEmpty().WithMessage(msg.Get("Required"))
                    .Must(x => new[] { "HR", "GL", "AR", "AP", "FA", "INV", "MANF", "SYS" }.Contains(x.ToUpper()))
                    .WithMessage(msg.Get("InvalidModuleType"));
            }
        }

        public class Handler : IRequestHandler<Query, List<GetModuleDto>>
        {
            private readonly IModuleRepository _repo;

            public Handler(IModuleRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<GetModuleDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var modules = await _repo.GetModulesByTypeAsync(request.ModuleType);

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
