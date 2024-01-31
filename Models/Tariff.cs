using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab4.Models
{
    public class Tariff
    {
        public int Id { get; set; }
        [Required] public string? Name { get; set; }
        [Required] public string? DiagnosisCode { get; set; }
        [Required] public string? DiagnosisName { get; set; }
        [Required] public double Cost { get; set; }
        List<Patient> Patients { get; set; } = new();
    }
}
