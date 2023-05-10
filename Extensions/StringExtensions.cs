using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BloggerApi.Extensions {
  public static class StringExtensions {
    public static string Center(this string s, char c, int w) {
      var p = Math.Max(0, (w - s.Length) / 2);

      if (p == 0) {
        return s;
      }

      if (p * 2 + s.Length != w) {
        s += " ";
      }

      var pl = $"{new string(c, p - 1)} ";
      var pr = $" {new string(c, p - 1)}";
      var result = $"{pl}{s}{pr}";

      return result;
    }

    public static string Clean(this string s) {
      var result =
        string.IsNullOrWhiteSpace(s)
          ? null
          : s;

      return result;
    }

    public static bool IsMultiline(this string s) {
      return s != null && s.Trim().Contains(Environment.NewLine);
    }

    public static string SqlEncode(this string s) {
      if (string.IsNullOrWhiteSpace(s)) {
        return null;
      }

      var result = s.Replace("'", "''").Replace("\"", "\"\"");

      return result;
    }

    public static DateTime ToDate(this string s) {
      var exception = new Exception("The date is invalid");

      if (string.IsNullOrWhiteSpace(s)) {
        throw exception;
      }

      var valid = DateTime.TryParseExact(s, "M/d/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out var result);

      if (!valid) {
        valid = DateTime.TryParseExact(s, "MMddyyyy", new CultureInfo("en-US"), DateTimeStyles.None, out result);
      }

      if (!valid) {
        valid = DateTime.TryParseExact(s, "yyyyMMdd", new CultureInfo("en-US"), DateTimeStyles.None, out result);
      }

      if (!valid) {
        valid = DateTime.TryParseExact(s, "yyyyMMddHH", new CultureInfo("en-US"), DateTimeStyles.None, out result);
      }

      if (!valid) {
        valid = DateTime.TryParseExact(s, "yyyyMMddHHzzz", new CultureInfo("en-US"), DateTimeStyles.None, out result);
      }

      if (!valid) {
        valid = DateTime.TryParseExact(s, "yyyyMMddHHmm", new CultureInfo("en-US"), DateTimeStyles.None, out result);
      }

      if (!valid) {
        valid = DateTime.TryParseExact(s, "yyyyMMddHHmmzzz", new CultureInfo("en-US"), DateTimeStyles.None, out result);
      }

      if (!valid) {
        valid = DateTime.TryParseExact(s, "yyyyMMddHHmmss", new CultureInfo("en-US"), DateTimeStyles.None, out result);
      }

      if (!valid) {
        valid = DateTime.TryParseExact(s, "yyyyMMddHHmmsszzz", new CultureInfo("en-US"), DateTimeStyles.None, out result);
      }

      if (!valid) {
        valid = DateTime.TryParseExact(s, "M/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out result);
      }

      if (!valid) {
        valid = DateTime.TryParseExact(s, "yyyyMM", new CultureInfo("en-US"), DateTimeStyles.None, out result);
      }

      if (!valid) {
        valid = DateTime.TryParseExact(s, "yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out result);
      }

      if (!valid) {
        valid = DateTime.TryParseExact(s, "M/d/yyyy H:mm", new CultureInfo("en-US"), DateTimeStyles.None, out result);
      }

      if (!valid) {
        valid = DateTime.TryParse(s, out result);
      }

      if (!valid) {
        throw exception;
      }

      var date = DateTime.UtcNow.AddYears(-150);

      if (result < date) {
        throw exception;
      }

      return result;
    }

    public static string ToLetterString(this string s) {
      var result =
        s == null
          ? null
          : Regex.Replace(s, "[^A-Za-z]", string.Empty, RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));

      return result;
    }

    public static DateTime? ToNullableDate(this string s) {
      if (string.IsNullOrWhiteSpace(s)) {
        return null;
      }

      var result = s.ToDate();

      return result;
    }

    public static string ToNumberString(this string s, params string[] prefixes) {
      var result =
        s == null
          ? null
          : Regex.Replace(s, "[^0-9]", string.Empty, RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));

      if (result == null) {
        return null;
      }

      if (prefixes == null) {
        return result;
      }

      foreach (var prefix in prefixes) {
        if (s.StartsWith(prefix)) {
          return prefix + result;
        }
      }

      return result;
    }

    public static string ToPhone(this string s) {
      if (string.IsNullOrWhiteSpace(s)) {
        return null;
      }

      if (s.StartsWith("+")) {
        return s;
      }

      return
        s.Length switch {
          11 => $"{s[..1]} ({s.Substring(1, 3)}) {s.Substring(4, 3)}-{s.Substring(7, 4)}",
          10 => $"({s[..3]}) {s.Substring(3, 3)}-{s.Substring(6, 4)}",
          7 => $"{s[..3]}-{s.Substring(3, 4)}",
          _ => s
        };
    }

    public static string ToPostalCode(this string s) {
      if (string.IsNullOrWhiteSpace(s)) {
        return null;
      }

      if (s.Length == 5) {
        return s;
      }

      var result = $"{s.Substring(0, 5)}-{s.Substring(5, 4)}";

      return result;
    }

    public static string ToReadable(this string s) {
      var result =
        s == null
          ? null
          : Regex.Replace(s, "([a-z]|[A-Z]{2,})([A-Z])", "$1 $2");

      return result;
    }

    public static string ToSentenceCase(this string s) {
      if (s == null) {
        return null;
      }

      var builder = new StringBuilder(s.Length + Regex.Matches(s, "[A-Z]").Count);
      var buffer = new Stack<char>();
      var start = true;

      for (var i = 0; i < s.Length; i++) {
        var c =
          i == 0
            ? char.ToUpper(s[i])
            : s[i];

        if (char.IsUpper(c)) {
          buffer.Push(c);
        }
        else {
          if (buffer.Count != 0) {
            var l = char.ToLower(buffer.Pop());

            var acronym =
              buffer.Count == 0
                ? null
                : string.Join(null, buffer.Reverse());

            if (start) {
              start = false;
            }
            else {
              builder.Append(" ");
            }

            if (acronym == null) {
              builder.Append(builder.Length == 0 ? char.ToUpper(l) : l);
            }
            else {
              builder
                .Append(acronym)
                .Append(" ")
                .Append(l);
            }

            buffer.Clear();
          }

          builder.Append(c);
        }
      }

      if (buffer.Count == 0) {
        return builder.ToString();
      }

      if (!start) {
        builder.Append(" ");
      }

      builder.Append(string.Join(null, buffer.Reverse()));

      return builder.ToString();
    }

    private static TimeZoneInfo GetTimeZone(string id) {
      return id switch {
        "AST" => TimeZoneInfo.FindSystemTimeZoneById("Atlantic Standard Time"),
        "ADT" => TimeZoneInfo.FindSystemTimeZoneById("Atlantic Standard Time"),
        "EST" => TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"),
        "EDT" => TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"),
        "CST" => TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"),
        "CDT" => TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"),
        "MST" => TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"),
        "MDT" => TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"),
        "PST" => TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"),
        "PDT" => TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"),
        "AKST" => TimeZoneInfo.FindSystemTimeZoneById("Alaskan Standard Time"),
        "AKDT" => TimeZoneInfo.FindSystemTimeZoneById("Alaskan Standard Time"),
        "HST" => TimeZoneInfo.FindSystemTimeZoneById("Hawaiian Standard Time"),
        "HDT" => TimeZoneInfo.FindSystemTimeZoneById("Hawaiian Standard Time"),
        "HAST" => TimeZoneInfo.FindSystemTimeZoneById("Hawaiian Standard Time"),
        "HADT" => TimeZoneInfo.FindSystemTimeZoneById("Hawaiian Standard Time"),
        "SST" => TimeZoneInfo.FindSystemTimeZoneById("Samoa Standard Time"),
        "SDT" => TimeZoneInfo.FindSystemTimeZoneById("Samoa Standard Time"),
        _ => throw new InvalidOperationException($"ID is unsupported: {id}")
      };
    }
  }
}