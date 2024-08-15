using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Models;
using robot_controller_api.Persistence;

namespace robot_controller_api.Controllers
{
    [ApiController]
    [Route("api/maps")]
    public class MapsController : ControllerBase
    {
        private readonly IMapDataAccess _mapRepo ;

        public MapsController(IMapDataAccess mapRepo)
        {
            _mapRepo = mapRepo;
        }

        [HttpGet]
        public IEnumerable<Map> GetAllMaps()
        {
            return _mapRepo.GetMaps();
        }

        [HttpGet("square")]
        public IEnumerable<Map> GetSquareMaps()
        {
            var squareMaps = _mapRepo.GetMaps().Where(m => m.Columns == m.Rows).ToList();
            return squareMaps;
        }

        [HttpGet("{id}")]
        public IActionResult GetMapById(int id)
        {
            var map = _mapRepo.GetMaps().FirstOrDefault(m => m.Id == id);
            if (map == null)
            {
                return NotFound();
            }
            return Ok(map);
        }

        [HttpPost]
        public IActionResult AddMap(Map newMap)
        {
            if (newMap == null)
            {
                return BadRequest();
            }

            

            _mapRepo.InsertMap(newMap);

            return CreatedAtAction(nameof(GetMapById), new { id = newMap.Id }, newMap);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMap(int id, Map updatedMap)
        {
            var map = _mapRepo.GetMaps().FirstOrDefault(m => m.Id == id);
            if (map == null)
            {
                return NotFound();
            }

            if (updatedMap == null)
            {
                return BadRequest();
            }
            map.Name = updatedMap.Name;
            map.Description = updatedMap.Description;
            map.Columns = updatedMap.Columns;
            map.Rows = updatedMap.Rows;
            map.ModifiedDate = DateTime.Now;
            _mapRepo.UpdateMap(map);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMap(int id)
        {
            var map = _mapRepo.GetMaps().FirstOrDefault(m => m.Id == id);
            if (map == null)
            {
                return NotFound();
            }

            _mapRepo.DeleteMap(id);

            return NoContent();
        }

        [HttpGet("{id}/{x}-{y}")]
        public IActionResult CheckCoordinate(int id, int x, int y)
        {
            if (x < 0 || y < 0)
            {
                return BadRequest();
            }

            var map = _mapRepo.GetMaps().FirstOrDefault(m => m.Id == id);
            if (map == null)
            {
                return NotFound();
            }

            bool isOnMap = x >= 0 && x < map.Columns && y >= 0 && y < map.Rows;

            return Ok(isOnMap);
        }
    }
}