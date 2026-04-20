using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Education.Dtos;
using Application.System.MasterData.Education.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.Education.Commands
{
    public static class CreateEducation
    {
        public record Command(CreateEducationDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg,IEducationRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new CreateEducationValidator(msg,repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IEducationRepository _repo;
            private readonly IValidationMessages _msg;
            public Handler(
                IEducationRepository repo, IValidationMessages msg,
                IContextService contextService)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {

                var codeExists = await _repo.CodeExistsAsync(request.Data.Code);
                if (codeExists)
                {
                    throw new ConflictException(_msg.CodeExists("Education", request.Data.Code));
                }

var entity = new Domain.System.MasterData.Education(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    level: request.Data.Level,
                    requiredYears: request.Data.RequiredYears,
                    remarks: request.Data.Remarks
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}