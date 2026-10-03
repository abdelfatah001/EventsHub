namespace Domain.Common
{
    public interface ICreatedAt
    {
        DateTime CreatedAt { get; set; }
    }

    public interface IUpdatedAt
    {
        DateTime? UpdatedAt { get; set; }
    }

    public interface IAuditable : ICreatedAt, IUpdatedAt
    {
    }
}
