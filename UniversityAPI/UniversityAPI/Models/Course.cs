using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UniversityAPI.Models
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ExternalCourseId { get; set; }

        [Required]
        [StringLength(10)]
        public string Code { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Range(1, 10)]
        public int Credits { get; set; }
        public string Department { get; set; }

        [Required]
        [StringLength(100)]
        public string Instructor { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Range(1, 1000)]
        public int MaxCapacity { get; set; }

        [Range(0, 1000)]
        public int CurrentEnrollment { get; set; }

        public bool IsActive { get; set; }
        public ICollection<Student> EnrolledStudents { get; set; }
    }
}
