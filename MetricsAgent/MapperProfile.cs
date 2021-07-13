using AutoMapper;
using MetricsAgent.DAL.Models;
using MetricsAgent.DTO;
using MetricsAgent.Requests;
using MetricsAgent.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CpuMetric, CpuMetricDto>().ForMember(metricDto => metricDto.Date, item => item.MapFrom(metric => DateTimeOffset.FromUnixTimeSeconds(metric.Time)));
            CreateMap<CpuMetricDto, CpuMetric>().ForMember(metric => metric.Time, item => item.MapFrom(metricDto => metricDto.Date.ToUnixTimeSeconds()));
            CreateMap<CpuMetricCreateRequest, CpuMetric>().ForMember(metric => metric.Time, item => item.MapFrom(metricCreateRequest => metricCreateRequest.Date.ToUnixTimeSeconds()));

            CreateMap<DotNetMetric, DotNetMetricDto>().ForMember(metricDto => metricDto.Date, item => item.MapFrom(metric => DateTimeOffset.FromUnixTimeSeconds(metric.Time)));
            CreateMap<DotNetMetricDto, DotNetMetric>().ForMember(metric => metric.Time, item => item.MapFrom(metricDto => metricDto.Date.ToUnixTimeSeconds()));
            CreateMap<DotNetMetricCreateRequest, DotNetMetric>().ForMember(metric => metric.Time, item => item.MapFrom(metricCreateRequest => metricCreateRequest.Date.ToUnixTimeSeconds()));

            CreateMap<HddMetric, HddMetricDto>().ForMember(metricDto => metricDto.Date, item => item.MapFrom(metric => DateTimeOffset.FromUnixTimeSeconds(metric.Time)));
            CreateMap<HddMetricDto, HddMetric>().ForMember(metric => metric.Time, item => item.MapFrom(metricDto => metricDto.Date.ToUnixTimeSeconds()));
            CreateMap<HddMetricCreateRequest, HddMetric>().ForMember(metric => metric.Time, item => item.MapFrom(metricCreateRequest => metricCreateRequest.Date.ToUnixTimeSeconds()));

            CreateMap<NetworkMetric, NetworkMetricDto>().ForMember(metricDto => metricDto.Date, item => item.MapFrom(metric => DateTimeOffset.FromUnixTimeSeconds(metric.Time)));
            CreateMap<NetworkMetricDto, NetworkMetric>().ForMember(metric => metric.Time, item => item.MapFrom(metricDto => metricDto.Date.ToUnixTimeSeconds()));
            CreateMap<NetworkMetricCreateRequest, NetworkMetric>().ForMember(metric => metric.Time, item => item.MapFrom(metricCreateRequest => metricCreateRequest.Date.ToUnixTimeSeconds()));

            CreateMap<RamMetric, RamMetricDto>().ForMember(metricDto => metricDto.Date, item => item.MapFrom(metric => DateTimeOffset.FromUnixTimeSeconds(metric.Time)));
            CreateMap<RamMetricDto, RamMetric>().ForMember(metric => metric.Time, item => item.MapFrom(metricDto => metricDto.Date.ToUnixTimeSeconds()));
            CreateMap<RamMetricCreateRequest, RamMetric>().ForMember(metric => metric.Time, item => item.MapFrom(metricCreateRequest => metricCreateRequest.Date.ToUnixTimeSeconds()));
        }
    }
}
