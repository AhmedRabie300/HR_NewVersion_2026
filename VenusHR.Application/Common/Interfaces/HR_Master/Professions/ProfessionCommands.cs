using MediatR;
using VenusHR.Application.Common.DTOs.Shared;
using VenusHR.Application.Common.Interfaces.HR_Master;
using VenusHR.Core.Master;
using WorkFlow_EF;

namespace VenusHR.Application.Common.Interfaces
{
     public class GetAllProfessionsQuery : IRequest<ApiResponse<object>>
    {
        public int Lang { get; set; }
        public GetAllProfessionsQuery(int lang = 0) => Lang = lang;
    }

    public class GetProfessionByIdQuery : IRequest<ApiResponse<object>>
    {
        public int Id { get; set; }
        public int Lang { get; set; }
        public GetProfessionByIdQuery(int id, int lang = 0)
        {
            Id = id;
            Lang = lang;
        }
    }

     public class ProfessionHandler :
        IRequestHandler<GetAllProfessionsQuery, ApiResponse<object>>,
        IRequestHandler<GetProfessionByIdQuery, ApiResponse<object>>
    {
        private readonly IHRMaster _hrMaster;

        public ProfessionHandler(IHRMaster hrMaster) => _hrMaster = hrMaster;

        private ApiResponse<object> ConvertToApiResponse(GeneralOutputClass<object> result)
        {
            if (result.ErrorCode == 0)
                return ApiResponse<object>.Succeeded(result.ResultObject, result.ErrorMessage ?? "Success");
            else
                return ApiResponse<object>.Failed(result.ErrorMessage ?? "Failed", result.ErrorCode.ToString());
        }

        public async Task<ApiResponse<object>> Handle(GetAllProfessionsQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetAllProfessionsAsync(request.Lang));

        public async Task<ApiResponse<object>> Handle(GetProfessionByIdQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetProfessionByIdAsync(request.Id, request.Lang));
    }
}