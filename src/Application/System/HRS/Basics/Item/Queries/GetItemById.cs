using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.Items.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.Items.Queries
{
    public static class GetItemById
    {
        public record Query(int Id) : IRequest<ItemDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly ICurrentUser _currentUser;

            public Validator(ICurrentUser currentUser)
            {
                _currentUser = currentUser;

               
            }
        }

        public class Handler : IRequestHandler<Query, ItemDto>
        {
            private readonly IItemRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IItemRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<ItemDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;

                var entity = await _repo.GetByIdAsync(request.Id);
 
                return new ItemDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    PurchaseDate: entity.PurchaseDate,
                    PurchasePrice: entity.PurchasePrice,
                    ExpiryDate: entity.ExpiryDate,
                    LicenseNumber: entity.LicenseNumber,
                    CompanyId: entity.CompanyId,
                    CompanyName: lang == 2 ? entity.Company?.ArbName : entity.Company?.EngName,
                    IsFromAssets: entity.IsFromAssets,
                    Remarks: entity.Remarks,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}