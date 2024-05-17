using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BuildDeploy.Business.Entity;

public class Language
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string? Name { get; set; } // like C#, Angular, etc.
    public string? ExecutableName { get; set; } // like dotnet, ng, etc.
    public string? BuildCommand { get; set; } // like dotnet build, ng build, etc.
    public string? ArgumentsJson { get; set; }
    public bool IsJavascriptFramework { get; set; } // mi serve di saperlo per poter controllare se esiste la cli globale

    [NotMapped]
    public Dictionary<string, string>? Arguments
    {
        get
        {
            if (ArgumentsJson is null) return [];
            return JsonSerializer.Deserialize<Dictionary<string, string>>(ArgumentsJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        set =>
            ArgumentsJson =
                JsonSerializer.Serialize(value, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public List<Project> Projects { get; set; }

}