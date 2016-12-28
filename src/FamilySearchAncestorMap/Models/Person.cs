using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilySearchAncestorMap.Models
{
    public class Person
    {
		public string Name { get; set; }
		public string Birth { get; set; }
		public string Death { get; set; }
		public string BirthAddress { get; set; }
		public string DeathAddress { get; set; }
		public string Id { get; set; }
	}
}
