using Falsify_Site.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System;

namespace Falsify_Site.Server.Services
{
    public class ShowsService
    {
        private readonly IMongoCollection<LiveShows> _liveShowsCollection;
        private readonly IMongoCollection<Venue> _venueCollection;

        public ShowsService(
            IOptions<FalsifyDatabaseSettings> showsStoreDatabaseSettings)
        {
            // Replace the placeholder with your Atlas connection string
            string connectionUri = showsStoreDatabaseSettings.Value.ConnectionString;
            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            // Sets the ServerApi field of the settings object to Stable API version 1
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            settings.UseTls = true;
            // Creates a new client and connects to the server
            var client = new MongoClient(settings);

            var mongoDatabase = client.GetDatabase(
                showsStoreDatabaseSettings.Value.DatabaseName);

            _liveShowsCollection = mongoDatabase.GetCollection<LiveShows>(
                showsStoreDatabaseSettings.Value.LiveShowsCollectionName);

            _venueCollection = mongoDatabase.GetCollection<Venue>(
                showsStoreDatabaseSettings.Value.VenueCollectionName);
        }

        public async Task<List<LiveShows>> GetAllShowsAsync()
        {
            var shows = await _liveShowsCollection.Find(_ => true).ToListAsync();

            foreach (var show in shows)
            {
                var venue = await _venueCollection.Find(v => v.Id == show.VenueId).FirstOrDefaultAsync();
                if (venue != null)
                {
                    show.Venue = venue;
                }
            }

            return shows;
        }
        public async Task<List<LiveShows>> GetShowForDateAsync(string id)
        {
            var filter = Builders<LiveShows>.Filter.Eq("DateId", id);
            return await _liveShowsCollection.Find(filter).ToListAsync();
        }
        
        public async Task<List<LiveShows>> GetShowsForMonthAsync(string id)
        {
            var filter = Builders<LiveShows>.Filter.Regex("DateId", new BsonRegularExpression("^(" + id + ")"));
             return await _liveShowsCollection.Find(filter).ToListAsync();
        }

        public async Task CreateAsync(LiveShows newShows) =>
            await _liveShowsCollection.InsertOneAsync(newShows);

        public async Task<List<LiveShows>> GetAllShowsVenuesAsync(List<LiveShows> shows)
        {
            for(int i = 0; i < shows.Count; i++)
            {
                var filter = Builders<Venue>.Filter.Eq("VenueId", shows[i].VenueId);
                var venue = await _venueCollection.Find(filter).FirstOrDefaultAsync();
                shows[i].VenueId = venue.Id;
                shows[i].Venue = new Venue(venue.Name, venue.Address);
            }

            return shows;
        }

        public async Task<ActionResult<Venue>> GetVenueForShowAsync(string dateId)
        {
            var sFilter = Builders<LiveShows>.Filter.Eq("DateId", dateId);
            try
            {
                LiveShows show = _liveShowsCollection.Find(sFilter).FirstOrDefault();
                var vFilter = Builders<Venue>.Filter.Eq("Id", show.VenueId);

                return await _venueCollection.Find(vFilter).FirstOrDefaultAsync();
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine("No show for date "+ dateId);
                return null;
            }
        }
    }
}
