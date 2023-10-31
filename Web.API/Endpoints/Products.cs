using Application.Produtcs.Create;
using Application.Produtcs.Delete;
using Application.Produtcs.Get;
using Application.Produtcs.Update;
using Carter;
using Domain.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Endpoints;

public class Products : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("products/{id:guid}", async (Guid id, ISender sender) =>
        {
            try
            {
                return Results.Ok(await sender.Send(new GetProductQuery(new ProductId(id))));
            }
            catch(Exception e)
            {
                if (e is ProductNotFoundException)
                {
                    return Results.NotFound(e.Message);
                }

                return Results.BadRequest(e.Message);
            }
        });

        app.MapPost("products", async (CreateProductCommand command, ISender sender) =>
        {
            await sender.Send(command);

            return Results.Ok();
        });

        app.MapPut("products/{id:guid}", async (Guid id, [FromBody] UpdateProductRequest request, ISender sender) =>
        {
            var command = new UpdateProductCommand(
                new ProductId(id),
                request.Name,
                request.Sku,
                request.Currency,
                request.Amount);

            await sender.Send(command);

            return Results.NoContent();
        });

        app.MapDelete("products/{id:guid}", async (Guid id, ISender sender) =>
        {
            try
            {
                await sender.Send(new DeleteProductCommand(new ProductId(id)));

                return Results.NoContent();
            }
            catch (Exception e)
            {
                if (e is ProductNotFoundException)
                {
                    return Results.NotFound(e.Message);
                }

                return Results.BadRequest(e.Message);
            }
        });

    }
}
