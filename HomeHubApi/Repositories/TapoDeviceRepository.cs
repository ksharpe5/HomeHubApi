using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeHubApi.Data;
using HomeHubApi.Models;
using HomeHubApi.SettingsModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TapoConnect;
using TapoConnect.Dto;
using TapoConnect.Protocol;
using TapoConnect.Util;
using TapoDeviceDto = HomeHubApi.DTOs.TapoDeviceDto;

namespace HomeHubApi.Repositories;

public class TapoDeviceRepository : ITapoDeviceRepository
{
    private readonly HomeHubContext _context;
    private readonly IMapper _mapper;
    private readonly string _username;
    private readonly string _password;

    private readonly TapoDeviceClient _deviceClient;

    public TapoDeviceRepository(
        HomeHubContext context,
        IMapper mapper,
        IOptions<TapoCredentials> opts)
    {
        _context = context;
        _mapper = mapper;
        _username = opts.Value.Username;
        _password = opts.Value.Password;
        
        _deviceClient = new TapoDeviceClient([
            new KlapDeviceClient(), 
            new SecurePassthroughDeviceClient()
        ]);
    }

    private async Task<TapoDeviceDto> GetDeviceInfoByIp(string ipAddress, int? id = null)
    {
        var deviceKey = await _deviceClient.LoginByIpAsync(ipAddress, _username, _password);
        var deviceInfo = await _deviceClient.GetDeviceInfoAsync(deviceKey);

        return new TapoDeviceDto
        {
            Id = id ?? -1,
            DeviceName = deviceInfo.Nickname,
            IsOn = deviceInfo.DeviceOn,
            IpAddress = ipAddress,
        };
    }

    public async Task<IEnumerable<TapoDeviceDto>> GetAll()
    {
        var result = new List<TapoDeviceDto>();
        var devices = await _context.TapoDevices.ToListAsync();
        
        foreach (var device in devices)
        {
            var dto = await GetDeviceInfoByIp(device.IpAddress, device.Id);
            result.Add(dto);
        }

        return result;
    }

    public async Task TurnOffDevice(string ipAddress)
    {
        var deviceKey = await _deviceClient.LoginByIpAsync(ipAddress, _username, _password);
        await _deviceClient.SetStateAsync(deviceKey, new TapoSetPlugState(false));
    }
    
    public async Task TurnOnDevice(string ipAddress)
    {
        var deviceKey = await _deviceClient.LoginByIpAsync(ipAddress, _username, _password);
        await _deviceClient.SetStateAsync(deviceKey, new TapoSetPlugState(true));
    }
}

public interface ITapoDeviceRepository
{
    Task<IEnumerable<TapoDeviceDto>> GetAll();
    Task TurnOffDevice(string ipAddress);
    Task TurnOnDevice(string ipAddress);
}