using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Respositories;
using System;
using System.Linq;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using Moq;

namespace Greggs.Products.UnitTests.Respositories
{
    public class ProductRepository_Tests
    {
        [Theory]
        [InlineData(1,2)]
        [InlineData(2,3)]
        public void Call_List_ValidParameters_ReturnsExpectedListOfProductDTO(int pageStart, int pageLength)
        {
            //arrange
            var mockDataAccess = new Mock<IDataAccess<Product>>();
            mockDataAccess.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>()))
                          .Returns<int, int>((pStart, pEnd) => CreateDatabaseResult(pStart, pEnd));

            var respositoryToTest = new ProductRepository(mockDataAccess.Object);

            //act
            var result = respositoryToTest.List(pageStart, pageLength);

            //assert
            result.Should().BeEquivalentTo(CreateExpectedOutput(pageStart, pageLength));
        }

        private IEnumerable<Product> CreateDatabaseResult(int startIndex, int numberOfItems)
        {
            return Enumerable.Range(startIndex, numberOfItems).Select(p => new Product{  Name = p.ToString(), PriceInPounds = 1.00m});
        }

        private IEnumerable<ProductDTO> CreateExpectedOutput(int startIndex, int numberOfItems)
        {
            return Enumerable.Range(startIndex, numberOfItems).Select(p => new ProductDTO { Name = p.ToString(), Price = 1.00m });
        }
    } 
}