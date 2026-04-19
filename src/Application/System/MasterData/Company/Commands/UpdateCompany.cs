using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Company.Dtos;
using Application.System.MasterData.Company.Validators;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.Company.Commands
{
    public static class UpdateCompany
    {
        public record Command(UpdateCompanyDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Data).SetValidator(new UpdateCompanyValidator(msg));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ICompanyRepository _repo;
                        private readonly IValidationMessages _msg;
public Handler(ICompanyRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var company = await _repo.GetByIdAsync(request.Data.Id);

                    company.UpdateBasicInfo(
                        request.Data.EngName,
                        request.Data.ArbName,
                        request.Data.ArbName4S,
                        request.Data.Remarks,
                        request.Data.DefaultTheme
                    );

                    company.UpdateEmployeeNaming(
                        request.Data.EmpFirstName,
                        request.Data.EmpSecondName,
                        request.Data.EmpThirdName,
                        request.Data.EmpFourthName,
                        request.Data.EmpNameSeparator
                    );

                    company.UpdateVacationSettings(
                        request.Data.VacationIsAccum,
                        request.Data.ZeroBalAfterVac,
                        request.Data.VacSettlement,
                        request.Data.AllowOverVacation,
                        request.Data.VacationFromPrepareDay,
                        request.Data.ExecuseRequestHoursallowed
                    );

                    company.UpdateSequenceSettings(
                        request.Data.HasSequence,
                        request.Data.SequenceLength,
                        request.Data.Prefix,
                        request.Data.Separator
                    );

                    company.UpdateFlags(
                        request.Data.IsHigry,
                        request.Data.IncludeAbsencDays,
                        request.Data.DefaultAttend,
                        request.Data.CountEmployeeVacationDaysTotal,
                        request.Data.EmployeeDocumentsAutoSerial,
                        request.Data.UserDepartmentsPermissions
                    );

                 if (request.Data.SalaryCalculation.HasValue)
                {
                    company.UpdateSalaryCalculation(request.Data.SalaryCalculation);
                }

                 if (request.Data.PrepareDay.HasValue)
                {
                    company.UpdatePrepareDay(request.Data.PrepareDay);
                }

                await _repo.UpdateAsync(company);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}