using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NameDayWorker.DbEntities;
using NameDayWorker.DbFunctions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using playground_first2.DTO;
using Swashbuckle.AspNetCore.Annotations;
using System.Reflection.Metadata;

namespace NameDayWorker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class NameDayController : ControllerBase
    {
        private readonly NameDayDbContext _dbContext;

        public NameDayController(NameDayDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Import namedays
        /// </summary>
        /// <param name="file">The file containing namedays data</param>
        /// <returns></returns>
        [HttpPost("Import")]
        [SwaggerOperation(Description = "Import namedays from a given .txt file into the database")]
        public IActionResult ImportNameDays(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file was imported");

            using (var streamReader = new StreamReader(file.OpenReadStream()))
            {
                var nameDayLoader = new NameDayLoader();
                var nameDays = nameDayLoader.LoadNameDays(streamReader);

                var dbImporter = new DbImporter(_dbContext);
                dbImporter.ImportDataToDatabase(nameDays);
            }

            return Ok("Namedays were imported");
        }

        /// <summary>
        /// Získat všechny svátky z databáze
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllNameDays")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NameDay))]
        public IActionResult GetAllNameDays()
        {
            var allNamesGetter = new AllNamesGetter(_dbContext);
            var json = allNamesGetter.GetAllNameDaysFromDatabase();

            return Ok(json);
        }

        /// <summary>
        /// Find namedays by name or part of a name, you can use wildcard "*" for searching
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchByName")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<NameDay>))]
        public IActionResult SearchByName(string name)
        {
            var finder = new ByParcialNameFinder();
            var matchingNameDays = finder.FindByPartialName(name, _dbContext);

            return Ok(matchingNameDays);
        }

        /// <summary>
        /// Add a new nameday into the database
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddNameDay")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NameDay))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddNameDay([FromBody] NameDayDTO nameDayDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nameDay = NameDayMaker.CreateNameDayFromDto(nameDayDto, _dbContext);
            
            return Created("", nameDayDto);
        }

        /// <summary>
        /// Find names by date
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchByDate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetNamesByDate(string date)
        {
            var finder = new ByDateFinder(_dbContext);

            var nameDay = finder.FindByDate(date);

            if (finder.IsInvalidDate)
            {
                return BadRequest("Invalid input");
            }

            if (finder.IsInvalidFormattedDate)
            {
                return BadRequest("Invalid date");
            }

            if (finder.IsNoNameDay)
            {
                return NotFound("At 29.2. is no nameday");
            }

            var names = finder.GetNamesBySpecificDate();
            return Ok(names);
        }
    }
}
