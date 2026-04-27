using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.Grades.Commands
{
    public static class SoftDeleteGradeTransaction
    {
        public record Command(int Id, int? RegUserId = null) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly ICurrentUser _CurrentUser;
            private readonly ILocalizationService _localizer;

            public Validator(ICurrentUser CurrentUser, ILocalizationService localizer)
            {
                _CurrentUser = CurrentUser;
                _localizer = localizer;
                int lang = CurrentUser.Language;
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(_localizer.GetMessage("IdGreaterThanZero", lang));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IGradeRepository _repo;

            public Handler(IGradeRepository repo)
            {
                _repo = repo;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                await _repo.SoftDeleteTransactionAsync(request.Id, request.RegUserId);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}