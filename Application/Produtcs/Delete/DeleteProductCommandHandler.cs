﻿using Application.Data;
using Domain.Products;
using MediatR;

namespace Application.Produtcs.Delete;

internal sealed class DeleteProductCommandHandler
    : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id);

        if (product is null)
        {
            throw new ProductNotFoundException(request.Id);
        }

        _productRepository.Delete(product);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}