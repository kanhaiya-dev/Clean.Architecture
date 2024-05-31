using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Architecture.Core.Common.Utility
{
    public class GitHubSettings
    {
        public const string ConfigurationSection = "GitHub";

        public string AccessToken { get; init; } = string.Empty;

        public string UserAgent { get; init; } = string.Empty;
    }
}
