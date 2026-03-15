namespace Domain.Common
{
    public abstract class LegacyEntity
    {
        public int Id { get; protected set; }

        public DateTime RegDate { get; protected set; }

        public int? RegUserId { get; protected set; }

        public void SetRegistration(DateTime regDate, int? regUserId)
        {
    
            if (RegDate == default) RegDate = regDate;
            if (RegUserId == null) RegUserId = regUserId;
        }
    }
}
