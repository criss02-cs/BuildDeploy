using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildDeploy.Models
{
    public class Project
    {
        public Guid Id { get; set; }
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
    }
}
