using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildDeploy.Business.Entity;
using Microsoft.EntityFrameworkCore;

namespace BuildDeploy.Business.Database;
public class LanguagesManager
{
    public async Task InitDatabase(List<Language> defaultLanguages)
    {
        var languages = await DatabaseContext.Instance.Languages.ToListAsync();
        foreach (var language in defaultLanguages)
        {
            if (languages.All(x => x.Id != language.Id))
            {
                language.Id = 0;
                await DatabaseContext.Instance.Languages.AddAsync(language);
            }
        }
        await DatabaseContext.Instance.SaveChangesAsync();
    }
}
