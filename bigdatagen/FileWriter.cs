using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

public static class FileWriter
{

    public static void TryWriteOrAppendToFile<T>(IEnumerable<T> data)
    {
        if(FileExists())
        {
            AppendToFile(data);
        }
        else
        {
            WriteToFile(data);
        }
    }

    static void WriteToFile<T>(IEnumerable<T> data)
    {
        // Write to file
        try{
            // Append to the file.
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Don't write the header again.
                HasHeaderRecord = true,
                Delimiter = "|"
            };
            using (var writer = new StreamWriter("./files/transactions.csv"))
            using (var csv = new CsvWriter(writer, config))
            {
                var options = new TypeConverterOptions { Formats = new[] { "MM/dd/yyyy" } };
                csv.Context.TypeConverterOptionsCache.AddOptions<DateTime>(options);          
                csv.WriteRecords(data);
            }
        }
        catch(Exception e)
        {
            // Log error
        }
    }

    static void AppendToFile<T>(IEnumerable<T> data)
    {
        try{
            // Append to the file.
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Don't write the header again.
                HasHeaderRecord = false,
                Delimiter = "|"
            };
            // Append to file
            using (var stream = File.Open("./files/transactions.csv", FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                var options = new TypeConverterOptions { Formats = new[] { "MM/dd/yyyy" } };
                csv.Context.TypeConverterOptionsCache.AddOptions<DateTime>(options);       
                csv.WriteRecords(data);
            }
        }
        catch(Exception e)
        {
            // Log error
        }
    }

    static bool FileExists()
    {
        if(File.Exists("./files/transactions.csv"))
        {
            return true;
        }
        return false;
    }
}