using AutoMapper;
using GeoinformationModeling.DataAccess.Entities;
using GeoinformationModeling.Web.Entities;
using GeoinformationModeling.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoinformationModeling.Web.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<GeoinformationModelingViewModel, DataAccess.Entities.RiverParams>()
                .ForMember(d => d.Alpha, s => s.MapFrom(k => k.TaskParams.Alpha))
                .ForMember(d => d.AngleOfInclinationSine, s => s.MapFrom(k => k.TaskParams.AngleOfInclinationSine))
                .ForMember(d => d.AngleSine, s => s.MapFrom(k => k.TaskParams.AngleSine))
                .ForMember(d => d.ChannelHydraulicRadius, s => s.MapFrom(k => k.TaskParams.ChannelHydraulicRadius))
                .ForMember(d => d.FreeSurfaceWidth, s => s.MapFrom(k => k.TaskParams.FreeSurfaceWidth))
                .ForMember(d => d.GravityAcceleration, s => s.MapFrom(k => k.TaskParams.GravityAcceleration))
                .ForMember(d => d.LengthByT, s => s.MapFrom(k => k.PiverParams.LengthByT))
                .ForMember(d => d.LengthByX, s => s.MapFrom(k => k.PiverParams.LengthByX))
                .ForMember(d => d.OutputMultiplicityByT, s => s.MapFrom(k => k.PiverParams.OutputMultiplicityByT))
                .ForMember(d => d.OutputMultiplicityByX, s => s.MapFrom(k => k.PiverParams.OutputMultiplicityByX))
                .ForMember(d => d.SplitsNumberByT, s => s.MapFrom(k => k.PiverParams.SplitsNumberByT))
                .ForMember(d => d.SplitsNumberByX, s => s.MapFrom(k => k.PiverParams.SplitsNumberByX))
                .ForMember(d => d.ShaziCoefficient, s => s.MapFrom(k => k.TaskParams.ShaziCoefficient))
                .ForMember(d => d.UserId, s => s.MapFrom(k => k.UserId))
                .ForMember(d => d.Id, s => s.MapFrom(k => k.RiverId))
                .ForMember(d => d.RiverName, s => s.MapFrom(k => k.RiverName));

            CreateMap<DataAccess.Entities.RiverParams, TaskParams>()
              .ForMember(d => d.Alpha, s => s.MapFrom(k => k.Alpha))
              .ForMember(d => d.AngleOfInclinationSine, s => s.MapFrom(k => k.AngleOfInclinationSine))
              .ForMember(d => d.AngleSine, s => s.MapFrom(k => k.AngleSine))
              .ForMember(d => d.ChannelHydraulicRadius, s => s.MapFrom(k => k.ChannelHydraulicRadius))
              .ForMember(d => d.FreeSurfaceWidth, s => s.MapFrom(k => k.FreeSurfaceWidth))
              .ForMember(d => d.GravityAcceleration, s => s.MapFrom(k => k.GravityAcceleration))
              .ForMember(d => d.ShaziCoefficient, s => s.MapFrom(k => k.ShaziCoefficient));

            CreateMap<DataAccess.Entities.RiverParams, GeoinformationModeling.Web.Entities.RiverParams>()
              .ForMember(d => d.LengthByT, s => s.MapFrom(k => k.LengthByT))
              .ForMember(d => d.LengthByX, s => s.MapFrom(k => k.LengthByX))
              .ForMember(d => d.OutputMultiplicityByT, s => s.MapFrom(k => k.OutputMultiplicityByT))
              .ForMember(d => d.OutputMultiplicityByX, s => s.MapFrom(k => k.OutputMultiplicityByX))
              .ForMember(d => d.SplitsNumberByT, s => s.MapFrom(k => k.SplitsNumberByT))
              .ForMember(d => d.SplitsNumberByX, s => s.MapFrom(k => k.SplitsNumberByX));

            CreateMap<DataAccess.Entities.RiverParams, GeoinformationModelingViewModel>()
               .ForMember(d => d.UserId, s => s.MapFrom(k => k.UserId))
               .ForMember(d => d.RiverId, s => s.MapFrom(k => k.Id))
               .ForMember(d => d.RiverName, s => s.MapFrom(k => k.RiverName));

            CreateMap<MapPointModel, MapParams>()
                .ForMember(d => d.PointY, s => s.MapFrom(k => k.PointY))
                .ForMember(d => d.PointX, s => s.MapFrom(k => k.PointX))
                .ForMember(d => d.SequenceNumber, s => s.MapFrom(k => k.SequenceNumber));
            CreateMap<MapParams, MapPointModel>()
                .ForMember(d => d.PointY, s => s.MapFrom(k => k.PointY))
                .ForMember(d => d.PointX, s => s.MapFrom(k => k.PointX))
                .ForMember(d => d.SequenceNumber, s => s.MapFrom(k => k.SequenceNumber));
        }
    }
}
