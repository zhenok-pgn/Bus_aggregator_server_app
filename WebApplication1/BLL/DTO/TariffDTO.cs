﻿using App.DAL.Entities;

namespace App.WEB.BLL.DTO
{
    public class TariffDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Currency Currency { get; set; }
        public required List<RouteSegmentPriceDTO> Prices { get; set; }
    }
}
