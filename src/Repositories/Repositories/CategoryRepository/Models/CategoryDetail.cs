namespace Repositories.Repositories.CategoryRepository.Models;

public class CategoryDetail(int id, string name)
{
    public int Id { get; } = id;
    public string Name { get; } = name;
}