using Application.Produtcs.Create;
using Application.Produtcs.Delete;
using Application.Produtcs.Get;
using Application.Produtcs.GetById;
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
        app.MapGet("products", async (
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            int page,
            int pageSize,
            ISender sender) =>
        {
            var query = new GetProductsQuery(searchTerm, sortColumn, sortOrder, page, pageSize);

            var products = await sender.Send(query);

            return Results.Ok(products);
        }).WithName("GetProducts");

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
        }).WithName("GetProduct");

        app.MapPost("products", async (
            CreateProductRequest request,
            [FromHeader(Name = "X-Idempotency-Key")] string requestId,
            ISender sender) =>
        {
            if (!Guid.TryParse(requestId, out Guid parsedRequestId))
            {
                return Results.BadRequest();
            }

            var command = new CreateProductCommand(
                parsedRequestId,
                request.Name,
                request.Sku,
                request.Currency,
                request.Amount);

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
        }).WithName("UpdateProduct");

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
        }).WithName("DeleteProduct");
    }
}
