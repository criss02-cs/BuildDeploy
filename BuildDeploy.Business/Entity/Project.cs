using SQLite;

namespace BuildDeploy.Business.Entity;
public class Project
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    /// <summary>
    /// Nome del progetto
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// Percorso del file .csproj
    /// </summary>
    public string? Path { get; set; }
    /// <summary>
    /// Data dell'ultima apertura del progetto
    /// </summary>
    public DateTime LastTimeOpened { get; set; }
    /// <summary>
    /// Property che rappresenta il percorso nell'ambiente di produzione
    /// </summary>
    public string? DefaultDeployPath { get; set; }
    /// <summary>
    /// Property che rappresenta il percorso locale di dove si trova la build in release
    /// </summary>
    public string? DefaultReleasePath { get; set; }
    public int? FtpProfileId { get; set; }
    public virtual FtpProfile? FtpProfile { get; set; }
}
