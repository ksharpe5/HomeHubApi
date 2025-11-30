namespace HomeHubApi.DTOs;

public class TapoDeviceDto
{
    public int Id { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;
    public bool IsOn { get; set; }
}