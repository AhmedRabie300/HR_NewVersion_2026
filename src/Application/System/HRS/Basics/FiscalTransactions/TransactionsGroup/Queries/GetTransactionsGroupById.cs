using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalTransactions.TransactionsGroup.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.FiscalTransactions.TransactionsGroup.Queries
{
    public static class GetTransactionsGroupById
    {
        public record Query(int Id) : IRequest<TransactionsGroupDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly ILocalizationService _localizer;
            private readonly IContextService _contextService;

            public Validator(ILocalizationService localizer, IContextService contextService)
            {
                _localizer = localizer;
                _contextService = contextService;

                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(x => _localizer.GetMessage("IdGreaterThanZero", _contextService.GetCurrentLanguage()));
            }
        }

        public class Handler : IRequestHandler<Query, TransactionsGroupDto>
        {
            private readonly ITransactionsGroupRepository _repo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;
            private readonly IValidationMessages _msg;

            public Handler(ITransactionsGroupRepository repo, IContextService contextService, ILocalizationService localizer, IValidationMessages msg)
            {
                _repo = repo;
                _contextService = contextService;
                _localizer = localizer;
                _msg = msg;
            }

            public async Task<TransactionsGroupDto> Handle(Query request, CancellationToken cancellationToken)
            {
 
                var entity = await _repo.GetByIdAsync(request.Id);
           
    

                return new TransactionsGroupDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    CompanyId: entity.CompanyId,
                    CompanyName: entity.Company?.EngName,
                    Remarks: entity.Remarks,
                    RegComputerId: entity.RegComputerId,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}