using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace BeyondCloudPingConsole
{
    public class PingService : IPingService
    {
        private readonly BeyondCloudOptions _options;
        private readonly ILogger<PingService> _logger;

        public PingService(IOptions<BeyondCloudOptions> options, ILoggerFactory loggerFactory)
        {
            _options = options.Value;
            _logger = loggerFactory.CreateLogger<PingService>();
        }

        public void Ping()
        {
            try
            {
                _logger.LogInformation("Ping");
                var path = $"{Path.DirectorySeparatorChar}{Path.DirectorySeparatorChar}{_options.ServerName}{Path.DirectorySeparatorChar}{_options.ServerFolder}";
                _logger.LogInformation($"Path: {path}");
                var di = new DirectoryInfo(path);

                if (di.Exists)
                {
                    var result = di.GetDirectories();
                    _logger.LogInformation($"{_options.ServerFolder} directories: {result.Length}");
                }
                else
                    _logger.LogError("Path doesnt exist");
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"An error occurred in service: {ex.Message}");
                throw;
            }
        }
    }
}
