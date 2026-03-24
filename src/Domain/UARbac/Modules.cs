using Domain.Common;

namespace Domain.UARbac
{
    public class Module : LegacyEntity
    {
         public string Code { get; private set; } = null!;
        public string? Prefix { get; private set; }
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }

         public int? FormId { get; private set; }

         public bool? IsRegistered { get; private set; }
        public bool? FiscalYearDependant { get; private set; }

         public int? Rank { get; private set; }

         public string? Remarks { get; private set; }

         public bool? IsAR { get; private set; }      
        public bool? IsAP { get; private set; }       
        public bool? IsGL { get; private set; }       
        public bool? IsFA { get; private set; }       
        public bool? IsINV { get; private set; }      
        public bool? IsHR { get; private set; }      
        public bool? IsMANF { get; private set; }     
        public bool? IsSYS { get; private set; }     

         public int? RegUserId { get; private set; }
        public int? regComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }

       
        private Module() { }  

        public Module(
            string code,
            string? prefix,
            string? engName,
            string? arbName,
            string? arbName4S,
            int? formId,
            bool? isRegistered,
            bool? fiscalYearDependant,
            int? rank,
            string? remarks,
            int? regUserId,
            int? regComputerId)
        {
            Code = code;
            Prefix = prefix;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            FormId = formId;
            IsRegistered = isRegistered;
            FiscalYearDependant = fiscalYearDependant;
            Rank = rank;
            Remarks = remarks;
            RegUserId = regUserId;
            regComputerId = regComputerId;
            RegDate = DateTime.UtcNow;

             IsAR = false;
            IsAP = false;
            IsGL = false;
            IsFA = false;
            IsINV = false;
            IsHR = false;
            IsMANF = false;
            IsSYS = false;
        }

         public void UpdateBasicInfo(
            string? engName,
            string? arbName,
            string? arbName4S,
            int? rank,
            string? remarks)
        {
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (rank.HasValue) Rank = rank;
            if (remarks != null) Remarks = remarks;
        }

        public void UpdateModuleTypes(
            bool? isAR,
            bool? isAP,
            bool? isGL,
            bool? isFA,
            bool? isINV,
            bool? isHR,
            bool? isMANF,
            bool? isSYS)
        {
            if (isAR.HasValue) IsAR = isAR;
            if (isAP.HasValue) IsAP = isAP;
            if (isGL.HasValue) IsGL = isGL;
            if (isFA.HasValue) IsFA = isFA;
            if (isINV.HasValue) IsINV = isINV;
            if (isHR.HasValue) IsHR = isHR;
            if (isMANF.HasValue) IsMANF = isMANF;
            if (isSYS.HasValue) IsSYS = isSYS;
        }

        public void UpdateRegistration(bool? isRegistered, bool? fiscalYearDependant)
        {
            if (isRegistered.HasValue) IsRegistered = isRegistered;
            if (fiscalYearDependant.HasValue) FiscalYearDependant = fiscalYearDependant;
        }

        public void UpdateForm(int? formId)
        {
            if (formId.HasValue) FormId = formId;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.UtcNow;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

         public bool IsActive() => !CancelDate.HasValue;

        public string GetName(int lang) => lang == 2 ? ArbName ?? EngName! : EngName ?? ArbName!;
    }
}