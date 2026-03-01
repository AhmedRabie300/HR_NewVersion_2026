 
 using MediatR;
using VenusHR.Application.Common.DTOs.Shared;
 using WorkFlow_EF;

namespace VenusHR.Application.Common.Interfaces.HR_Master.Religions
{
     public class GetAllReligionsQuery : IRequest<ApiResponse<object>>
    {
        public int Lang { get; set; }
        public GetAllReligionsQuery(int lang = 0) => Lang = lang;
    }

    public class GetReligionByIdQuery : IRequest<ApiResponse<object>>
    {
        public int Id { get; set; }
        public int Lang { get; set; }
        public GetReligionByIdQuery(int id, int lang = 0)
        {
            Id = id;
            Lang = lang;
        }
    }

     public class ReligionHandler :
        IRequestHandler<GetAllReligionsQuery, ApiResponse<object>>,
        IRequestHandler<GetReligionByIdQuery, ApiResponse<object>>
    {
        private readonly IHRMaster _hrMaster;

        public ReligionHandler(IHRMaster hrMaster) => _hrMaster = hrMaster;

        private ApiResponse<object> ConvertToApiResponse(GeneralOutputClass<object> result)
        {
            if (result.ErrorCode == 0)
                return ApiResponse<object>.Succeeded(result.ResultObject, result.ErrorMessage ?? "Success");
            else
                return ApiResponse<object>.Failed(result.ErrorMessage ?? "Failed", result.ErrorCode.ToString());
        }

        public async Task<ApiResponse<object>> Handle(GetAllReligionsQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetAllReligionsAsync(request.Lang));

        public async Task<ApiResponse<object>> Handle(GetReligionByIdQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetReligionByIdAsync(request.Id, request.Lang));
    }
}