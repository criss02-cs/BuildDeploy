using System.IO;
using System.Linq.Expressions;
using System.Text;
using BuildDeployWpf.Models;
using Microsoft.EntityFrameworkCore;
using SQLite;

namespace BuildDeployWpf.Database;
public class DbService
{
    private static DbService? _instance;

    public static DbService Instance => _instance ??= new DbService();

    private DbService()
    {
    }

    public async Task<List<Project>> GetAllProjects<T>(Func<Project, T> orderBy, bool descending = false)
    {
        var projects = await DatabaseContext.Instance.Projects
            .ToListAsync();
        projects = descending ? [.. projects.OrderByDescending(orderBy)] : [.. projects.OrderBy(orderBy)];
        // controllo se ancora il progetto esiste, se non esiste lo elimino dal database
        foreach (var project in projects.ToList().Where(project => !File.Exists(@$"{project.Path}\{project.Name}")))
        {
            projects.Remove(project);
            DatabaseContext.Instance.Projects.Remove(project);
            await DatabaseContext.Instance.SaveChangesAsync();
        }
        return projects;
    }

    public async Task<Project?> GetProjectById(int id) =>
        await DatabaseContext.Instance.Projects.FirstOrDefaultAsync(x => x.Id == id);


    public async Task<bool> AddOrUpdateProject(Project project)
    {
        var existingProject = await DatabaseContext.Instance.Projects.FirstOrDefaultAsync(p => p.Id == project.Id);
        if (existingProject == null)
        {
            return await InsertNewProject(project);
        }
        return await UpdateProject(project);
    }

    private async Task<bool> InsertNewProject(Project project)
    {
        DatabaseContext.Instance.Projects.Add(project);
        var result = await DatabaseContext.Instance.SaveChangesAsync();
        return result == 1;
    }

    private async Task<bool> UpdateProject(Project project)
    {
        DatabaseContext.Instance.Projects.Update(project);
        var result = await DatabaseContext.Instance.SaveChangesAsync();
        return result == 1;
    }
}
