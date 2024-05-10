using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BuildDeploy.Business.Entity;
public class FtpProfile
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Hostname { get; set; }
    public int Port { get; set; }

    public virtual List<Project> Projects { get; set; }
}
