using ecommerce.Data.Base;
using ecommerce.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ecommerce.Models
{
    public class Movie : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Display(Name = "Movie Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Price")]
        public double Price { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Category")]
        public MovieCategory MovieCategory { get; set; }
        
        // Relationships
        public List<Actor_Movie> Actors_Movies { get; set; }

        public int CinemaId { get; set; }
        [ForeignKey("CinemaId")]
        public Cinema Cinema { get; set; }

        public int ProducerId { get; set; }
        [ForeignKey("ProducerId")]
        public Producer Producer { get; set; }
    }
}
