using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.Items.Dtos;
using Application.System.HRS.Basics.Items.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.Items.Commands
{
    public static class UpdateItem
    {
        public record Command(UpdateItemDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IItemRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateItemValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IItemRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IItemRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("Item", request.Data.Id));

                entity.Update(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    purchaseDate: request.Data.PurchaseDate,
                    purchasePrice: request.Data.PurchasePrice,
                    expiryDate: request.Data.ExpiryDate,
                    licenseNumber: request.Data.LicenseNumber,
                    isFromAssets: request.Data.IsFromAssets,
                    remarks: request.Data.Remarks
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}