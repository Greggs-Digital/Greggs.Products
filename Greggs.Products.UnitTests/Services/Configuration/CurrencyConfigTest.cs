using System;
using System.IO;
using System.Linq;
using System.Text;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services.Configuration;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Greggs.Products.UnitTests.Services.Configuration
{
    public class CurrencyConfigTest
    {

        public CurrencyConfigTest()
        {

        }

        [Fact]
        public void GetCurrencyConversionConfigTest()
        {
            //arrange

            var config = "{\"CurrencyConversions\":{\"BaseCurrency\":\"GBP\",\"CurrencyConversionList\":[{\"Currency\":\"EUR\",\"ConversionRate\":1.1},{\"Currency\":\"YASER\",\"ConversionRate\":1.1}]}}";

            var configuration = new ConfigurationBuilder()
            .AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(config)))
            .Build();
            var service = new CurrencyConversionService(configuration);


            //act
            var result = service.GetCurrencyConversionConfig();

            //assert
            Assert.Equal("GBP", result.BaseCurrency);
            Assert.Single(result.CurrencyConversionList);

        }



    }
}

