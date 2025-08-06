// Models/Movie.cs
namespace StreamFlixAPI.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Genre { get; set; }

        public string ThumbnailUrl { get; set; }

        public string ImdbId { get; set; } // ✅ Add this field
    }
}
