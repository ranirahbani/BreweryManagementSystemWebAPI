using BreweryManagementSystemWebAPI.Controllers;
using BreweryManagementSystemWebAPI.Data;
using BreweryManagementSystemWebAPI.DTOs;
using BreweryManagementSystemWebAPI.Repositories;
using BreweryManagementSystemWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace BreweryManagementSystemWebAPI.Tests
{
    public class QuoteControllerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly WholesalerStocksController _controller;
        private readonly IWholesalerStockRepository _wholesalerStockRepository;

        public QuoteControllerTests()
        {
            // Initialize the real database context
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer("Server=DESKTOP-G6ES3BM\\MSSQLSERVER01;Database=belgium;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;

            _context = new ApplicationDbContext(options);
            _wholesalerStockRepository = new WholesalerStockRepository(_context);
            var wholesalerStockService = new WholesalerStockService(_wholesalerStockRepository);

            _controller = new WholesalerStocksController(wholesalerStockService);
        }


        [Fact]
        public async Task GenerateQuote_ReturnsExpectedQuotes()
        {
            // Arrange
            var quoteRequest = new QuoteRequestDto
            {
                WholesalerId = 3,
                WholesalerName = "BeerWorld",
                BeerOrderList = new List<BeerOrderDto>
        {
            new BeerOrderDto { BeerId = 1, Quantity = 5 },
            new BeerOrderDto { BeerId = 3, Quantity = 15 }
        }
            };



            var expectedQuoteDto = new QuoteDto
            {
                TotalQuantity = 20,
                DiscountPercentage = 10,
                TotalAmount = (Decimal)70.65
            };

            // Act
            var result = await _controller.GenerateQuoteAsync(quoteRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            // Cast okResult.Value to IEnumerable<WholesalerStockDto>
            var returnedSales = Assert.IsAssignableFrom<IEnumerable<QuoteDto>>(okResult.Value);

            // Convert IEnumerable to List if needed
            var returnedSaleList = returnedSales.ToList();

            // Validate that data matches expected results
            Assert.NotEmpty(returnedSaleList);

            // Compare the returned item to the expected item
            var returnedQuote = returnedSaleList.First();
            Assert.Equal(expectedQuoteDto.TotalQuantity, returnedQuote.TotalQuantity);
            Assert.Equal(expectedQuoteDto.DiscountPercentage, returnedQuote.DiscountPercentage);
            Assert.Equal(expectedQuoteDto.TotalAmount, returnedQuote.TotalAmount);
        }



        [Fact]
        public async Task GenerateQuote_Success_LessThan10Drinks()
        {
            // Arrange
            var quoteRequest = new QuoteRequestDto
            {
                WholesalerId = 3,
                WholesalerName = "BeerWorld",
                BeerOrderList = new List<BeerOrderDto>
        {
            new BeerOrderDto { BeerId = 1, Quantity = 5 }
        }
            };



            var expectedQuoteDto = new QuoteDto
            {
                TotalQuantity = 5,
                DiscountPercentage = 0,
                TotalAmount = 11
            };

            // Act
            var result = await _controller.GenerateQuoteAsync(quoteRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            // Cast okResult.Value to IEnumerable<WholesalerStockDto>
            var returnedSales = Assert.IsAssignableFrom<IEnumerable<QuoteDto>>(okResult.Value);

            // Convert IEnumerable to List if needed
            var returnedSaleList = returnedSales.ToList();

            // Validate that data matches expected results
            Assert.NotEmpty(returnedSaleList);

            // Compare the returned item to the expected item
            var returnedQuote = returnedSaleList.First();
            Assert.Equal(expectedQuoteDto.TotalQuantity, returnedQuote.TotalQuantity);
            Assert.Equal(expectedQuoteDto.DiscountPercentage, returnedQuote.DiscountPercentage);
            Assert.Equal(expectedQuoteDto.TotalAmount, returnedQuote.TotalAmount);
        }


        [Fact]
        public async Task GenerateQuote_Success_10To20Drinks_WithDiscount()
        {
            // Arrange
            var quoteRequest = new QuoteRequestDto
            {
                WholesalerId = 3,
                WholesalerName = "BeerWorld",
                BeerOrderList = new List<BeerOrderDto>
        {
            new BeerOrderDto { BeerId = 3, Quantity = 15 }
        }
            };



            var expectedQuoteDto = new QuoteDto
            {
                TotalQuantity = 15,
                DiscountPercentage = 10,
                TotalAmount = (decimal)60.75
            };

            var expectedQuotes = new List<QuoteDto> { expectedQuoteDto };

            // Act
            var result = await _controller.GenerateQuoteAsync(quoteRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            // Cast okResult.Value to IEnumerable<WholesalerStockDto>
            var returnedSales = Assert.IsAssignableFrom<IEnumerable<QuoteDto>>(okResult.Value);

            // Convert IEnumerable to List if needed
            var returnedSaleList = returnedSales.ToList();

            // Validate that data matches expected results
            Assert.NotEmpty(returnedSaleList);

            // Compare the returned item to the expected item
            var returnedQuote = returnedSaleList.First();
            Assert.Equal(expectedQuoteDto.TotalQuantity, returnedQuote.TotalQuantity);
            Assert.Equal(expectedQuoteDto.DiscountPercentage, returnedQuote.DiscountPercentage);
            Assert.Equal(expectedQuoteDto.TotalAmount, returnedQuote.TotalAmount);
        }


        [Fact]
        public async Task GenerateQuote_Success_MoreThan20Drinks_WithDiscount()
        {
            // Arrange
            var quoteRequest = new QuoteRequestDto
            {
                WholesalerId = 3,
                WholesalerName = "BeerWorld",
                BeerOrderList = new List<BeerOrderDto>
        {
            new BeerOrderDto { BeerId = 1, Quantity = 5 },
            new BeerOrderDto { BeerId = 3, Quantity = 15 }
        }
            };



            var expectedQuoteDto = new QuoteDto
            {
                TotalQuantity = 20,
                DiscountPercentage = 10,
                TotalAmount = (Decimal)70.65
            };

            // Act
            var result = await _controller.GenerateQuoteAsync(quoteRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            // Cast okResult.Value to IEnumerable<WholesalerStockDto>
            var returnedSales = Assert.IsAssignableFrom<IEnumerable<QuoteDto>>(okResult.Value);

            // Convert IEnumerable to List if needed
            var returnedSaleList = returnedSales.ToList();

            // Validate that data matches expected results
            Assert.NotEmpty(returnedSaleList);

            // Compare the returned item to the expected item
            var returnedQuote = returnedSaleList.First();
            Assert.Equal(expectedQuoteDto.TotalQuantity, returnedQuote.TotalQuantity);
            Assert.Equal(expectedQuoteDto.DiscountPercentage, returnedQuote.DiscountPercentage);
            Assert.Equal(expectedQuoteDto.TotalAmount, returnedQuote.TotalAmount);
        }


        [Fact]
        public async Task GenerateQuote_Fails_EmptyOrderList()
        {
            // Arrange
            var quoteRequest = new QuoteRequestDto
            {
                WholesalerId = 3,
                WholesalerName = "BeerWorld",
                BeerOrderList = new List<BeerOrderDto>()
            };

            // Act
            var result = await _controller.GenerateQuoteAsync(quoteRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Error: The order cannot be empty.\r\n", badRequestResult.Value);
        }


        [Fact]
        public async Task GenerateQuote_Fails_NonExistingWholesaler()
        {
            // Arrange
            var quoteRequest = new QuoteRequestDto
            {
                WholesalerId = null,
                WholesalerName = "TESTTT",
                BeerOrderList = new List<BeerOrderDto>
        {
            new BeerOrderDto { BeerId = 1, Quantity = 5 },
            new BeerOrderDto { BeerId = 3, Quantity = 15 }
        }
            };

            // Act
            var result = await _controller.GenerateQuoteAsync(quoteRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Error: The wholesaler must exist.\r\n", badRequestResult.Value);
        }


        [Fact]
        public async Task GenerateQuote_Fails_DuplicateBeers()
        {
            // Arrange
            var quoteRequest = new QuoteRequestDto
            {
                WholesalerId = 3,
                WholesalerName = "BeerWorld",
                BeerOrderList = new List<BeerOrderDto>
        {
            new BeerOrderDto { BeerId = 1, Quantity = 5 },
            new BeerOrderDto { BeerId = 1, Quantity = 5 },
            new BeerOrderDto { BeerId = 3, Quantity = 15 }
        }
            };

            // Act
            var result = await _controller.GenerateQuoteAsync(quoteRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Error: There can't be any duplicate in the order.\r\n", badRequestResult.Value);
        }


        [Fact]
        public async Task GenerateQuote_Fails_BeerNotSoldByWholesaler()
        {
            // Arrange
            var quoteRequest = new QuoteRequestDto
            {
                WholesalerId = null,
                WholesalerName = "GeneDrinks",
                BeerOrderList = new List<BeerOrderDto>
        {
            new BeerOrderDto { BeerId = 3, Quantity = 15 }
        }
            };

            // Act
            var result = await _controller.GenerateQuoteAsync(quoteRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Error: The beer must be sold by the wholesaler.\r\n", badRequestResult.Value);
        }

    }

}
