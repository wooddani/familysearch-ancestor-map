using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilySearchAncestorMap.Models
{
	public class Place
	{
		public PlaceType Type {get; set;}
		public string Name { get; set; }
		public Location Location { get; set; }
	}

	public enum PlaceType
	{
		Birth,
		Death
	}
}
