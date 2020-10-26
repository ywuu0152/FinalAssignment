namespace FinalAssignment.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Lecture")]
    public partial class Lecture
    {
        public int lectureId { get; set; }

        [Required]
        public string lectureName { get; set; }

        [Required]
        public string description { get; set; }

        [Column(TypeName = "date")]
        public DateTime date { get; set; }

        [Required]
        public string grade { get; set; }

        public int gradeNumber { get; set; }
    }
}
