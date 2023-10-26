﻿using Domain.Products;

namespace Domain.UnitTests.Products;

public class SkuTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Create_Should_ReturnNull_WhenValueIsNullOrEmpty(string? value)
    {
        // Act
        var sku = Sku.Create(value);

        // Assert
        Assert.Null(sku);
    }

    public static IEnumerable<object[]> InvalidSkuLengthDataMethod() => new List<object[]>
    {
        new object[] { "invalid_sku" },
        new object[] { "invalid_sku_1" },
        new object[] { "invalid_sku_2" }
    };

    [Theory]
    [MemberData(nameof(InvalidSkuLengthDataMethod))]
    public static void Create_Should_ReturnNull_WhenValueLengthIsInvalid(string value)
    {
        // Act
        var sku = Sku.Create(value);

        // Assert
        Assert.Null(sku);
    }
}
