using MovieManager.Models;

var filePath = "movies.json";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(_ => new MovieRental(filePath));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/movies", (MovieRental library) =>
{
    var movies = library.GetMovies();
    return Results.Ok(movies);
});

app.MapGet("/movies/{id}", (Guid id, MovieRental library) =>
{
    var movie = library.GetMovies().FirstOrDefault(m => m.Id == id);
    if (movie == null)
        return Results.NotFound();
    return Results.Ok(movie);
});

app.MapPost("/movies", (Movie movie, MovieRental library) =>
{
    library.AddMovie(movie);
    return Results.Created($"/movies/{movie.Id}", movie);
});

app.MapDelete("/movies/{id}", (Guid id, MovieRental library) =>
{
    library.RemoveMovie(id);
    return Results.NoContent();
});

app.Run();
