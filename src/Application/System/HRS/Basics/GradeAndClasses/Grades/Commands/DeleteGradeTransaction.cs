using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.GradeAndClasses.Grades.Commands
{
    public static class DeleteGradeTransaction
    {
        public record Command(int Id) : IRequest<bool>;
 

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly IGradeRepository _repo;

            public Handler(IGradeRepository repo)
            {
                _repo = repo;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                if (!await _repo.TransactionExistsAsync(request.Id))
                    return false;

                await _repo.DeleteTransactionAsync(request.Id);
                await _repo.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}