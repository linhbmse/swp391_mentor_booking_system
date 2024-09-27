using CsvHelper;
using CsvHelper.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace SwpMentorBooking.Infrastructure.Utils
{
    public class CSVFileService
    {
        // First we need to only inject one service
        // ---
        public CSVFileService()
        {

        }

        public List<T> ReadCSVFile<T>(string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
            };
            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, config))
                {   // Convert CSV records to a list of T
                    List<T> records = csv.GetRecords<T>().ToList();
                    return records;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the CSV file: {ex.Message}");
                return new List<T>();
            }
        }

        public List<(T Record, List<string> Errors)> ValidateCSVData<T>(List<T> records)
        {
            List<(T Record, List<string> Errors)> validationResults = new();
            // 
            foreach (var record in records)
            {
                var context = new ValidationContext(record, serviceProvider: null, items: null);
                var errors = new List<ValidationResult>();
                if (!Validator.TryValidateObject(record, context, errors, validateAllProperties: true))
                {
                    var errorMessages = errors.Select(e => e.ErrorMessage).ToList();
                    validationResults.Add((record, errorMessages));
                }
                else
                {
                    validationResults.Add((record, new List<string>()));
                }
            }
            return validationResults;
        }

        //public List<T> GetValidRecords<T>(List<(T Record, List<ValidationResult> Errors)> validationResults)
        //{
        //    var validRecords = validationResults.Where(vr => !vr.Errors.Any())
        //                                        .Select(vr => vr.Record)
        //                                        .ToList();

        //    return validRecords;
        //}
    }
}
