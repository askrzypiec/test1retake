

using Microsoft.Data.SqlClient;
using Test1retake.Controllers;
using Test1retake.Exceptions;
using Test1retake.Services;

public class DbService : IDbService
{
    private readonly string _connectionString;

    public DbService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default") ?? string.Empty;
    }

    public async Task<ProjectDetailsDto> GetProjectByIdAsync(int projectId)
    {
        const string query = @"
    SELECT 
        p.ProjectId, p.Objective, p.StartDate, p.EndDate,
        a.Name AS ArtifactName, a.OriginDate,
        i.InstitutionId, i.Name AS InstitutionName, i.FoundedYear,
        s.FirstName, s.LastName, s.HireDate, sa.Role
    FROM Preservation_Project p
    JOIN Artifact a ON p.ArtifactId = a.ArtifactId
    JOIN Institution i ON a.InstitutionId = i.InstitutionId
    LEFT JOIN Staff_Assignment sa ON sa.ProjectId = p.ProjectId
    LEFT JOIN Staff s ON sa.StaffId = s.StaffId
    WHERE p.ProjectId = @ProjectId";

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@ProjectId", projectId);

        await connection.OpenAsync();
        var reader = await command.ExecuteReaderAsync();

        ProjectDetailsDto? project = null;

        while (await reader.ReadAsync())
        {
            if (project == null)
            {
                project = new ProjectDetailsDto
                {
                    ProjectId = reader.GetInt32(0),
                    Objective = reader.GetString(1),
                    StartDate = reader.GetDateTime(2),
                    EndDate = await reader.IsDBNullAsync(3) ? null : reader.GetDateTime(3),
                    Artifact = new ArtifactDto
                    {
                        Name = reader.GetString(4),
                        OriginDate = reader.GetDateTime(5),
                        Institution = new InstitutionDto
                        {
                            InstitutionId = reader.GetInt32(6),
                            Name = reader.GetString(7),
                            FoundedYear = reader.GetInt32(8)
                        }
                    },
                    StaffAssignments = new List<StaffAssignmentDto>()
                };
            }


            if (!await reader.IsDBNullAsync(9))
            {
                var staffAssignment = new StaffAssignmentDto
                {
                    FirstName = reader.GetString(9),
                    LastName = reader.GetString(10),
                    HireDate = reader.GetDateTime(11),
                    Role = reader.GetString(12)
                };
                project.StaffAssignments.Add(staffAssignment);
            }
        }

        if (project == null)
            throw new NotFoundException($"Project with ID {projectId} not found.");

        return project;
    }
}


