namespace DatabaseContext.Entities;

public partial class OperationLog
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int UserId { get; set; }

    public string Operation { get; set; } = null!;

    public string Content { get; set; } = null!;

    public int? StatusCode { get; set; }

    public virtual User User { get; set; } = null!;
}
