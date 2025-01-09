using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RangoAgil.API.Entities;

public class Rango
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    public ICollection<Ingredient> Ingredients { get; set; } = [];
    
    [SetsRequiredMembers]
    public Rango(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public Rango()
    {
        
    }
}