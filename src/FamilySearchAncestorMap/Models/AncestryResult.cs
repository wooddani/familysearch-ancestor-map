using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilySearchAncestorMap.Models
{
    public class AncestryResult
    {
		public List<Person> persons { get; set; }
		public List<Place> places { get; set; }
	}
}
