using System.Device.Location;
using System.Collections.Generic;

namespace streetmapping
{
    class Vertex
    {
        public string name;
        public GeoCoordinate coordinates;
        public List<Vertex> adjList;
    }
}
