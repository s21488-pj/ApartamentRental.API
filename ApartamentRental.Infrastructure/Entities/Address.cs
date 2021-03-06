namespace ApartamentRental.Infrastructure.Entities;

public class Address : BaseEntity
{
    public new int Id { get; set; }
    public string Street{ get; set; }
    public string? AparmentNumber{ get; set; } 
    public string? BuildingNumber{ get; set; } 
    public string City{ get; set; } 
    public string ZipCode{ get; set; } 
    public string Country{ get; set; } 
    
}