using Application.Common.Abstractions;
using Application.UARbac.Abstractions;
using Application.UARbac.FormControls.Dtos;
using FluentValidation;
using MediatR;

namespace Application.UARbac.FormControls;

public static class ListByFormId
{
    public sealed record Query(int FormId, int? Section) : IRequest<List<GetFormControlDto>>;

    public sealed class Validator : AbstractValidator<Query>
    {
        public Validator(IValidationMessages msg)
        {
            RuleFor(x => x.FormId)
                .GreaterThan(0)
                .WithMessage(msg.Get("IdGreaterThanZero"));

            RuleFor(x => x.Section)
                .Must(s => !s.HasValue || s.Value > 0)
                .WithMessage(msg.Get("IdGreaterThanZero"));
        }
    }

    public sealed class Handler : IRequestHandler<Query, List<GetFormControlDto>>
    {
        private readonly IFormControlRepository _repo;
        public Handler(IFormControlRepository repo) => _repo = repo;

        public async Task<List<GetFormControlDto>> Handle(Query request, CancellationToken ct)
        {
            var items = await _repo.ListByFormIdAsync(request.FormId, request.Section, ct);

            return items.Select(x => new GetFormControlDto(
                x.Id,
                x.FormId,
                x.Name,
                x.FieldName,
                x.Format,
                x.Section,
                x.EngCaption,
                x.ArbCaption,
                x.Compulsory,
                x.IsHide,
                x.IsDisabled,
                x.SearchID,
                x?.Rank
            )).ToList();
        }
    }
}
