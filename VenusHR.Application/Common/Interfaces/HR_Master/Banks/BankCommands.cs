using MediatR;
using VenusHR.Application.Common.DTOs.Shared;
using VenusHR.Application.Common.Interfaces.HR_Master;
using VenusHR.Core.Master;
using WorkFlow_EF;

namespace VenusHR.Application.Common.Interfaces.HR_Master.Banks
{
    public class GetAllBanksQuery : IRequest<ApiResponse<object>>
    {
        public int Lang { get; set; }
        public GetAllBanksQuery(int lang = 0) => Lang = lang;
    }

    public class GetBankByIdQuery : IRequest<ApiResponse<object>>
    {
        public int Id { get; set; }
        public int Lang { get; set; }
        public GetBankByIdQuery(int id, int lang = 0)
        {
            Id = id;
            Lang = lang;
        }
    }

    public class CreateBankCommand : IRequest<ApiResponse<object>>
    {
        public sys_Banks Bank { get; set; }
        public CreateBankCommand(sys_Banks bank) => Bank = bank;
    }

    public class UpdateBankCommand : IRequest<ApiResponse<object>>
    {
        public int Id { get; set; }
        public sys_Banks Bank { get; set; }
        public UpdateBankCommand(int id, sys_Banks bank)
        {
            Id = id;
            Bank = bank;
        }
    }

    public class BankHandler :
        IRequestHandler<GetAllBanksQuery, ApiResponse<object>>,
        IRequestHandler<GetBankByIdQuery, ApiResponse<object>>,
        IRequestHandler<CreateBankCommand, ApiResponse<object>>,
        IRequestHandler<UpdateBankCommand, ApiResponse<object>>
    {
        private readonly IHRMaster _hrMaster;

        public BankHandler(IHRMaster hrMaster) => _hrMaster = hrMaster;

        private ApiResponse<object> ConvertToApiResponse(GeneralOutputClass<object> result)
        {
            if (result.ErrorCode == 0)
                return ApiResponse<object>.Succeeded(result.ResultObject, result.ErrorMessage ?? "Success");
            else
                return ApiResponse<object>.Failed(result.ErrorMessage ?? "Failed", result.ErrorCode.ToString());
        }

        public async Task<ApiResponse<object>> Handle(GetAllBanksQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetAllBanksAsync(request.Lang));

        public async Task<ApiResponse<object>> Handle(GetBankByIdQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetBankByIdAsync(request.Id, request.Lang));

        public async Task<ApiResponse<object>> Handle(CreateBankCommand request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.CreateBankAsync(request.Bank));

        public async Task<ApiResponse<object>> Handle(UpdateBankCommand request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.UpdateBankAsync(request.Id, request.Bank));
    }
}