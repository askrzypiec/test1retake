namespace Test1retake.Services;

public interface IDbService
{ 
    Task<ProjectDetailsDto> GetProjectByIdAsync(int id);
}