using LoginApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LoginApi.Models.Entities;

public partial class Category
{
    public long Id { get; set; }

    public string? Description { get; set; }

    public int? QtyPoints1 { get; set; }

    public int? QtyPoints2 { get; set; }

    public int? QtyMonths { get; set; }
}


public static class CategoryEndpoints
{
	public static void MapCategoryEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Category").WithTags(nameof(Category));

        group.MapGet("/", async (AppDbContext db) =>
        {
            return await db.Category.ToListAsync();
        })
        .WithName("GetAllCategories")
        .WithOpenApi()
        .RequireAuthorization();

        group.MapGet("/{id}", async Task<Results<Ok<Category>, NotFound>> (long id, AppDbContext db) =>
        {
            return await db.Category.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Category model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetCategoryById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (long id, Category category, AppDbContext db) =>
        {
            var affected = await db.Category
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, category.Id)
                  .SetProperty(m => m.Description, category.Description)
                  .SetProperty(m => m.QtyPoints1, category.QtyPoints1)
                  .SetProperty(m => m.QtyPoints2, category.QtyPoints2)
                  .SetProperty(m => m.QtyMonths, category.QtyMonths)
                  );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateCategory")
        .WithOpenApi();

        group.MapPost("/", async (Category category, AppDbContext db) =>
        {
            db.Category.Add(category);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Category/{category.Id}",category);
        })
        .WithName("CreateCategory")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (long id, AppDbContext db) =>
        {
            var affected = await db.Category
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteCategory")
        .WithOpenApi();
    }
}