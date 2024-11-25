using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.Entities
{
    public class Price
    {
        public int Id { get; set; }
        public int TariffId { get; set; }
        public Tariff? Tariff { get; set; }
        public int DepartureStationId { get; set; }
        public Station? DepartureStation { get; set; }
        public int ArrivalStationId { get; set; }
        public Station? ArrivalStation { get; set; }
        public int Value { get; set; }
    }
}
