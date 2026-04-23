using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.Items.Dtos;
using Application.System.HRS.Basics.Items.Validators;
using Domain.System.HRS.Basics;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.Items.Commands
{
    public static class CreateItem
    {
        public record Command(CreateItemDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IItemRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateItemValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IItemRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IItemRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId = _currentUser.CompanyId;

                var entity = new Item(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    purchaseDate: request.Data.PurchaseDate,
                    purchasePrice: request.Data.PurchasePrice,
                    expiryDate: request.Data.ExpiryDate,
                    licenseNumber: request.Data.LicenseNumber,
                    companyId: companyId,
                    isFromAssets: request.Data.IsFromAssets,
                    remarks: request.Data.Remarks,
                    regComputerId: request.Data.RegComputerId
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}