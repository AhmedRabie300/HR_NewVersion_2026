namespace Application.System.HRS.Basics.GradesAndClasses.Grades.Dtos
{ 
    public sealed record UpdateGradeTransactionDto(
        int Id,
        int GradeId,
        int TransactionTypeId,
        decimal? MinValue,
        decimal? MaxValue,
        int? PaidAtVacation,
        bool? OnceAtPeriod,
        int? IntervalId,
        int? NumberOfTickets,
        string? Remarks
    );
}