namespace Envora.Web.Models.Projects;

public sealed class CreateProjectRequest
{
    public string ProjectNumber { get; set; } = "";
    public string ProjectName { get; set; } = "";
    public string Status { get; set; } = "Conceptual";
}


