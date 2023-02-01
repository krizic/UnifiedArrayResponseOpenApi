using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace UnifiedArrayResponseOpenApi.Controllers;

public enum SearchResultType
{
    CUSTOMER,
    TOUR,
    HOTEL
}

[SwaggerDiscriminator("type")]
[SwaggerSubType(typeof(CustomerSearchResultDto), DiscriminatorValue = nameof(SearchResultType.CUSTOMER))]
[SwaggerSubType(typeof(TourSearchResultDto), DiscriminatorValue = nameof(SearchResultType.TOUR))]
[SwaggerSubType(typeof(HotelSearchResultDto), DiscriminatorValue = nameof(SearchResultType.HOTEL))]
public abstract class SearchResultDto
{
    public string link { get; set; }
    public SearchResultType type { get; set; }
}

// Customer
public class CustomerId
{
    public string Id { get; set; }
    public string Company { get; set; }
}

class CustomerSearchResultDto : SearchResultDto
{
    public CustomerId Id { get; set; }
}

// Tour
class TourId
{
    public int Id { get; set; }
    public string Season { get; set; }
}

class TourSearchResultDto : SearchResultDto
{
    public TourId Id { get; set; }
}

// Hotel
class HotelId
{
    public int Id { get; set; }
    public int HotelChainNumber { get; set; }
}

class HotelSearchResultDto : SearchResultDto
{
    public HotelId Id { get; set; }
}

public class GlobalSearchResultDto
{
    [Required] public List<SearchResultDto> result { get; set; }
    public int count { get; set; }
}

[ApiController]
[Route("[controller]")]
public class GlobalSearch : ControllerBase
{
    public static readonly GlobalSearchResultDto ExampleResult = new GlobalSearchResultDto()
    {
        result = new List<SearchResultDto>
        {
            new CustomerSearchResultDto {link = "customer/123", type = SearchResultType.CUSTOMER, Id = new CustomerId {Id = "1234", Company = "2345"}},
            new TourSearchResultDto {link = "tour/123", type = SearchResultType.TOUR, Id = new TourId {Id = 1234, Season = "2023"}},
            new HotelSearchResultDto {link = "hotel/123", type = SearchResultType.HOTEL, Id = new HotelId {Id = 1234, HotelChainNumber = 5641}},
        },
        count = 3
    };

    [HttpGet(Name = "GetResults")]
    [ProducesResponseType(typeof(GlobalSearchResultDto), StatusCodes.Status200OK)]
    public JsonResult Get()
    {
        return new JsonResult(ExampleResult);
    }
}