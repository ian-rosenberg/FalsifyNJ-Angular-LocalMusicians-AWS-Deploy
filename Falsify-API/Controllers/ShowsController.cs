using Falsify_Site.Server.Models;
using Falsify_Site.Server.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;

namespace Falsify_Site.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowsController(ShowsService showsService) : ControllerBase
    {
        private readonly ShowsService _showsService = showsService;

        [HttpGet]
        public async Task<List<LiveShows>> Get()
        {
            return await _showsService.GetAllShowsAsync();
        }

        [HttpGet("Day/{id:length(8)}")]
        public async Task<ActionResult<List<LiveShows>>> GetShowForDate(string id)
        {
            var show = await _showsService.GetShowForDateAsync(id);

            if (show is null)
            {
                return NotFound();
            }

            return show;
        }

        [HttpGet("Date/{dateId:length(8)}")]
        public async Task<ActionResult<Venue>> GetVenueForShow(string dateId)
        {
               return await _showsService.GetVenueForShowAsync(dateId);
        }

        [HttpGet("Month/{month:length(2)}")]
        public async Task<ActionResult<List<LiveShows>>> GetShowsForMonth(string month)
        {
            var shows = await _showsService.GetShowsForMonthAsync(month);

            if (shows is null || shows.Count == 0)
            {
                return NotFound();
            }

            return shows;
        }
    }
}
