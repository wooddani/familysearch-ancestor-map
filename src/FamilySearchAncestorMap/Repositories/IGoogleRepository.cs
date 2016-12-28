using FamilySearchAncestorMap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilySearchAncestorMap
{
    public interface IGoogleRepository
    {
		Location GetLatLong(string address);
    }
}
