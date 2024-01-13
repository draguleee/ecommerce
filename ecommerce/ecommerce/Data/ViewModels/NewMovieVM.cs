using ecommerce.Data.Base;
using ecommerce.Data.Enums;
using ecommerce.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ecommerce.Models
{
    public class NewMovieVM
    {
        public int Id { get; set; }

        [Display(Name = "Movie Poster URL")]
        [Required(ErrorMessage = "Movie poster URL is required!")]
        public string ImageUrl { get; set; }

        [Display(Name = "Movie Name")]
        [Required(ErrorMessage = "Movie name is required!")]
        public string Name { get; set; }

        [Display(Name = "Movie Description")]
        [Required(ErrorMessage = "Movie description is required!")]
        public string Description { get; set; }

        [Display(Name = "Price in $")]
        [Required(ErrorMessage = "Movie price is required!")]
        public double Price { get; set; }

        [Display(Name = "Movie Start Date")]
        [Required(ErrorMessage = "Movie start date is required!")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Movie End Date")]
        [Required(ErrorMessage = "Movie end date is required!")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Select a Category...")]
        [Required(ErrorMessage = "Movie category is required!")]
        public MovieCategory MovieCategory { get; set; }

        // Relationships
        [Display(Name = "Select Actor(s)...")]
        [Required(ErrorMessage = "Movie actor(s) is required!")]
        public List<int> ActorIds { get; set; }

        [Display(Name = "Select a Cinema...")]
        [Required(ErrorMessage = "Movie cinema is required!")]
        public int CinemaId { get; set; }

        [Display(Name = "Select a Producer...")]
        [Required(ErrorMessage = "Movie producer is required!")]
        public int ProducerId { get; set; }
    }
}
