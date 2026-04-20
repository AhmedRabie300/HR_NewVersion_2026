using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.City.Dtos;
using Application.System.MasterData.City.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.City.Commands
{
    public static class CreateCity
    {
        public record Command(CreateCityDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg,ICityRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new CreateCityValidator(msg,repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ICityRepository _repo;
                        private readonly IValidationMessages _msg;
private readonly ICountryRepository _countryRepo;
            public Handler(
                ICityRepository repo, IValidationMessages msg,
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
                    throw new ConflictException(_msg.CodeExists("City", request.Data.Code));
                }

var entity = new Domain.System.MasterData.City(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    phoneKey: request.Data.PhoneKey,
                    regionId: request.Data.RegionId,
                    timeZone: request.Data.TimeZone,
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