using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.HICompanies.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.HICompanies.Queries
{
    public static class GetHICompanyById
    {
        public record Query(int Id) : IRequest<HICompanyDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly ICurrentUser _currentUser;

            public Validator(ICurrentUser currentUser)
            {
                _currentUser = currentUser;

                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(x => _currentUser.Language == 2
                        ? "المعرف يجب أن يكون أكبر من 0"
                        : "ID must be greater than 0");
            }
        }

        public class Handler : IRequestHandler<Query, HICompanyDto>
        {
            private readonly IHICompanyRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IHICompanyRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<HICompanyDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;

                var entity = await _repo.GetByIdAsync(request.Id);
               
                return new HICompanyDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    CompanyId: entity.CompanyId,
                    CompanyName: lang == 2 ? entity.Company?.ArbName : entity.Company?.EngName,
                    Remarks: entity.Remarks,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive(),
                    Classes: entity.Classes.Select(c => new HICompanyClassDto(
                        Id: c.Id,
                        HICompanyId: c.HICompanyId,
                        CompanyId: c.CompanyId,
                        CompanyName: lang == 2 ? c.Company?.ArbName : c.Company?.EngName,
                        EngName: c.EngName,
                        ArbName: c.ArbName,
                        ArbName4S: c.ArbName4S,
                        Remarks: c.Remarks,
                        CompanyAmount: c.CompanyAmount,
                        EmployeeAmount: c.EmployeeAmount,
                        RegDate: c.RegDate,
                        CancelDate: c.CancelDate,
                        IsActive: c.IsActive()
                    )).ToList()
                );
            }
        }
    }
}