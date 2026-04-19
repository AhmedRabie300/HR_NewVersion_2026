using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Branch.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Branch.Queries
{
    public static class GetBranchById
    {
        public record Query(int Id) : IRequest<BranchDto>;

        public sealed class Validator : AbstractValidator<Query>
        {

            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"));
            }
        }

        public class Handler : IRequestHandler<Query, BranchDto>
        {
            private readonly IBranchRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(
                IBranchRepository repo,
                IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<BranchDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity is null)
                    throw new NotFoundException(_msg.NotFound("Branch", request.Id));

                return new BranchDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    CompanyId: entity.CompanyId,
                    CompanyName: entity.Company?.EngName ?? entity.Company?.ArbName,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    ParentId: entity.ParentId,
                    ParentBranchName: entity.ParentBranch?.EngName ?? entity.ParentBranch?.ArbName,
                    CountryId: entity.CountryId,
                    CityId: entity.CityId,
                    DefaultAbsent: entity.DefaultAbsent,
                    PrepareDay: entity.PrepareDay,
                    AffectPeriod: entity.AffectPeriod,
                    Remarks: entity.Remarks,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}