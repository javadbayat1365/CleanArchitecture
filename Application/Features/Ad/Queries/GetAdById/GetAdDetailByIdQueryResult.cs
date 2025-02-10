using Application.Common.MappingConfiguration;
using AutoMapper;
using Domain.Entities.Ad;
using static Domain.Entities.Ad.AdEntitiy;

namespace Application.Features.Ad.Queries.GetAdById;

public record GetAdDetailByIdQueryResult(
    Guid AdId,
    string Title,
    string Description,
    Guid OwnerId,
    string OwnerUserName,
    Guid LocationId,
    string LocationName,
    Guid CategoryId,
    string CategoryName,
    AdState CurrentState,
    string OwnerPhoneNumber): ICreateApplicationMapper<AdEntitiy>
{
    public record AdDetailsImageModel(string ImageName, string ImageUrl);
    public AdDetailsImageModel[] AdImages { get; set; }

    public void Map(Profile profile) {

        profile.CreateMap<AdEntitiy, GetAdDetailByIdQueryResult>()
            .ForCtorParam(nameof(AdId), opt => opt.MapFrom(c => c.Id))
            .ForCtorParam(nameof(OwnerId), opt => opt.MapFrom(c => c.UserId))
            .ForCtorParam(nameof(OwnerUserName), opt => opt.MapFrom(c => c.UserEntity.UserName))
            .ForCtorParam(nameof(LocationName), opt => opt.MapFrom(c => c.LocationEntity.Name))
            .ForCtorParam(nameof(CategoryName), opt => opt.MapFrom(c => c.CategoryEntity.Name))
            .ForCtorParam(nameof(OwnerPhoneNumber), opt => opt.MapFrom(c => c.UserEntity.PhoneNumber))
            .ForCtorParam(nameof(CurrentState), opt => opt.MapFrom(c => c.CurrentState))
            .ReverseMap();
    }
}
