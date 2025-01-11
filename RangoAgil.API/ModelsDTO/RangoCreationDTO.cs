using System.ComponentModel.DataAnnotations;

namespace RangoAgil.API.ModelsDTO;

public class RangoCreationDTO
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string? Name { get; set; }
}