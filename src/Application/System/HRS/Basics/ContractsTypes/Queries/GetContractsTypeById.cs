using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.ContractsTypes.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.ContractsTypes.Queries
{
    public static class GetContractsTypeById
    {
        public record Query(int Id) : IRequest<ContractsTypeDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly ICurrentUser _currentUser;

            public Validator(ICurrentUser currentUser)
            {
                _currentUser = currentUser;
               
            }
        }

        public class Handler : IRequestHandler<Query, ContractsTypeDto>
        {
            private readonly IContractsTypeRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IContractsTypeRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<ContractsTypeDto> Handle(Query request, CancellationToken cancellationToken)
            {
 
                var entity = await _repo.GetByIdAsync(request.Id);
         
                return new ContractsTypeDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    CompanyId: entity.CompanyId,
                    IsSpecial: entity.IsSpecial,
                    Remarks: entity.Remarks,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}