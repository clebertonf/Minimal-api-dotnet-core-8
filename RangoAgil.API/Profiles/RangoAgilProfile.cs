﻿using AutoMapper;
using RangoAgil.API.Entities;
using RangoAgil.API.ModelsDTO;

namespace RangoAgil.API.Profiles;

public class RangoAgilProfile : Profile
{
    public RangoAgilProfile()
    {
        CreateMap<Ingredient, IngredientDTO>().ReverseMap();
        CreateMap<Ingredient, IngredientCreationDTO>().ReverseMap();
        CreateMap<Ingredient, IngredientUpdate>().ReverseMap();
        CreateMap<Rango, RangoDTO>().ReverseMap();
        CreateMap<Rango, RangoCreationDTO>().ReverseMap();
        CreateMap<Rango, RangoUpdateDTO>().ReverseMap();
    }
}