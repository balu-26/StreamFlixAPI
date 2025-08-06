    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using StreamFlixAPI.Data;
    using StreamFlixAPI.Models;

    namespace StreamFlixAPI.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class MoviesController : ControllerBase
        {
            private readonly AppDbContext _context;

            public MoviesController(AppDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<IActionResult> GetMovies()
            {
                var movies = await _context.Movies.ToListAsync();
                return Ok(movies);
            }

            [HttpPost]
            public async Task<IActionResult> AddMovie([FromBody] Movie movie)
            {
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();
                return Ok(movie);
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteMovie(int id)
            {
                var movie = await _context.Movies.FindAsync(id);
                if (movie == null) return NotFound();
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
