using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyond_The_Stars.Locations;

namespace Beyond_The_Stars.DataProviders
{
    public interface IDataProvider
    {
        SolSystem GetPlayerLocation(out Dictionary<int, SolSystem> allSystems);
    }
}
