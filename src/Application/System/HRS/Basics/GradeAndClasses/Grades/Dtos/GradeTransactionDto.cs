namespace Application.System.HRS.Basics.GradesAndClasses.Grades.Dtos
{
    public sealed record GradeTransactionDto(
        int Id,
        int GradeId,
        int TransactionTypeId,
        string? TransactionTypeName,
        int? CompanyId,
        string? CompanyName,
        decimal? MinValue,
        decimal? MaxValue,
        int? PaidAtVacation,
        bool? OnceAtPeriod,
        int? IntervalId,
        string? IntervalName,
        int? NumberOfTickets,
        string? Remarks,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}