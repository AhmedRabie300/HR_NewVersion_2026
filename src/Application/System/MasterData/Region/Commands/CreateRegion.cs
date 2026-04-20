using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Region.Dtos;
using Application.System.MasterData.Region.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Region.Commands
{
    public static class CreateRegion
    {
        public record Command(CreateRegionDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IRegionRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new CreateRegionValidator(msg,repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IRegionRepository _repo;
                        private readonly IValidationMessages _msg;
private readonly ICountryRepository _countryRepo;
            public Handler(
                IRegionRepository repo, IValidationMessages msg,
                ICountryRepository countryRepo)
            {
                _repo = repo;
                _msg = msg;
                _countryRepo = countryRepo;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var codeExists = await _repo.CodeExistsAsync(request.Data.Code);
                if (codeExists)
                {
                    throw new ConflictException(_msg.CodeExists("Region", request.Data.Code));
                }

                var country = await _countryRepo.GetByIdAsync(request.Data.CountryId);

                var entity = new Domain.System.MasterData.Region(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    countryId: request.Data.CountryId,
                    remarks: request.Data.Remarks
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}