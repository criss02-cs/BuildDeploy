using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BuildDeploy.Models;
using SQLite;
using FileInfo = System.IO.FileInfo;

namespace BuildDeploy.Database;
public class DbService
{
    private readonly SQLiteAsyncConnection _database;

    public DbService()
    {
        _database = new SQLiteAsyncConnection(DbConstants.DatabasePath, DbConstants.Flags);
        _database.CreateTableAsync<Project>();
    }

    public async Task<List<Project>> GetAllProjects<T>(Expression<Func<Project, T>> orderBy, bool descending = false)
    {
        var table = _database.Table<Project>();
        table = descending ? table.OrderByDescending(orderBy) : table.OrderBy(orderBy);
        return await table.ToListAsync();
    }


    public async Task<bool> AddOrUpdateProject(Project project)
    {
        var existingProject = await _database.Table<Project>().FirstOrDefaultAsync(p => p.Id == project.Id);
        if (existingProject == null)
        {
            return await InsertNewProject(project);
        }
        return await UpdateProject(project);
    }

    private async Task<bool> InsertNewProject(Project project)
    {
        var result = await _database.InsertAsync(project);
        return result == 1;
    }

    private async Task<bool> UpdateProject(Project project)
    {
        var result = await _database.UpdateAsync(project);
        return result == 1;
    }
}
