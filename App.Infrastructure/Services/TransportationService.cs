using App.Application.DTO;
using App.Application.DTO.Requests;
using App.Application.Services;
using App.Core.Entities;
using App.Infrastructure.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Services
{
    public class TransportationService : ITransportationService
    {
        private ApplicationDBContext _db; 
        private IAuthService _authService;
        private IMapper _mapper;
        public TransportationService(IAuthService authService, ApplicationDBContext db, IMapper mapper) { 
            _authService = authService;
            _db = db;
            _mapper = mapper;
        }

        public async Task CreateBusAsync(BusDTO dto)
        {
            var bus = _mapper.Map<Bus>(dto);
            _db.Buses.Add(bus);
            await _db.SaveChangesAsync();
        }

        public async Task CreateDriverAsync(DriverRegisterRequest request)
        {
            await _authService.RegisterDriverAsync(request);
        }

        public Task DeleteBusAsync(int carrierId, int busId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteDriverAsync(int carrierId, int routeId)
        {
            throw new NotImplementedException();
        }

        public async Task<BusDTO> GetBusAsync(int carrierId, int busId)
        {
            var bus = await _db.Buses.Include(b => b.Seats).FirstOrDefaultAsync(b => b.Id == busId);
            if (bus == null)
                throw new KeyNotFoundException($"Bus with ID {bus} was not found.");
            if (carrierId != bus.CarrierId)
                throw new UnauthorizedAccessException("You do not have permission to bus for this carrier.");

            return _mapper.Map<BusDTO>(bus);
        }

        public async Task<List<BusDTO>> GetBusesAsync(int carrierId)
        {
            var buses = await _db.Buses.Include(b=>b.Seats)
           .Where(r => r.CarrierId == carrierId)
           .ToListAsync();

            return _mapper.Map<List<BusDTO>>(buses);
        }

        public async Task<DriverDTO> GetDriverAsync(int carrierId, int driverId)
        {
            var driver = await _db.Drivers.FirstOrDefaultAsync(r => r.Id == driverId);
            if (driver == null)
                throw new KeyNotFoundException($"Driver with ID {driverId} was not found.");
            if (carrierId != driver.CarrierId)
                throw new UnauthorizedAccessException("You do not have permission to driver for this carrier.");

            return _mapper.Map<DriverDTO>(driver);
        }

        public async Task<List<DriverDTO>> GetDriversAsync(int carrierId)
        {
            var drivers = await _db.Drivers
           .Where(r => r.CarrierId == carrierId)
           .ToListAsync();

            return _mapper.Map<List<DriverDTO>>(drivers);
        }

        public Task UpdateBusAsync(BusDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task UpdateDriverAsync(DriverDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
