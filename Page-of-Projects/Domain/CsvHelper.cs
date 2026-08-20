// --Copyright (c) 2026 Robert A. Howell

using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace ProjectsPage.Domain;

public static class CsvExportHelper
{
    public static byte[] ToCsvBytes<T>(IEnumerable<T> records, bool includeHeader = true)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true), leaveOpen: true);
        using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
          {
                  HasHeaderRecord = includeHeader,
                  ShouldQuote = args => true
          });

        csv.WriteRecords(records);
        writer.Flush();
        writer.Close();

        return memoryStream.ToArray();
    }
};
