using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vega.Controllers.Resources;
using vega.Core.Models;
using vega.Core;

namespace vega.Controllers
{
    [Route("/api/vehicles")]
    public class VehiclesController : Controller
    {
        private readonly IMapper _mapper;
        
        private readonly IVehicleRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public VehiclesController(
            IMapper mapper, 
             
            IVehicleRepository repository,
            IUnitOfWork unitOfWork
            )
        {
            _mapper = mapper;
            
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] SaveVehicleResource saveVehicleResource)
        {
           
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            /*Leaving this in here just for reference only
             this should be constructed from the client not the API*/

//            var model = await _context.Models.FindAsync(vehicleResource.ModelId);
//            if (model == null)
//            {
//                ModelState.AddModelError("ModelId", "Invalid ModelId");
//                return BadRequest(ModelState);
//            }
            
            var vehicle = _mapper.Map<SaveVehicleResource, Vehicle>(saveVehicleResource);
            vehicle.LastUpdate = DateTime.Now;
            
            _repository.Add(vehicle);
            await _unitOfWork.CompleteAsync();


            vehicle = await _repository.GetVehicle(vehicle.Id);
            
            var result = _mapper.Map<Vehicle, VehicleResource>(vehicle);
            return Ok(result);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] SaveVehicleResource saveVehicleResource)
        {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

            var vehicle = await _repository.GetVehicle(id);
            
            if (vehicle == null)
                return NotFound();
            
            _mapper.Map<SaveVehicleResource, Vehicle>(saveVehicleResource, vehicle);
            vehicle.LastUpdate = DateTime.Now;


            await _unitOfWork.CompleteAsync();

            vehicle = await _repository.GetVehicle(vehicle.Id);

            var result = _mapper.Map<Vehicle, VehicleResource>(vehicle);
            return Ok(result);
         }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await _repository.GetVehicle(id, includeRelated: false);

            if (vehicle == null)
                return NotFound();
            
            
            _repository.Remove(vehicle);
            await _unitOfWork.CompleteAsync();

            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(int id)
        {
            var vehicle = await _repository.GetVehicle(id);
                
            if (vehicle == null)
                return NotFound();

            var vehicleResource = _mapper.Map<Vehicle, VehicleResource>(vehicle);
            return Ok(vehicleResource);

        }
        
        [HttpGet]
        public async Task<QueryResultResource<VehicleResource>> GetVehicles(VehicleQueryResource vehicleQueryResource)
        {

            var filter = _mapper.Map<VehicleQueryResource, VehicleQuery>(vehicleQueryResource);
            var queryResult = await _repository.GetVehicles(filter);

            return _mapper.Map<QueryResult<Vehicle>, QueryResultResource<VehicleResource>>(queryResult);
        }
    }
}