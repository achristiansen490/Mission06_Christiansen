using Microsoft.VisualBasic.FileIO;
using Mission06_Christiansen.Models;

namespace Mission06_Christiansen.Data;

public static class DbSeeder
{
    public static void Seed(MovieCollectionContext db, string contentRootPath)
    {
        if (db.Movies.Any()) return; // already seeded

        var csvPath = Path.Combine(contentRootPath, "Data", "Joel Hilton Movie Collection - Sheet1.csv");
        if (!File.Exists(csvPath))
        {
            // If you want, you can throw here to make the issue obvious:
            // throw new FileNotFoundException("CSV not found", csvPath);
            return;
        }

        using var parser = new TextFieldParser(csvPath);
        parser.SetDelimiters(",");
        parser.HasFieldsEnclosedInQuotes = true;

        var headers = parser.ReadFields();
        if (headers == null) return;

        var map = headers
            .Select((h, i) => new { h = h.Trim(), i })
            .ToDictionary(x => x.h, x => x.i);

        string Get(string[] row, string header)
            => map.ContainsKey(header) && map[header] < row.Length ? row[map[header]].Trim() : "";

        int ParseYear(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return 0;
            var first = raw.Split('-')[0].Trim();   // handles "2001-2002"
            return int.TryParse(first, out var y) ? y : 0;
        }

        bool? ParseEdited(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return null;
            raw = raw.Trim().ToLower();
            return raw switch
            {
                "yes" or "true" => true,
                "no" or "false" => false,
                _ => null
            };
        }

        while (!parser.EndOfData)
        {
            var row = parser.ReadFields();
            if (row == null) continue;

            var movie = new Movie
            {
                Category = Get(row, "Category"),
                Title = Get(row, "Title"),
                Year = ParseYear(Get(row, "Year")),
                Director = Get(row, "Director"),
                Rating = Get(row, "Rating"),
                Edited = ParseEdited(Get(row, "Edited")),
                LentTo = string.IsNullOrWhiteSpace(Get(row, "Lent To:")) ? null : Get(row, "Lent To:"),
                Notes = string.IsNullOrWhiteSpace(Get(row, "Notes")) ? null : Get(row, "Notes")
            };

            if (!string.IsNullOrWhiteSpace(movie.Title))
                db.Movies.Add(movie);
        }

        db.SaveChanges();
    }
}
