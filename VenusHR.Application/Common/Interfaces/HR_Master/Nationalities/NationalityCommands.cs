using MediatR;
using VenusHR.Application.Common.DTOs.Shared;
using VenusHR.Application.Common.Interfaces.HR_Master;
using VenusHR.Core.Master;
using WorkFlow_EF;

namespace VenusHR.Application.Common.Interfaces
{
 
    public class GetAllNationalitiesQuery : IRequest<ApiResponse<object>>
    {
        public int Lang { get; set; }
        public GetAllNationalitiesQuery(int lang = 0) => Lang = lang;
    }

    public class GetNationalityByIdQuery : IRequest<ApiResponse<object>>
    {
        public int Id { get; set; }
        public int Lang { get; set; }
        public GetNationalityByIdQuery(int id, int lang = 0)
        {
            Id = id;
            Lang = lang;
        }
    }

 
    public class CreateNationalityCommand : IRequest<ApiResponse<object>>
    {
        public sys_Nationalities Nationality { get; set; }
        public CreateNationalityCommand(sys_Nationalities nationality) => Nationality = nationality;
    }

    public class UpdateNationalityCommand : IRequest<ApiResponse<object>>
    {
        public int Id { get; set; }
        public sys_Nationalities Nationality { get; set; }
        public UpdateNationalityCommand(int id, sys_Nationalities nationality)
        {
            Id = id;
            Nationality = nationality;
        }
    }

 
    public class NationalityHandler :
        IRequestHandler<GetAllNationalitiesQuery, ApiResponse<object>>,
        IRequestHandler<GetNationalityByIdQuery, ApiResponse<object>>,
        IRequestHandler<CreateNationalityCommand, ApiResponse<object>>,
        IRequestHandler<UpdateNationalityCommand, ApiResponse<object>>
    {
        private readonly IHRMaster _hrMaster;

        public NationalityHandler(IHRMaster hrMaster) => _hrMaster = hrMaster;

        private ApiResponse<object> ConvertToApiResponse(GeneralOutputClass<object> result)
        {
            if (result.ErrorCode == 0)
                return ApiResponse<object>.Succeeded(result.ResultObject, result.ErrorMessage ?? "Success");
            else
                return ApiResponse<object>.Failed(result.ErrorMessage ?? "Failed", result.ErrorCode.ToString());
        }

        public async Task<ApiResponse<object>> Handle(GetAllNationalitiesQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetAllNationalitiesAsync(request.Lang));

        public async Task<ApiResponse<object>> Handle(GetNationalityByIdQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetNationalityByIdAsync(request.Id, request.Lang));

        public async Task<ApiResponse<object>> Handle(CreateNationalityCommand request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.CreateNationalityAsync(request.Nationality));

        public async Task<ApiResponse<object>> Handle(UpdateNationalityCommand request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.UpdateNationalityAsync(request.Id, request.Nationality));
    }
}