// Application/System/MasterData/DependantType/Queries/GetDependantTypeById.cs
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.DependantType.Dtos;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.DependantType.Queries
{
    public static class GetDependantTypeById
    {
        public record Query(int Id) : IRequest<DependantTypeDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly ILocalizationService _localizer;
            private readonly IContextService _ContextService;

            public Validator(ILocalizationService localizer, IContextService ContextService)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(x => _localizer.GetMessage("IdGreaterThanZero", _ContextService.GetCurrentLanguage()));
            }
        }

        public class Handler : IRequestHandler<Query, DependantTypeDto>
        {
            private readonly IDependantTypeRepository _repo;
            private readonly IContextService _ContextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                IDependantTypeRepository repo,
                IContextService ContextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _ContextService = ContextService;
                _localizer = localizer;
            }

     

            public async Task<DependantTypeDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = _ContextService.GetCurrentCompanyId();
                var lang = _ContextService.GetCurrentLanguage();

                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new Exception(string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("DependantType", lang),
                        request.Id));

                if (entity.CompanyId != companyId)
                    throw new UnauthorizedAccessException("Access denied: Dependant type does not belong to your company");

                return new DependantTypeDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    CompanyId: entity.CompanyId,
                    CompanyName: entity.Company?.EngName ?? entity.Company?.ArbName,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    Remarks: entity.Remarks,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}