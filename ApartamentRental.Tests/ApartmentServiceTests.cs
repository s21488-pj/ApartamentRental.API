using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApartamentRental.Core.DTO;
using ApartamentRental.Core.Services;
using ApartamentRental.Infrastructure.Entities;
using ApartamentRental.Infrastructure.Repository;
using FluentAssertions;
using Moq;
using Xunit;

namespace ApartamentRental.Tests;

public class ApartmentServiceTests
{
    
    
    [Fact]
    public async Task GetTheCheapestApartmentAsync_ShouldReturnNull_WhenApartmentsCollectionIsNull()
    {
        
        var sut = new ApartmentService(Mock.Of<IApartmentRepository>(), Mock.Of < ILandlordRepository>(),
            Mock.Of<IAddressService>());

        var result = await sut.GetTheCheapestApartmentAsync();
        result.Should().BeNull();
    }
    [Fact]
    public async Task GetTheCheapestApartmentAsync_ShouldReturnTheCheapestApartment()
    {
        var apartments = new List<Apartment>
        {
            new()
            {
                Address = new Address()
                {
                    City = "Gdańsk",
                    Country = "Poland",
                    Street = "Grunwaldzka",
                    AparmentNumber = "1",
                    BuildingNumber = "2",
                    ZipCode = "80-000"
                },
                Floor = 1,
                RentAmount = 2000,
                SquareMeters = 45,
                NumberOfRooms = 3,
                IsElevator = true,
            },
            new()
            {
                Address = new Address()
                {
                    City = "Gdynia",
                    Country = "Poland",
                    Street = "Wielkopolska",
                    AparmentNumber = "2",
                    BuildingNumber = "1",
                    ZipCode = "80-001"
                },
                Floor = 2,
                RentAmount = 1999,
                SquareMeters = 40,
                NumberOfRooms = 2
            }
            
        };

        var apartmentRepositoryMock = new Mock<IApartmentRepository>();
        apartmentRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(apartments);

        var sut = new ApartmentService(apartmentRepositoryMock.Object, Mock.Of<ILandlordRepository>(),
            Mock.Of<IAddressService>());
        var result = await sut.GetTheCheapestApartmentAsync();
        result.City.Should().Be("Gdynia");
        result.Street.Should().Be("Wielkopolska");
        result.RentAmount.Should().Be(1999);
        result.SquareMeters.Should().Be(40);
        result.NumberOfRooms.Should().Be(2);
        result.IsElevatorInBuilding.Should().BeFalse();
        
    }
    
    
}
