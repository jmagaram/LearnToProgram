# LearnToProgram

A series of exercises and tools that help Israeli junior high and high school students learn to program in C#. It serves both beginners taking their first steps and more advanced students.

The site is branded for the Marom school (מרום), so the brand in the top-left of the top bar is in Hebrew. Clicking it goes to the home page.

## Audience

- Students are teenagers, and many are native Hebrew speakers who read English as a second language. Keep instructions short, plain and friendly.
- Hebrew text is welcome alongside English. Mark Hebrew elements with `lang="he" dir="rtl"`. Code and C# identifiers stay in English.
- Exercises should range from beginner (conditions, loops) to more advanced (data structures such as linked lists).

## Project

- `LearnToProgram/` is a Blazor WebAssembly app, deployed to Azure Static Web Apps (`.github/workflows/`).
- `Pages/` holds one page per exercise set. The site is hub-and-spoke: the home page shows a tile per area (such as Conditions), and inside an area the top bar switches between its pages. Register every page in `Layout/Areas.cs`.
- `Practice/` holds the problem definitions and the in-browser C# runner that compiles and tests student code.
- Build with `dotnet build LearnToProgram/LearnToProgram.csproj`.
