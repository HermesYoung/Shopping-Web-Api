namespace Repositories.Common.Errors;

public class CategoryRepositoryError(string message, CategoryError data) : ErrorBase<CategoryError>(message, data)
{
    public static CategoryRepositoryError CreateCategoryNotFoundError(int categoryId) =>
       new CategoryRepositoryError("Category not found", new CategoryError(CategoryErrorType.CategoryNotFound, categoryId));

    public static CategoryRepositoryError CreateCategoryAlreadyExistError(int categoryId) =>
        new CategoryRepositoryError("Category already exists",
            new CategoryError(CategoryErrorType.CategoryAlreadyExists, categoryId));
}