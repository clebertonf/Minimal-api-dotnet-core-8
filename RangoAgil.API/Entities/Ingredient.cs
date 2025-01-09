using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RangoAgil.API.Entities;

public class Ingredient
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    public ICollection<Rango> Rangos { get; set; } = [];
    
    [SetsRequiredMembers]
    public Ingredient(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public Ingredient()
    {
        
    }
}