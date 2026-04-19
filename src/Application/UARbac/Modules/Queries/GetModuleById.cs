using Application.Common;
using Application.Common.Abstractions;
using Application.UARbac.Abstractions;
using Application.UARbac.Modules.Dtos;
using FluentValidation;
using MediatR;

namespace Application.UARbac.Modules.Queries
{
    public static class GetModuleById
    {
        public record Query(int Id) : IRequest<GetModuleDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0)
                    .WithMessage(msg.Get("IdGreaterThanZero"));
            }
        }

        public class Handler : IRequestHandler<Query, GetModuleDto>
        {
            private readonly IModuleRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IModuleRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<GetModuleDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var module = await _repo.GetByIdAsync(request.Id);
                if (module == null)
                    throw new NotFoundException(_msg.NotFound("Module", request.Id));

                return new GetModuleDto(
                    Id: module.Id,
                    Code: module.Code,
                    Prefix: module.Prefix,
                    EngName: module.EngName,
                    ArbName: module.ArbName,
                    ArbName4S: module.ArbName4S,
                    FormId: module.FormId,
                    IsRegistered: module.IsRegistered,
                    FiscalYearDependant: module.FiscalYearDependant,
                    Rank: module.Rank,
                    Remarks: module.Remarks,
                    IsAR: module.IsAR,
                    IsAP: module.IsAP,
                    IsGL: module.IsGL,
                    IsFA: module.IsFA,
                    IsINV: module.IsINV,
                    IsHR: module.IsHR,
                    IsMANF: module.IsMANF,
                    IsSYS: module.IsSYS,
                    RegUserId: module.RegUserId,
                    regComputerId: module.regComputerId,
                    RegDate: module.RegDate,
                    CancelDate: module.CancelDate,
                    IsActive: module.IsActive()
                );
            }
        }
    }
}
