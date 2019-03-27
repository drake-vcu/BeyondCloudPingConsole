using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Steeltoe.Common.Net;
using System;
using System.IO;
using System.Net;

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

                if (string.IsNullOrEmpty(_options.Username) || string.IsNullOrEmpty(_options.Password))
                    throw new ArgumentException("username or password empty");

                var credentials = new NetworkCredential(_options.Username, _options.Password);
                var path = $"{Path.DirectorySeparatorChar}{Path.DirectorySeparatorChar}{_options.ServerName}{Path.DirectorySeparatorChar}{_options.ServerFolder}";
                using (WindowsNetworkFileShare networkPath = new WindowsNetworkFileShare(path, credentials))
                {
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
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"An error occurred in service: {ex.Message}");
                //throw;
            }
        }
    }
}
