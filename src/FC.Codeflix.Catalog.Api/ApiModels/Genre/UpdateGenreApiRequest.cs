namespace FC.Codeflix.Catalog.Api.ApiModels.Genre;

public class UpdateGenreApiRequest
{
    public string Name { get; set; }
    public bool? IsActive { get; set; }
    public List<Guid>? CategoriesId { get; set; }
    public UpdateGenreApiRequest(string name, bool? isActive = null, List<Guid>? categoriesId = null)
    {
        Name = name;
        IsActive = isActive;
        CategoriesId = categoriesId;
    }
}