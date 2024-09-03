using BreweryManagementSystemWebAPI.DTOs;
using BreweryManagementSystemWebAPI.Models;
using BreweryManagementSystemWebAPI.Repositories;
using BreweryManagementSystemWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using BreweryManagementSystemWebAPI.Data;
using Newtonsoft.Json;

namespace BreweryManagementSystemWebAPI.Tests
{
    public class BeerControllerTests
    {

        private readonly ApplicationDbContext _context;
        private readonly BeerController _controller;
        private readonly IBeerRepository _beerRepository;

        public BeerControllerTests()
        {
            // Initialize the real database context
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer("Server=DESKTOP-G6ES3BM\\MSSQLSERVER01;Database=belgium;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;

            _context = new ApplicationDbContext(options);
            _beerRepository = new BeerRepository(_context);
            var beerService = new BeerService(_beerRepository);

            _controller = new BeerController(beerService);
        }

        [Fact]
        public async Task ListBeersByBrewery_ReturnsCorrectBeers()
        {
            // Arrange
            var breweryDto = new BreweryDto
            {
                br_Id = 1,
                br_Name = null
            };

            var expectedBeers = new BeerDto[]
            {
                new BeerDto { be_Id = 1, be_Name = "Leffe Blonde", be_AlcoholContent = 6.60m, be_Price = 2.20m, br_Id = 1, br_Name = "Abbaye de Leffe" },
                new BeerDto { be_Id = 18, be_Name = "Leffe Blue", be_AlcoholContent = 8.50m, be_Price = 3.40m, br_Id = 1, br_Name = "Abbaye de Leffe" }
            };

            // Act
            var result = await _controller.ListBeersByBrewery(breweryDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Cast okResult.Value to IEnumerable<BeerDto>
            var returnedBeers = Assert.IsAssignableFrom<IEnumerable<BeerDto>>(okResult.Value);

            // Convert IEnumerable to List if needed
            var returnedBeerList = returnedBeers.ToList();

            // Validate that data matches expected results
            Assert.NotEmpty(returnedBeerList);
            Assert.Equal(expectedBeers.Length, returnedBeerList.Count);

            // Compare each item in the lists
            foreach (var expectedBeer in expectedBeers)
            {
                var matchingBeer = returnedBeerList.FirstOrDefault(beer => beer.be_Id == expectedBeer.be_Id);
                Assert.NotNull(matchingBeer); // Ensure that the beer is in the result

                // Validate each property
                Assert.Equal(expectedBeer.be_Name, matchingBeer.be_Name);
                Assert.Equal(expectedBeer.be_AlcoholContent, matchingBeer.be_AlcoholContent);
                Assert.Equal(expectedBeer.be_Price, matchingBeer.be_Price);
                Assert.Equal(expectedBeer.br_Id, matchingBeer.br_Id);
                Assert.Equal(expectedBeer.br_Name, matchingBeer.br_Name);
            }
        }

        [Fact]
        public async Task ListBeersByBrewery_ReturnsError()
        {
            // Arrange
            var breweryDto = new BreweryDto
            {
                br_Id = 0,
                br_Name = "TESTTTTT"
            };

            // Act
            var result = await _controller.ListBeersByBrewery(breweryDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error: Brewery not found.", badRequestResult.Value);
        }





        [Fact]
        public async Task AddBeer_ReturnsAddedBeer()
        {
            // Arrange
            var newBeerDto = new BeerDto
            {
                be_Name = "ALMAZA",
                be_AlcoholContent = 8.5m,
                be_Price = 7.0m,
                br_Id = 1,
                br_Name = null
            };

            var expectedBeer = new BeerDto
            {
                be_Name = "ALMAZA",
                be_AlcoholContent = 8.5m,
                be_Price = 7.0m,
                br_Id = 1,
                br_Name = "Abbaye de Leffe"
            };

            // Act
            var result = await _controller.AddBeer(newBeerDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Cast okResult.Value to IEnumerable<BeerDto>
            var returnedBeers = Assert.IsAssignableFrom<IEnumerable<BeerDto>>(okResult.Value);

            // Convert IEnumerable to List if needed
            var returnedBeerList = returnedBeers.ToList();

            // Validate that data matches expected results
            Assert.NotEmpty(returnedBeerList);
            
            // Compare the returned item to the expected item
            var returnedBeer = returnedBeerList.First();
            Assert.Equal(expectedBeer.be_Name, returnedBeer.be_Name);
            Assert.Equal(expectedBeer.be_AlcoholContent, returnedBeer.be_AlcoholContent);
            Assert.Equal(expectedBeer.be_Price, returnedBeer.be_Price);
            Assert.Equal(expectedBeer.br_Id, returnedBeer.br_Id);
            Assert.Equal(expectedBeer.br_Name, returnedBeer.br_Name);
        }

        [Fact]
        public async Task AddBeer_ReturnsError()
        {
            // Arrange
            var newBeerDto = new BeerDto
            {
                be_Name = "ALMAZA",
                be_AlcoholContent = 8.5m,
                be_Price = 7.0m,
                br_Id = 0,
                br_Name = "TESTTTTTTTT"
            };

            // Act
            var result = await _controller.AddBeer(newBeerDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error: Brewery not found.", badRequestResult.Value);
        }





        [Fact]
        public async Task DeleteBeer_ReturnsSuccess()
        {
            // Arrange
            var beerDto = new BeerDto
            {
                be_Id = null,
                be_Name = "ALMAZA"
            };

            var expectedBeer = new BeerDto
            {
                be_Name = "ALMAZA",
            };

            // Act
            var result = await _controller.DeleteBeer(beerDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Cast okResult.Value to IEnumerable<BeerDto>
            var returnedBeers = Assert.IsAssignableFrom<IEnumerable<BeerDto>>(okResult.Value);

            // Convert IEnumerable to List if needed
            var returnedBeerList = returnedBeers.ToList();

            // Validate that data matches expected results
            Assert.NotEmpty(returnedBeerList);

            // Compare the returned item to the expected item
            var returnedBeer = returnedBeerList.First();
            Assert.Equal(expectedBeer.be_Name, returnedBeer.be_Name);
        }

        [Fact]
        public async Task DeleteBeer_ReturnsError()
        {
            // Arrange
            var beerDto = new BeerDto
            {
                be_Id = null,
                be_Name = "TESTTTTTT"
            };

            var expectedBeer = new BeerDto
            {
                be_Name = "ALMAZA",
            };

            // Act
            var result = await _controller.DeleteBeer(beerDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error: Beer not found.", badRequestResult.Value);
        }
    }
}
