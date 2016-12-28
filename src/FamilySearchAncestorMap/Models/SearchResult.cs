using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilySearchAncestorMap.Models
{
    public class SearchResult
    {
		public string Context { get; set; }
		public int Total { get; set; }
		public List<Person> Persons { get; set; }
	}
}
