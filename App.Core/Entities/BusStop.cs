namespace App.Core.Entities
{
    /// <summary>
    /// Остановка или станция, находится в населенном пункте (Locality)
    /// </summary>
    public class BusStop
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public StationType Type { get; set; }
        public string? Address { get; set; }
        public int LocalityId { get; set; }
        public Locality? Locality { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }

    public enum StationType
    {
        Station,
        Terminal,
        Stop,
        Other
    }
}
