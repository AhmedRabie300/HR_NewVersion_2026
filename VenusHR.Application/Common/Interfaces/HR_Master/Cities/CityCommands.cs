using MediatR;
using VenusHR.Application.Common.DTOs.Shared;
using VenusHR.Application.Common.Interfaces.HR_Master;
using VenusHR.Core.Master;
using WorkFlow_EF;

namespace VenusHR.Application.Common.Interfaces
{
    // =============== QUERIES ===============
    public class GetAllCitiesQuery : IRequest<ApiResponse<object>>
    {
        public int Lang { get; set; }
        public GetAllCitiesQuery(int lang = 0) => Lang = lang;
    }

    public class GetCityByIdQuery : IRequest<ApiResponse<object>>
    {
        public int Id { get; set; }
        public int Lang { get; set; }
        public GetCityByIdQuery(int id, int lang = 0)
        {
            Id = id;
            Lang = lang;
        }
    }

    // =============== HANDLER ===============
    public class CityHandler :
        IRequestHandler<GetAllCitiesQuery, ApiResponse<object>>,
        IRequestHandler<GetCityByIdQuery, ApiResponse<object>>
    {
        private readonly IHRMaster _hrMaster;

        public CityHandler(IHRMaster hrMaster) => _hrMaster = hrMaster;

        private ApiResponse<object> ConvertToApiResponse(GeneralOutputClass<object> result)
        {
            if (result.ErrorCode == 0)
                return ApiResponse<object>.Succeeded(result.ResultObject, result.ErrorMessage ?? "Success");
            else
                return ApiResponse<object>.Failed(result.ErrorMessage ?? "Failed", result.ErrorCode.ToString());
        }

        public async Task<ApiResponse<object>> Handle(GetAllCitiesQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetAllCitiesAsync(request.Lang));

        public async Task<ApiResponse<object>> Handle(GetCityByIdQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetCityByIdAsync(request.Id, request.Lang));
    }
}