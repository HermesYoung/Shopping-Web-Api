namespace Repositories.Common.Errors;

public class CategoryError
{
    public CategoryErrorType Type { get; }
    public int CategoryId { get; }
    public int? ParentId { get; }

    public CategoryError(CategoryErrorType type, int categoryId, int? parentId = null)
    {
        Type = type;
        CategoryId = categoryId;
        ParentId = parentId;
    }
}