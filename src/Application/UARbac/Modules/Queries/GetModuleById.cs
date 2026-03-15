using Application.UARbac.Modules.Dtos;
using Application.UARbac.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.UARbac.Modules.Queries
{
    public static class GetModuleById
    {
         public record Query(int Id) : IRequest<GetModuleDto>;

         public sealed class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0)
                    .WithMessage("Module ID must be greater than 0");
            }
        }

         public class Handler : IRequestHandler<Query, GetModuleDto>
        {
            private readonly IModuleRepository _repo;

            public Handler(IModuleRepository repo)
            {
                _repo = repo;
            }

            public async Task<GetModuleDto> Handle(Query request, CancellationToken cancellationToken)
            {
                 var module = await _repo.GetByIdAsync(request.Id);

                 if (module == null)
                    throw new Exception($"Module with ID {request.Id} not found");

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
                    RegComputerId: module.RegComputerId,
                    RegDate: module.RegDate,
                    CancelDate: module.CancelDate,
                    IsActive: module.IsActive()  
                );
            }
        }
    }
}