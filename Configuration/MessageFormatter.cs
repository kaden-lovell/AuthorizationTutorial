using System;
using System.IO;
using System.Linq;
using System.Text;
using BloggerApi.Extensions;
using Serilog.Events;
using Serilog.Formatting;

namespace BloggerApi.Configuration {
  public class MessageFormatter : ITextFormatter {
    public void Format(LogEvent logEvent, TextWriter output) {
      var level = GetLevel(logEvent);
      var sourceContext = GetSourceContext(logEvent);
      var scope = GetScope(logEvent);
      var message = logEvent.RenderMessage().Trim();
      var exception = logEvent.Exception?.ToString().Trim();
      var builder = new StringBuilder();

      builder.Append($"{logEvent.Timestamp:yyyy-MM-dd hh:mm:ss tt zzz} [{level}] {(sourceContext == null ? null : $"[{sourceContext}] ")}{(scope == null ? null : $"{scope} ")}");

      if (message.IsMultiline()) {
        builder
          .AppendLine()
          .AppendLine("message".Center('-', 132))
          .AppendLine()
          .AppendLine(message)
          .AppendLine();

        if (!string.IsNullOrWhiteSpace(exception)) {
          builder
            .AppendLine("exception".Center('-', 132))
            .AppendLine()
            .AppendLine(exception)
            .AppendLine();
        }

        builder
          .AppendLine("end".Center('-', 132));
      }
      else {
        builder.AppendLine(message);

        if (!string.IsNullOrWhiteSpace(exception)) {
          builder
            .AppendLine("exception".Center('-', 132))
            .AppendLine()
            .AppendLine(exception)
            .AppendLine()
            .AppendLine("end".Center('-', 132));
        }
      }

      output.Write(builder.ToString());
    }

    private static string GetLevel(LogEvent logEvent) {
      return logEvent.Level switch {
        LogEventLevel.Verbose => "VRB",
        LogEventLevel.Debug => "DBG",
        LogEventLevel.Information => "INF",
        LogEventLevel.Warning => "WRN",
        LogEventLevel.Error => "ERR",
        LogEventLevel.Fatal => "FTL",
        _ => throw new InvalidOperationException($"Unsupported level: {logEvent.Level}")
      };
    }

    private static string GetScope(LogEvent logEvent) {
      logEvent.Properties.TryGetValue("Scope", out var value);

      var result = value?.ToString();

      return result;
    }

    private static string GetSourceContext(LogEvent logEvent) {
      logEvent.Properties.TryGetValue("SourceContext", out var value);

      if (value == null) {
        return null;
      }

      var tokens = value.ToString().Replace("\"", null).Split(".");

      var result =
        tokens[0] == "Connect"
          ? tokens.Last()
          : null;

      return result;
    }
  }
}