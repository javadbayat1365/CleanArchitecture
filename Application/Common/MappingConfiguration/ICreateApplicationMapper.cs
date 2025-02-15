﻿using AutoMapper;

namespace Application.Common.MappingConfiguration;

public interface ICreateApplicationMapper<TSource>
{
    void Map(Profile profile)
    {
        profile.CreateMap(typeof(TSource),GetType()).ReverseMap();
    }
}
