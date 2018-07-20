using System;
using System.IO;
using System.Linq;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using vega.Controllers.Resources;
using vega.Core;
using vega.Core.Models;

namespace vega.Controllers
{
    public class PhotosController : Controller
    {

       
        private readonly IHostingEnvironment _host;
        private readonly IVehicleRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly PhotoSettings photoSettings;

        public PhotosController(
            IHostingEnvironment host, 
            IVehicleRepository repository, 
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOptionsSnapshot<PhotoSettings> options
            )
        {
            photoSettings = options.Value;
            _host = host;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        /// <summary>
        /// Routing should be
        /// api/vehicles/1/photos
        /// </summary>
        [Route("/api/vehicles/{vehicleId}/photos")]
        
        [HttpPost]
        public async Task<IActionResult> Upload(int vehicleId, IFormFile file)
        {
            var vehicle = await _repository.GetVehicle(vehicleId, includeRelated: false);
            if (vehicle == null)
                return NotFound();
            
            //Edge Cases
            if (file == null) return BadRequest("NUll FILE");
            if (file.Length == 0) return BadRequest("File Contains no data");
            if (file.Length > photoSettings.MaxBytes) return BadRequest("File is too large");
            if (!photoSettings.IsSupported(file.FileName))
                return BadRequest("The File type is incorrect");
            
            
            
            
            var uploadsFolderPath = Path.Combine(_host.WebRootPath, "uploads");

            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            var filePath = Path.Combine(uploadsFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var photo = new Photo {FileName = fileName};
            vehicle.Photos.Add(photo);

            await _unitOfWork.CompleteAsync();

            return Ok(_mapper.Map<Photo, PhotoResource>(photo));
        }
    }
}