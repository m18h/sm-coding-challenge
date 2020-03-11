using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sm_coding_challenge.Models;
using sm_coding_challenge.Services.DataProvider;

namespace sm_coding_challenge.Controllers
{
    public class HomeController : Controller
    {
        private IDataProvider _dataProvider;
        public HomeController(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        }

        public IActionResult Index()
        {
            return View();
        }

        // Made method async by renaming method, and changing return type
        [HttpGet]
        public async Task<IActionResult> PlayerAsync(string id)
        {
            // Added try-catch clock to catch exceptions and gracefully return them
            try
            {
                // validate id param
                if (!string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest(new
                    {
                        Message = "Invalid ID"
                    });
                }

                // Renamed method to get player by id to use new async method name
                var data = await _dataProvider.GetPlayerByIdAsync(id);
                // Return an OK object result instead of just a json result
                return Ok(data);
            }
            catch (Exception ex)
            {
                // Return error result
                return ReturnError(ex);
            }
        }

        // Made method async by renaming method, and changing return type
        [HttpGet]
        public async Task<IActionResult> PlayersAsync(string ids)
        {
            // Added try-catch clock to catch exceptions and gracefully return them
            try
            {
                // validate id param
                if (!string.IsNullOrWhiteSpace(ids))
                {
                    return BadRequest(new
                    {
                        Message = "Invalid ID"
                    });
                }

                var idList = ids.Split(',');
                var returnList = new List<PlayerModel>();
                foreach (var id in idList)
                {
                    // Renamed method to get player by id to use new async method name
                    returnList.Add(await _dataProvider.GetPlayerByIdAsync(id));
                }
                // Return an OK object result instead of just a json result
                return Ok(returnList);
            }
            catch (Exception ex)
            {
                // Return error result
                return ReturnError(ex);
            }
        }

        // Made method async by renaming method, and changing return type
        [HttpGet]
        public async Task<IActionResult> LatestPlayersAsync(string ids)
        {
            // Added try-catch clock to catch exceptions and gracefully return them
            try
            {
                // split ids into array
                var idList = ids.Split(',');

                return Ok(await _dataProvider.GetLatestPlayersAsync(idList));
            }
            catch (Exception ex)
            {
                // Return error result
                return ReturnError(ex);
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// This creates a custom error result
        /// </summary>
        /// <param name="ex">An exception object from a try-catch</param>
        /// <returns>A json object with the message from the exception that was thrown. This also sets the response code as 500</returns>
        [NonAction]
        private JsonResult ReturnError(Exception ex)
        {
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return new JsonResult(new
            {
                Message = ex.Message
            });
        }
    }
}
