using System.Net.Http.Json;
using System.Text.Json;
using BlazorWasm.Models;

namespace BlazorWasm.Services;

internal sealed class ProductService : IProductService
{
    private readonly HttpClient client;
    private readonly JsonSerializerOptions _options;

    public ProductService(HttpClient httpClient, JsonSerializerOptions options)
    {
        client = httpClient;
        _options = options;
    }
    public async Task<List<Product>?> Get()
    {
        var response = await client.GetAsync("v1/products");
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new ApplicationException();

        return JsonSerializer.Deserialize<List<Product>>(content, _options);
    }

    public async Task Add(Product product)
    {
        var response = await client.PostAsync("v1/products", JsonContent.Create(product));
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(content);
        }
    }

    public async Task Delete(int productId)
    {
        var response = await client.DeleteAsync($"v1/products/{productId}");
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(content);
        }
    }
}

public interface IProductService
{
    Task<List<Product>?> Get();

    Task Add(Product product);

    Task Delete(int productId);
}