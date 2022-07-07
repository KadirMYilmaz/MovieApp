using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

namespace MvcMovie.Services
{
    public class MovieRepository : IMovieRepository, IDisposable
    {
        private readonly MvcMovieContext _context;

        public MovieRepository(MvcMovieContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie>> GetMovies()
        {
            return await _context.Movies.ToListAsync();
        }

        public async Task<Movie> GetMovieById(int id)
        {
            var movie = await _context.Movies.Where(t => t.Id == id).FirstOrDefaultAsync();
            return movie;
        }

        public async Task AddNewMovie(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMovie(Movie movie)
        {
            if (movie == null)
            {
                throw new ArgumentNullException(nameof(movie));
            }

            _context.Update(movie);
            await _context.SaveChangesAsync();
        }

        public async Task<Movie> DeleteMovie(int id)
        {
            Movie movie = await _context.Movies.Where(t => t.Id == id).FirstOrDefaultAsync();

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return movie;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }
    }
}