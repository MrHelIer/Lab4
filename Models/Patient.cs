using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4.Models
{
    public class Patient
    {
        public int Id { get; set; }
        [Required] public string? FullName { get; set; }
        [Required] public int Age { get; set; }
        [Required] public string? Gender { get; set; }
        [Required] public string? PlaceOfResidence { get; set; }
        [Required] public string? DiagnosisCode { get; set; }
        [Required] public string? DiagnosisName { get; set; }
        [Required] public int Days { get; set; }
        [Required] public double Cost { get; set; }
        [Required] public int? TariffId { get; set; }
        public Tariff? Tariff { get; set; }
    }
}
