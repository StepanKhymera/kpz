using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class PoliciesController : Controller
    {
        // GET: PoliciesController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PoliciesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PoliciesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PoliciesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PoliciesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PoliciesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PoliciesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PoliciesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
