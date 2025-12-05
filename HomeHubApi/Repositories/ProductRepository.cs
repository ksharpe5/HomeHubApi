using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeHubApi.Data;
using HomeHubApi.DTOs;
using HomeHubApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeHubApi.Repositories;

public class ProductRepository(HomeHubContext context, IMapper mapper) : IProductRepository
{
    public async Task<List<ProductDto>> GetAll()
    {
        return await context.Products
            .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<ProductDto?> GetByBarcode(string barcode)
    {
        var entity = await context.Products.FirstOrDefaultAsync(e => e.Barcode == barcode);
        return entity == null ? null : mapper.Map<ProductDto>(entity);
    }

    public async Task<ProductDto> Add(ProductDto product)
    {
        var entity = mapper.Map<Product>(product);
        await context.Products.AddAsync(entity);
        await context.SaveChangesAsync();
        return mapper.Map<ProductDto>(entity);
    }

    public async Task<ProductDto?> Update(ProductDto product)
    {
        var entity = await context.Products.FirstOrDefaultAsync(e => e.Id == product.Id);
        if (entity == null) return null;
        
        mapper.Map(product, entity);
        await context.SaveChangesAsync();
        return mapper.Map<ProductDto>(entity);
    }

    public async Task<bool> Remove(int id)
    {
        var entity = await context.Products.FirstOrDefaultAsync(e => e.Id == id);
        if (entity == null) return false;
        context.Products.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}

public interface IProductRepository
{
    Task<List<ProductDto>> GetAll();
    Task<ProductDto?> GetByBarcode(string barcode);
    Task<ProductDto> Add(ProductDto product);
    Task<ProductDto?> Update(ProductDto product);
    Task<bool> Remove(int id);
}