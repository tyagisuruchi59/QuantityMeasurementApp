using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuantityMeasurementAppModel.Entities
{
    [Table("QuantityMeasurements")]
    public class QuantityMeasurementEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Column("this_value")]
        public double ThisValue { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("this_unit")]
        public string ThisUnit { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("this_measurement_type")]
        public string ThisMeasurementType { get; set; } = string.Empty;

        [Required]
        [Column("that_value")]
        public double ThatValue { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("that_unit")]
        public string ThatUnit { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("that_measurement_type")]
        public string ThatMeasurementType { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("operation")]
        public string Operation { get; set; } = string.Empty;

        [Column("result_string")]
        public string? ResultString { get; set; }

        [Column("result_value")]
        public double ResultValue { get; set; }

        [MaxLength(50)]
        [Column("result_unit")]
        public string? ResultUnit { get; set; }

        [MaxLength(100)]
        [Column("result_measurement_type")]
        public string? ResultMeasurementType { get; set; }

        [Column("error_message")]
        public string? ErrorMessage { get; set; }

        [Column("is_error")]
        public bool IsError { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}