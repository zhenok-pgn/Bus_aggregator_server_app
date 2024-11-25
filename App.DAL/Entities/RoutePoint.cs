using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.Entities
{
    public class RoutePoint
    {
        public int Id { get; set; }
        public int StationId { get; set; }
        public Station? Station { get; set; }
        public int RouteId { get; set; }
        public Route? Route { get; set; }
        public int? RoutePointId { get; set; }
        public RoutePoint? PreviousRoutePoint { get; set; }
        public bool IsBoarding { get; set; }
        public bool IsDisembarkation { get; set; }
        public bool IsLongTermParking { get; set; }
        public TimeOnly? HoursOnTheRoad { get; set; }
        public TimeOnly? HoursOfTheParking { get; set; }
    }
}
