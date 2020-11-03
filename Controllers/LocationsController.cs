using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalAssignment.Models;

namespace FinalAssignment.Controllers
{
    public class LocationsController : Controller
    {
        private assignmentModel db = new assignmentModel();

        // GET: Locations
        public ActionResult Index()
        {
            return View(db.Locations.ToList());
        }

        // GET: Locations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // GET: Locations/Create
        public ActionResult Create()
        {
            if (User.IsInRole("Tutor") || User.IsInRole("Administrator"))
            {
                return View();
            }
            else
            {
                TempData["locationCreationFailedMsg"] = " Only tutors and administrators are allowed to create location! ";
                return RedirectToAction("Index");
            }
            
        }

        // POST: Locations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "locationId,locationName,description,latitude,longitude")] Location location)
        {
            if (ModelState.IsValid)
            {
                db.Locations.Add(location);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(location);
        }

        // GET: Locations/Edit/5
        public ActionResult Edit(int? id)
        {

            if (User.IsInRole("Tutor") || User.IsInRole("Administrator"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Location location = db.Locations.Find(id);
                if (location == null)
                {
                    return HttpNotFound();
                }
                return View(location);
            }
            else
            {
                TempData["locationEditFailedMsg"] = " Only tutors and administrators are allowed to edit location! ";
                return RedirectToAction("Index");
            }

            
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "locationId,locationName,description,latitude,longitude")] Location location)
        {
            if (ModelState.IsValid)
            {
                db.Entry(location).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(location);
        }

        // GET: Locations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (User.IsInRole("Tutor") || User.IsInRole("Administrator"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Location location = db.Locations.Find(id);
                if (location == null)
                {
                    return HttpNotFound();
                }
                return View(location);
            }
            else
            {
                TempData["locationDeleteFailedMsg"] = " Only tutors and administrators are allowed to Delete location! ";
                return RedirectToAction("Index");
            }
            
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Location location = db.Locations.Find(id);
            db.Locations.Remove(location);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
