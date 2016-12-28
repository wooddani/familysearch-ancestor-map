using FamilySearchAncestorMap.Models;
using System.Collections.Generic;

namespace FamilySearchAncestorMap
{
	public interface IFamilySearchRepository
	{
		List<Person> Search(string authToken, string surname, string givenName);
		List<Person> Ancestry(string authToken, string pid);
	}
}