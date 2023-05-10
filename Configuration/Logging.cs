using Serilog.Events;

namespace BloggerApi.Configuration {
  public class Logging {
    public int FileCountLimit { get; set; }

    public long FileSizeLimit { get; set; }

    public LogEventLevel LogLevel { get; set; }

    public string OutputTemplate { get; set; }

    public string PathFormat { get; set; }
  }
}