using BreweryManagementSystemWebAPI.Controllers;
using BreweryManagementSystemWebAPI.Data;
using BreweryManagementSystemWebAPI.DTOs;
using BreweryManagementSystemWebAPI.Models;
using BreweryManagementSystemWebAPI.Repositories;
using BreweryManagementSystemWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace BreweryManagementSystemWebAPI.Tests
{
    public class WholesalerStocksControllerTests
    {


        private readonly ApplicationDbContext _context;
        private readonly WholesalerStocksController _controller;
        private readonly IWholesalerStockRepository _wholesalerStockRepository;

        public WholesalerStocksControllerTests()
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
        public async Task AddSaleToWholesaler_ReturnsSuccess()
        {
            // Arrange
            var saleDto = new WholesalerStockDto
            {
                be_Id = 1,
                ws_Quantity = 10,
                wh_Id = 1
            };

            var expectedStock = new WholesalerStockDto
            {
                wh_Id = 1,
                wh_Name = "GeneDrinks",
                be_Id = 1, 
                be_Name = "Leffe Blonde",
                ws_Quantity = 30

            };

            
            // Act
            var result = await _controller.AddSaleToWholesalerAsync(saleDto);

            // Assert
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Cast okResult.Value to IEnumerable<WholesalerStockDto>
            var returnedSales = Assert.IsAssignableFrom<IEnumerable<WholesalerStockDto>>(okResult.Value);

            // Convert IEnumerable to List if needed
            var returnedSaleList = returnedSales.ToList();

            // Validate that data matches expected results
            Assert.NotEmpty(returnedSaleList);

            // Compare the returned item to the expected item
            var returnedSale = returnedSaleList.First();
            Assert.Equal(expectedStock.wh_Id, returnedSale.wh_Id);
            Assert.Equal(expectedStock.wh_Name, returnedSale.wh_Name);
            Assert.Equal(expectedStock.be_Id, returnedSale.be_Id);
            Assert.Equal(expectedStock.be_Name, returnedSale.be_Name);
            Assert.Equal(expectedStock.ws_Quantity, returnedSale.ws_Quantity);
        }

        [Fact]
        public async Task AddSaleToWholesaler_ReturnsFailed()
        {
            // Arrange
            var saleDto = new WholesalerStockDto
            {
                be_Id = 1,
                ws_Quantity = 10,
                wh_Id = 99999
            };



            // Act
            var result = await _controller.AddSaleToWholesalerAsync(saleDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error: Wholesaler not found.", badRequestResult.Value);

        }



        [Fact]
        public async Task UpdateSaleToWholesaler_ReturnsSuccess()
        {
            // Arrange
            var saleDto = new WholesalerStockDto
            {
                be_Id = 1,
                ws_Quantity = 20,
                wh_Id = 1
            };

            var expectedStock = new WholesalerStockDto
            {
                wh_Id = 1,
                wh_Name = "GeneDrinks",
                be_Id = 1,
                be_Name = "Leffe Blonde",
                ws_Quantity = 20

            };


            // Act
            var result = await _controller.UpdateWholesalerStockAsync(saleDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Cast okResult.Value to IEnumerable<WholesalerStockDto>
            var returnedSales = Assert.IsAssignableFrom<IEnumerable<WholesalerStockDto>>(okResult.Value);

            // Convert IEnumerable to List if needed
            var returnedSaleList = returnedSales.ToList();

            // Validate that data matches expected results
            Assert.NotEmpty(returnedSaleList);

            // Compare the returned item to the expected item
            var returnedSale = returnedSaleList.First();
            Assert.Equal(expectedStock.wh_Id, returnedSale.wh_Id);
            Assert.Equal(expectedStock.wh_Name, returnedSale.wh_Name);
            Assert.Equal(expectedStock.be_Id, returnedSale.be_Id);
            Assert.Equal(expectedStock.be_Name, returnedSale.be_Name);
            Assert.Equal(expectedStock.ws_Quantity, returnedSale.ws_Quantity);
        }


        [Fact]
        public async Task UpdateSaleToWholesaler_ReturnsFailed()
        {
            // Arrange
            var saleDto = new WholesalerStockDto
            {
                be_Id = 900,
                ws_Quantity = 20,
                wh_Id = 1
            };


            // Act
            var result = await _controller.UpdateWholesalerStockAsync(saleDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error: Beer not found.", badRequestResult.Value);

        }


    }
}
