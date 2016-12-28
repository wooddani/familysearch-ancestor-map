using FamilySearchAncestorMap.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Extensions.Options;

namespace FamilySearchAncestorMap
{
	public class GoogleRepository : IGoogleRepository
	{
		private static TimeSpan cacheTime = new TimeSpan(1, 0, 0, 0); // 1 day
		private IMemoryCache _memoryCache;
		private HttpClient _client = null;
		private Uri _baseUrl;
		private string GoogleAPIKey = "&key=";

		// Because there will be several similar locations, start builing a cache
		// TODO: start saving it in a DB.

		public GoogleRepository(IMemoryCache memoryCache, IOptions<AppKeys> config)
		{
			_memoryCache = memoryCache;
			_baseUrl = new Uri("https://maps.googleapis.com/");
			_client = new HttpClient() { BaseAddress = _baseUrl };

			GoogleAPIKey += config.Value.GoogleApiKey;
		}

		public Location GetLatLong(string address)
		{
			if (!string.IsNullOrEmpty(address))
			{
				Location loc = _memoryCache.Get(address) as Location;
				if (loc == null)
				{
					//https://maps.googleapis.com/maps/api/geocode/json?address=Herriman+Utah&key=GETYOURS
					var data = Get("maps/api/geocode/json?address=" + Uri.EscapeDataString(address)).Result;
					//Console.WriteLine($"lat {data.results[0].geometry.location.lat} long {data.results[0].geometry.location.lng}");

					if (data != null &&
						data.results != null &&
						data.results.Count > 0)
					{
						loc = new Location()
						{
							Latitude = data.results[0].geometry.location.lat,
							Longitude = data.results[0].geometry.location.lng
						};

						_memoryCache.Set(address, loc,
							new MemoryCacheEntryOptions().SetAbsoluteExpiration(cacheTime));

						return loc;
					}
				}
				return loc;
			}
			return null;
		}		

		public async Task<dynamic> Get(string apiRoute)
		{
			var url = new Uri(_baseUrl, apiRoute + GoogleAPIKey);
			var response = await _client.GetAsync(url).ConfigureAwait(false);
			var s = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject(s);
		}
	}
}
