using System.Collections;
using ApartamentRental.Core.DTO;
using ApartamentRental.Infrastructure.Entities;
using ApartamentRental.Infrastructure.Repository;
 
namespace ApartamentRental.Core.Services;

public class ApartmentService : IApartmentService
{
    private readonly IApartmentRepository _apartmentRepository;
    private readonly ILandlordRepository _landlordRepository;
    private readonly IAddressService _addressService;
    public ApartmentService(IApartmentRepository apartmentRepository, ILandlordRepository landlordRepository, IAddressService addressService)
    {
        _apartmentRepository = apartmentRepository;
        _landlordRepository = landlordRepository;
        _addressService = addressService;
    }

    public async Task<IEnumerable<ApartmentBasicInformationResponseDto>> GetAllApartmentsBasicInfosAsync()
    {
        var apartments = await _apartmentRepository.GetAll();
        return apartments.Select(x => new ApartmentBasicInformationResponseDto(
            x.RentAmount,
            x.NumberOfRooms,
            x.SquareMeters,
            x.IsElevator,
            x.Address.City,
            x.Address.Street
        ));
    }

    public Task AddNewApartmentToExistingLandLordAsync(ApartmentBasicInformationResponseDto dto)
    {
        throw new NotImplementedException();
    }


    public async Task AddNewApartmentToExistingLandLordAsync(ApartmentCreationRequestDto dto)
    {
        var landlord = await _landlordRepository.GetById(dto.LandLordId);

        var addressId = await _addressService.GetAddressIdOrCreateAsync(dto.Country,
            dto.City, dto.ZipCode, dto.Street, dto.BuildingNumber, dto.ApartmentNumber);

        await _apartmentRepository.Add(new Apartment
        {
            AddressId = addressId,
            Floor = dto.Floor,
            LandlordId = landlord.Id,
            IsElevator = dto.IsElevator,
            RentAmount = dto.RentAmount,
            SquareMeters = dto.SquareMeters,
            NumberOfRooms = dto.NumberOfRooms

        });

    }

    public async Task<ApartmentBasicInformationResponseDto?> GetTheCheapestApartmentAsync()
    {
        var apartments = await _apartmentRepository.GetAll();
        var cheapestOne = apartments.MinBy(x => x.RentAmount);
        if (cheapestOne is null) return null;
        return new ApartmentBasicInformationResponseDto(
            cheapestOne.RentAmount,
            cheapestOne.NumberOfRooms,
            cheapestOne.SquareMeters,
            cheapestOne.IsElevator,
            cheapestOne.Address.City,
            cheapestOne.Address.Street);

    }
    
    
}