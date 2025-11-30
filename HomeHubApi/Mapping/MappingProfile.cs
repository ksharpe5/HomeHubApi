using AutoMapper;
using HomeHubApi.DTOs;
using HomeHubApi.Models;

namespace HomeHubApi.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Recipe, RecipeDto>().ReverseMap();
        CreateMap<Ingredient, IngredientDto>().ReverseMap();
        CreateMap<Instruction, InstructionDto>().ReverseMap();
        
        CreateMap<TapoDevice, TapoDeviceDto>().ReverseMap();
    }
}