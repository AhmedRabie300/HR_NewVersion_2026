using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Education.Dtos;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Education.Queries
{
    public static class GetEducationById
    {
        public record Query(int Id, int Lang = 1) : IRequest<EducationDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(ILocalizationService localizer)
            {
                RuleFor(x => x.Id).GreaterThan(0).WithMessage(localizer.GetMessage("IdGreaterThanZero", 1));
            }
        }

        public class Handler : IRequestHandler<Query, EducationDto>
        {
            private readonly IEducationRepository _repo;

            public Handler(IEducationRepository repo)
            {
                _repo = repo;
            }

            public async Task<EducationDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new Exception($"Education with ID {request.Id} not found");

                return new EducationDto(
                    entity.Id,
                    entity.Code,
                    entity.CompanyId,
                    entity.Company?.EngName ?? entity.Company?.ArbName,
                    entity.EngName,
                    entity.ArbName,
                    entity.ArbName4S,
                    entity.Level,
                    entity.RequiredYears,
                    entity.Remarks,
                    entity.RegDate,
                    entity.CancelDate,
                    entity.IsActive()
                );
            }
        }
    }
}