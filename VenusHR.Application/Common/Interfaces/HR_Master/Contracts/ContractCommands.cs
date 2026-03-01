using MediatR;
using VenusHR.Application.Common.DTOs.Shared;
using VenusHR.Application.Common.Interfaces.HR_Master;
using VenusHR.Core.Master;
using WorkFlow_EF;

namespace VenusHR.Application.Common.Interfaces
{
     public class GetAllContractsQuery : IRequest<ApiResponse<object>>
    {
        public int Lang { get; set; }
        public GetAllContractsQuery(int lang = 0) => Lang = lang;
    }

    public class GetContractByIdQuery : IRequest<ApiResponse<object>>
    {
        public int Id { get; set; }
        public int Lang { get; set; }
        public GetContractByIdQuery(int id, int lang = 0)
        {
            Id = id;
            Lang = lang;
        }
    }

    // =============== COMMANDS ===============
    public class CreateContractCommand : IRequest<ApiResponse<object>>
    {
        public hrs_Contracts Contract { get; set; }
        public CreateContractCommand(hrs_Contracts contract) => Contract = contract;
    }

    public class UpdateContractCommand : IRequest<ApiResponse<object>>
    {
        public int Id { get; set; }
        public hrs_Contracts Contract { get; set; }
        public UpdateContractCommand(int id, hrs_Contracts contract)
        {
            Id = id;
            Contract = contract;
        }
    }

    // =============== HANDLER ===============
    public class ContractHandler :
        IRequestHandler<GetAllContractsQuery, ApiResponse<object>>,
        IRequestHandler<GetContractByIdQuery, ApiResponse<object>>,
        IRequestHandler<CreateContractCommand, ApiResponse<object>>,
        IRequestHandler<UpdateContractCommand, ApiResponse<object>>
    {
        private readonly IHRMaster _hrMaster;

        public ContractHandler(IHRMaster hrMaster) => _hrMaster = hrMaster;

        private ApiResponse<object> ConvertToApiResponse(GeneralOutputClass<object> result)
        {
            if (result.ErrorCode == 0)
                return ApiResponse<object>.Succeeded(result.ResultObject, result.ErrorMessage ?? "Success");
            else
                return ApiResponse<object>.Failed(result.ErrorMessage ?? "Failed", result.ErrorCode.ToString());
        }

        public async Task<ApiResponse<object>> Handle(GetAllContractsQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetAllContractsAsync(request.Lang));

        public async Task<ApiResponse<object>> Handle(GetContractByIdQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetContractByIdAsync(request.Id, request.Lang));

        public async Task<ApiResponse<object>> Handle(CreateContractCommand request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.CreateContractAsync(request.Contract));

        public async Task<ApiResponse<object>> Handle(UpdateContractCommand request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.UpdateContractAsync(request.Id, request.Contract));
    }
}