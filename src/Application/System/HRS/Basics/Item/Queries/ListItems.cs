using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.Items.Dtos;
using MediatR;

namespace Application.System.HRS.Basics.Items.Queries
{
    public static class ListItems
    {
        public record Query : IRequest<List<ItemDto>>;

        public class Handler : IRequestHandler<Query, List<ItemDto>>
        {
            private readonly IItemRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IItemRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<List<ItemDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;
                var companyId = _currentUser.CompanyId;

                var items = await _repo.GetByCompanyIdAsync(companyId);

                return items.Select(x => new ItemDto(
                    Id: x.Id,
                    Code: x.Code,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    PurchaseDate: x.PurchaseDate,
                    PurchasePrice: x.PurchasePrice,
                    ExpiryDate: x.ExpiryDate,
                    LicenseNumber: x.LicenseNumber,
                    CompanyId: x.CompanyId,
                    CompanyName: lang == 2 ? x.Company?.ArbName : x.Company?.EngName,
                    IsFromAssets: x.IsFromAssets,
                    Remarks: x.Remarks,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive()
                )).ToList();
            }
        }
    }
}