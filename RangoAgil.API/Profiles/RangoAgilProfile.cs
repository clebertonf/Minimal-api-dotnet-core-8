using AutoMapper;
using RangoAgil.API.Entities;
using RangoAgil.API.ModelsDTO;

namespace RangoAgil.API.Profiles;

public class RangoAgilProfile : Profile
{
    public RangoAgilProfile()
    {
        CreateMap<Ingredient, IngredientDTO>().ReverseMap();
        CreateMap<Rango, RangoDTO>().ReverseMap();
    }
}