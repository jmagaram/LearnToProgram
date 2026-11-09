namespace LearnToProgram.Layout;

/// <summary>One page of exercises inside an area, such as "Conditions 1".</summary>
public sealed record Exercise(int Number, string Href, string Title, string HebrewTitle);

/// <summary>A major topic shown as a tile on the home page, such as "Conditions".</summary>
public sealed record Area(string Title, string HebrewTitle, string Blurb, Exercise[] Exercises);

/// <summary>
/// The site map. The home page shows one tile per area, and the top bar shows the exercises of
/// the area you are in. Add new pages here.
/// </summary>
public static class Areas
{
    public static readonly Area[] All =
    {
        new("Conditions", "תנאים",
            "Comparisons, %, && and ||, and digits.",
            new Exercise[]
            {
                new(1, "conditions-1", "Read the code", "קריאת קוד"),
                new(2, "conditions-2", "Write the code", "כתיבת קוד"),
            }),
        new("Arrays", "מערכים",
            "Indexes, Length, loops over arrays, and 2D grids.",
            new Exercise[]
            {
                new(1, "arrays-1", "Read the code", "קריאת קוד"),
                new(2, "arrays-2", "Write the code", "כתיבת קוד"),
            }),
        new("Linked List", "רשימה מקושרת",
            "Walk, build and change chains of IntNode.",
            new Exercise[]
            {
                new(1, "linked-list-1", "Read the code", "קריאת קוד"),
                new(2, "linked-list-2", "Write the code", "כתיבת קוד"),
            }),
        new("Projects", "פרויקטים",
            "Bigger programs you can watch run, like the Game of Life.",
            new Exercise[]
            {
                new(1, "life-1", "Life in a row", "חיים בשורה"),
                new(2, "life-2", "Game of Life", "משחק החיים"),
            }),
    };

    /// <summary>The area that owns a relative path like "conditions-2", or null (for the home page).</summary>
    public static Area? Find(string relativePath)
    {
        // only the first segment counts, so "conditions-1/7" (question 7) belongs to Conditions
        var path = relativePath.Split('?', '#')[0].Trim('/').Split('/')[0].ToLowerInvariant();
        if (path.Length == 0) return null;
        return All.FirstOrDefault(a => a.Exercises.Any(e => e.Href == path))
            // older links such as /conditions or /linked-list still work
            ?? All.FirstOrDefault(a => a.Exercises.Any(e => e.Href.StartsWith(path + "-")));
    }
}
