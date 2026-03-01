using MediatR;
using VenusHR.Application.Common.DTOs.Shared;
using VenusHR.Application.Common.Interfaces.HR_Master;
using VenusHR.Core.Master;
using WorkFlow_EF;

namespace VenusHR.Application.Common.Interfaces
{
    // =============== QUERIES ===============
    public class GetAllCompaniesQuery : IRequest<ApiResponse<object>>
    {
        public int Lang { get; set; }
        public GetAllCompaniesQuery(int lang = 0) => Lang = lang;
    }

    public class GetCompanyByIdQuery : IRequest<ApiResponse<object>>
    {
        public int Id { get; set; }
        public int Lang { get; set; }
        public GetCompanyByIdQuery(int id, int lang = 0)
        {
            Id = id;
            Lang = lang;
        }
    }

    // =============== HANDLER ===============
    public class CompanyHandler :
        IRequestHandler<GetAllCompaniesQuery, ApiResponse<object>>,
        IRequestHandler<GetCompanyByIdQuery, ApiResponse<object>>
    {
        private readonly IHRMaster _hrMaster;

        public CompanyHandler(IHRMaster hrMaster) => _hrMaster = hrMaster;

        private ApiResponse<object> ConvertToApiResponse(GeneralOutputClass<object> result)
        {
            if (result.ErrorCode == 0)
                return ApiResponse<object>.Succeeded(result.ResultObject, result.ErrorMessage ?? "Success");
            else
                return ApiResponse<object>.Failed(result.ErrorMessage ?? "Failed", result.ErrorCode.ToString());
        }

        public async Task<ApiResponse<object>> Handle(GetAllCompaniesQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetAllCompaniesAsync(request.Lang));

        public async Task<ApiResponse<object>> Handle(GetCompanyByIdQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetCompanyByIdAsync(request.Id, request.Lang));
    }
}