namespace App.Core.Entities
{
    /// <summary>
    /// Остановка или станция, находится в населенном пункте (Locality)
    /// </summary>
    public class BusStop
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public int LocalityOsmId { get; set; }
        public Locality? Locality { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
