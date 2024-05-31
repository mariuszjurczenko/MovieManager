using System.Text.Json;

namespace MovieManager.Models;

public class MovieRental
{
    private List<Movie> movies;
    private readonly string libraryFilePath;

    public MovieRental(string libraryFilePath)
    {
        this.libraryFilePath = libraryFilePath;
        LoadData();
    }

    public void AddMovie(Movie movie)
    {
        if (movie is ActionMovie actionMovie)
            movies.Add(actionMovie);
        else
            movies.Add(movie);
            
        SaveData();    
    }

    public void RemoveMovie(Guid movieId)
    {
        Movie movie = movies.FirstOrDefault(m => m.Id == movieId);
        if (movie != null)
        {
            movies.Remove(movie);
            SaveData();
        }
    }

    public IEnumerable<Movie> GetMovies()
    {
        return movies;
    }

    public IEnumerable<ActionMovie> GetActionMovies()
   {
       return movies.OfType<ActionMovie>();
   }


    private void SaveData()
    {
        string jsonData = JsonSerializer.Serialize(movies);
        File.WriteAllText(libraryFilePath, jsonData);
    }

    private void LoadData()
    {
        if (File.Exists(libraryFilePath))
        {
            string jsonData = File.ReadAllText(libraryFilePath);
            movies = JsonSerializer.Deserialize<List<Movie>>(jsonData);
        }
        else
            movies = new List<Movie>();
    }
}
