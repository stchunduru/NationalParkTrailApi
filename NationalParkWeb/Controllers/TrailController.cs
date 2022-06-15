using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NationalParkWeb.Interfaces;
using NationalParkWeb.Models;
using NationalParkWeb.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NationalParkWeb.Controllers
{
    public class TrailController : Controller
    {
        private readonly iParkRepository _repoPark;
        private readonly iTrailRepository _repoTrail;
        public TrailController(iParkRepository repoPark, iTrailRepository repoTrail)
        {
            _repoPark = repoPark;
            _repoTrail = repoTrail;
        }

        public IActionResult Index()
        {
            return View(new Trail() { });
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<Park> parkList = await _repoPark.GetAllAsync(SD.ParkAPIPath);

            TrailsViewModel obj = new TrailsViewModel()
            {
                ParkList = parkList.Select(o => new SelectListItem
                {
                    Text = o.Name,
                    Value = o.Id.ToString()
                }),
                Trail = new Trail()

            };

            if (id == null)
            {
                return View(obj);
            }

            obj.Trail = await _repoTrail.GetAsync(SD.TrailAPIPath, id.GetValueOrDefault());

            if (obj.Trail == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsViewModel trai)
        {
            if (ModelState.IsValid)
            {
                if (trai.Trail.Id == 0)
                {
                    await _repoTrail.CreateAsync(SD.TrailAPIPath, trai.Trail);
                }
                else
                {
                    await _repoTrail.UpdateAsync(SD.TrailAPIPath + trai.Trail.Id, trai.Trail);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                IEnumerable<Park> parkList = await _repoPark.GetAllAsync(SD.ParkAPIPath);

                TrailsViewModel obj = new TrailsViewModel()
                {
                    ParkList = parkList.Select(o => new SelectListItem
                    {
                        Text = o.Name,
                        Value = o.Id.ToString()
                    }),
                    Trail = new Trail()

                };
                return View(obj);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _repoTrail.DeleteAsync(SD.TrailAPIPath, id);
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }

        public async Task<IActionResult> GetAllTrails()
        {
            return Json(new { data = await _repoTrail.GetAllAsync(SD.TrailAPIPath) });
        }

    }
}
