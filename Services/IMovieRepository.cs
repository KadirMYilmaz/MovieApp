using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvcMovie.Models;

namespace MvcMovie.Services
{
    public interface IMovieRepository : IDisposable
    {
        Task<IEnumerable<Movie>> GetMovies();
        Task<Movie> GetMovieById(int id);
        Task AddNewMovie(Movie movie);
        Task UpdateMovie(Movie movie);
        Task<Movie> DeleteMovie(int id);
    }
}