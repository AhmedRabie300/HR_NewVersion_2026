using Application.System.MasterData.Abstractions;
using Application.System.MasterData.MaritalStatus.Dtos;
 using Application.Common.Abstractions;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;
using Application.Common;

namespace Application.System.MasterData.MaritalStatus.Commands
{
    public static class CreateMaritalStatus
    {
        public record Command(CreateMaritalStatusDto Data, int Lang = 1) : IRequest<int>;

 
        // No Validations Present
        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IMaritalStatusRepository _repo;
            private readonly ILocalizationService _localizer;

            public Handler(IMaritalStatusRepository repo, ILocalizationService localizer)
            {
                _repo = repo;
                _localizer = localizer;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _repo.CodeExistsAsync(request.Data.Code))
                    throw new ConflictException(string.Format(
                        _localizer.GetMessage("CodeExists", request.Lang),
                        _localizer.GetMessage("MaritalStatus", request.Lang),
                        request.Data.Code));

                var entity = new Domain.System.MasterData.MaritalStatus(
                    request.Data.Code,
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.Remarks,
                    request.Data.RegUserId,
                    request.Data.regComputerId
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}