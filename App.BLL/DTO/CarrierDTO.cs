namespace App.BLL.DTO
{
    public class CarrierDTO(int id, string name, string inn, string ogrn, string ogrnip, string address, string phones)
    {
        public int Id { get; set; } = id;
        public string? Name { get; set; } = name;
        public string? Inn { get; set; } = inn;
        public string? Ogrn { get; set; } = ogrn;
        public string? Ogrnip { get; set; } = ogrnip;
        public string? Address { get; set; } = address;
        public string? OfficeHours { get; set; }
        public string? Phones { get; set; } = phones;
    }
}
