namespace Repositories.Repositories.CategoryRepository.Models;

public class CategoryDetail(Guid id, string name)
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
}