using MediatR;
using VenusHR.Application.Common.DTOs.Shared;
using VenusHR.Application.Common.Interfaces.HR_Master;
using VenusHR.Core.Master;
using WorkFlow_EF;

namespace VenusHR.Application.Common.Interfaces
{
    // =============== QUERIES ===============
    public class GetAllLocationsQuery : IRequest<ApiResponse<object>>
    {
        public int Lang { get; set; }
        public GetAllLocationsQuery(int lang = 0) => Lang = lang;
    }

    public class GetLocationByIdQuery : IRequest<ApiResponse<object>>
    {
        public int Id { get; set; }
        public int Lang { get; set; }
        public GetLocationByIdQuery(int id, int lang = 0)
        {
            Id = id;
            Lang = lang;
        }
    }

    public class GetLocationsByCityQuery : IRequest<ApiResponse<object>>
    {
        public int CityId { get; set; }
        public int Lang { get; set; }
        public GetLocationsByCityQuery(int cityId, int lang = 0)
        {
            CityId = cityId;
            Lang = lang;
        }
    }

    // =============== HANDLER ===============
    public class LocationHandler :
        IRequestHandler<GetAllLocationsQuery, ApiResponse<object>>,
        IRequestHandler<GetLocationByIdQuery, ApiResponse<object>>,
        IRequestHandler<GetLocationsByCityQuery, ApiResponse<object>>
    {
        private readonly IHRMaster _hrMaster;

        public LocationHandler(IHRMaster hrMaster) => _hrMaster = hrMaster;

        private ApiResponse<object> ConvertToApiResponse(GeneralOutputClass<object> result)
        {
            if (result.ErrorCode == 0)
                return ApiResponse<object>.Succeeded(result.ResultObject, result.ErrorMessage ?? "Success");
            else
                return ApiResponse<object>.Failed(result.ErrorMessage ?? "Failed", result.ErrorCode.ToString());
        }

        public async Task<ApiResponse<object>> Handle(GetAllLocationsQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetAllLocationsAsync(request.Lang));

        public async Task<ApiResponse<object>> Handle(GetLocationByIdQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetLocationByIdAsync(request.Id, request.Lang));

        public async Task<ApiResponse<object>> Handle(GetLocationsByCityQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetLocationsByCityAsync(request.CityId, request.Lang));
    }
}