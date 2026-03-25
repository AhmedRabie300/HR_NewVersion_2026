using Application.System.MasterData.Abstractions;
using Application.System.MasterData.DocumentTypesGroup.Dtos;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.DocumentTypesGroup.Queries
{
    public static class GetDocumentTypesGroupById
    {
        public record Query(int Id, int Lang = 1) : IRequest<DocumentTypesGroupDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(ILocalizationService localizer)
            {
                RuleFor(x => x.Id).GreaterThan(0).WithMessage(localizer.GetMessage("IdGreaterThanZero", 1));
            }
        }

        public class Handler : IRequestHandler<Query, DocumentTypesGroupDto>
        {
            private readonly IDocumentTypesGroupRepository _repo;

            public Handler(IDocumentTypesGroupRepository repo)
            {
                _repo = repo;
            }

            public async Task<DocumentTypesGroupDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new Exception($"DocumentTypesGroup with ID {request.Id} not found");

                return new DocumentTypesGroupDto(
                    entity.Id,
                    entity.Code,
                    entity.EngName,
                    entity.ArbName,
                    entity.Remarks,
                    entity.RegDate,
                    entity.CancelDate,
                    entity.IsActive()
                );
            }
        }
    }
}