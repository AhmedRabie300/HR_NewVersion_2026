using VenusHR.Application.Common.DTOs.Lookups;
using VenusHR.Core.Master;

namespace VenusHR.Application.Common.Mappers
{
    public static class BloodGroupMapper
    {
        // Entity → DTO
        public static BloodGroupDto ToDto(this hrs_BloodGroups entity)
        {
            if (entity == null) return null;

            return new BloodGroupDto
            {
                ID = entity.ID,
                Code = entity.Code,
                EngName = entity.EngName,
                ArbName = entity.ArbName,
                ArbName4S = entity.ArbName4S,
                Remarks = entity.Remarks,
                CancelDate = entity.CancelDate
            };
        }

        // Create DTO → Entity
        public static hrs_BloodGroups ToEntity(this CreateBloodGroupDto dto, int userId = 1, int computerId = 1)
        {
            return new hrs_BloodGroups
            {
                Code = dto.Code,
                EngName = dto.EngName,
                ArbName = dto.ArbName,
                ArbName4S = dto.ArbName4S,
                Remarks = dto.Remarks,
                RegUserID = userId,
                RegComputerID = computerId,
                RegDate = DateTime.Now,
                CancelDate = DateTime.MaxValue
            };
        }

        // Update DTO → Entity (for updates)
        public static void UpdateEntity(this UpdateBloodGroupDto dto, hrs_BloodGroups entity)
        {
            if (entity == null) return;

            entity.Code = dto.Code;
            entity.EngName = dto.EngName;
            entity.ArbName = dto.ArbName;
            entity.ArbName4S = dto.ArbName4S;
            entity.Remarks = dto.Remarks;
        }
    }
}