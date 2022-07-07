using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;
using MvcMovie.Services;

namespace MvcMovie.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepository;

        public MovieController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        // GET: Movies
        public async Task<IActionResult> Index ([Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] string movieGenre, string searchString)
        {
            var movies = await _movieRepository.GetMovies();

            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title!.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(movieGenre))
            { 
                movies = movies.Where(x => x.Genre == movieGenre);
            }
            var movieGenreVM = new MovieGenreViewModel
            {
                Genres = new SelectList(movies.OrderBy(x => x.Genre).Select(s => s.Genre).Distinct().ToList()),
                Movies = movies.ToList()
            };

            return View(movieGenreVM);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var movieDetail = await _movieRepository.GetMovieById(id);

            return View(movieDetail);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie)
        {
            await _movieRepository.AddNewMovie(movie);

            return RedirectToAction(nameof(Index));
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var movie = await _movieRepository.GetMovieById(id);
            if (movie == null) return View("Not Found");

            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                return View(movie);
            }

            await _movieRepository.UpdateMovie(movie);
            return RedirectToAction(nameof(Index));
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _movieRepository.GetMovieById(id);
            if (movie == null) return View("Not Found");

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _movieRepository.GetMovieById(id);
            if (movie == null) return View("Not Found");

            await _movieRepository.DeleteMovie(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
