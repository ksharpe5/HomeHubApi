using AutoMapper;
using HomeHubApi.DTOs;
using HomeHubApi.Models;

namespace HomeHubApi.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Recipe ↔ RecipeDto
        CreateMap<Recipe, RecipeDto>()
            .ForMember(dest => dest.Ingredients,
                opt => opt.MapFrom(src => src.RecipeIngredients))
            .ReverseMap()
            .ForMember(dest => dest.RecipeIngredients, opt => opt.Ignore());

        // RecipeIngredient ↔ RecipeIngredientDto
        CreateMap<RecipeIngredient, RecipeIngredientDto>()
            .ForMember(dest => dest.Product,
                opt => opt.MapFrom(src => src.Product))
            .ReverseMap()
            .ForMember(dest => dest.Product, opt => opt.Ignore()); // Product navigation handled by FK

        // Product ↔ ProductDto
        CreateMap<Product, ProductDto>().ReverseMap();

        // Instruction ↔ InstructionDto
        CreateMap<Instruction, InstructionDto>().ReverseMap();
    }
}