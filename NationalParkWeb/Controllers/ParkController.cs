using Microsoft.AspNetCore.Mvc;
using NationalParkWeb.Interfaces;
using NationalParkWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace NationalParkWeb.Controllers
{
    public class ParkController : Controller
    {
        private readonly iParkRepository _repo;

        public ParkController(iParkRepository parkRepository)
        {
            _repo = parkRepository;
        }

        public IActionResult Index()
        {
            return View(new Park() { });
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            var park = new Park();

            if(id == null)
            {
                return View(park);
            }

            
            park = await _repo.GetAsync(SD.ParkAPIPath, id.GetValueOrDefault());
            

            if (park == null)
            {
                return NotFound();
            }

            return View(park);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Park park)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if(files.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using var ms1 = new MemoryStream();
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                    park.Picture = p1;
                }
                else
                {
                    var dbPic = await _repo.GetAsync(SD.ParkAPIPath, park.Id);
                    if(dbPic != null)
                    {
                        park.Picture = dbPic.Picture;
                    }
                }

                if(park.Id == 0)
                {
                    await _repo.CreateAsync(SD.ParkAPIPath, park);
                }
                else
                {
                    await _repo.UpdateAsync(SD.ParkAPIPath + park.Id, park);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(park);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _repo.DeleteAsync(SD.ParkAPIPath, id);
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }

        public async Task<IActionResult> GetAllParks()
        {
            return Json(new { data = await _repo.GetAllAsync(SD.ParkAPIPath) });
        }
    }
}
