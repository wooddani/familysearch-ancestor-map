using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilySearchAncestorMap.Models;
using Gedcomx.Api.Lite.Core;
using Microsoft.Extensions.Options;

namespace FamilySearchAncestorMap
{
	public class FamilySearchRepository : IFamilySearchRepository
	{
		private IOptions<AppKeys> _config;
		
		public FamilySearchRepository(IOptions<AppKeys> config)
		{
			_config = config;

			
		}

		public List<Person> Search(string authToken, string surname, string givenName)
		{
			var searchString = "";
			if (!string.IsNullOrEmpty(surname))
			{
				searchString += $" surname:{surname}~ ";
			}
			if (!string.IsNullOrEmpty(givenName))
			{
				searchString += $" givenName:{givenName}~ ";
			}
			var encoded = Uri.EscapeDataString(searchString);

			// Pass in the auth token to our familysearch api
			var ft = new FamilySearchSDK(authToken, _config.Value.Environment);
			var searchResult = ft.Get("/platform/tree/search?q=" + encoded, MediaType.X_GEDCOMX_ATOM_JSON).Result;

			var list = new List<Person>();
			foreach (var e in searchResult.entries)
			{
				var p = e.content.gedcomx.persons[0];
				list.Add(new Person()
				{
					Id = p.id,
					Name = p.display.name,
					Birth = p.display.birthPlace + " on " + p.display.birthDate,
					Death = p.display.deathPlace + " on " + p.display.deathDate,
				});
			}
			return list;
		}

		public List<Person> Ancestry(string authToken, string personId)
		{
			List<Person> list = new List<Person>();

			// TODO: https://familysearch.org/developers/docs/api/tree/Ancestry_resource
			// Add personDetails as a parameter

			var ft = new FamilySearchSDK(authToken, _config.Value.Environment);
			var anc = ft.Get("/platform/tree/ancestry?person=" + personId).Result;
			foreach (var p in anc.persons)
			{
				var data = ft.Get("/platform/tree/persons/" + p.id).Result;
				var person = data.persons[0];
				list.Add(new Person()
				{
					Id = person.id,
					Name = person.display.name,
					Birth = person.display.birthPlace + " on " + person.display.birthDate,
					Death = person.display.deathPlace + " on " + person.display.deathDate,
					BirthAddress = person.display.birthPlace,
					DeathAddress = person.display.deathPlace,
				});
			}

			return list;
		}
	}
}
