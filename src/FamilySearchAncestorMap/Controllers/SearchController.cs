using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FamilySearchAncestorMap.Models;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FamilySearchAncestorMap.Controllers
{
    [Route("api/[controller]/[action]")]
    public class FamilySearchController : Controller
    {
		private IFamilySearchRepository _familySearchRepository;
		private IGoogleRepository _googleRepository;
		private IOptions<AppKeys> _config;

		public FamilySearchController(IFamilySearchRepository familySearchRepository,
			IGoogleRepository googleRepository, IOptions<AppKeys> config)
		{
			_familySearchRepository = familySearchRepository;
			_googleRepository = googleRepository;
			_config = config;
		}

		[HttpGet(Name = "Settings")]
		[ActionName("Settings")]
		public string Settings()
		{			
			return "";
		}

		[HttpGet(Name="Search")]
		[ActionName("Search")]
		public List<Person> Search(string surname = null, string givenName = null)
        {
			var list = new List<Person>();
			try
			{
				var authToken = GetAuthFromCookie();
				list = _familySearchRepository.Search(authToken, surname, givenName);
			}
			catch (Exception ex)
			{
				list.Add(new Person() { Name = ex.Message } );
			}
			return list;
        }

		private string GetAuthFromCookie()
		{
			return HttpContext.Request.Cookies["FS_AUTH_TOKEN"];
		}

		[HttpGet("{pid}", Name = "Ancestry")]
		[ActionName("Ancestry")]
		public AncestryResult Ancestry(string pid)
		{
			// TODO: Tuck this all into some business layer likely in the model.

			var authToken = GetAuthFromCookie();
			var result = new AncestryResult();
			result.persons = _familySearchRepository.Ancestry(authToken, pid);

			result.places = new List<Place>();
			foreach (var person in result.persons)
			{
				result.places.Add(new Place()
				{
					Type = PlaceType.Birth,
					Name = person.BirthAddress,
					Location = _googleRepository.GetLatLong(person.BirthAddress)
				});
				result.places.Add(new Place()
				{
					Type = PlaceType.Death,
					Name = person.DeathAddress,
					Location = _googleRepository.GetLatLong(person.DeathAddress)
				});
			}
			
			return result;
		}
	}
}
