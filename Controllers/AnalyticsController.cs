using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreamFlixAPI.Services;
using StreamFlixAPI.Data;
using StreamFlixAPI.Models;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace StreamFlixAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly GeoLocationService _geoService;
        private readonly AppDbContext _context;

        public AnalyticsController(GeoLocationService geoService, AppDbContext context)
        {
            _geoService = geoService;
            _context = context;
        }

        [HttpGet("log-access")]
        public async Task<IActionResult> LogAccess()
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "8.8.8.8";

            // Handle IPv6 loopback for local dev
           if (ip == "::1") ip = "8.8.8.8";

            var geo = await _geoService.GetLocationAsync(ip);

            var log = new AccessLog
            {
                Ip = geo.Query,
                Country = geo.Country,
                City = geo.City,
                Isp = geo.Isp,
                Timezone = geo.Timezone
            };

            _context.AccessLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Access logged", data = log });
        }

        [HttpGet("all-logs")]
        public async Task<IActionResult> GetAllLogs()
        {
            var logs = await _context.AccessLogs
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();

            return Ok(logs);
        }
    }
}
