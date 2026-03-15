namespace Application.Abstractions
{
    public interface ICurrentUser
    {
        int? UserId { get; }
        bool IsAuthenticated { get; }
    }
}
