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
    public class LecturesController : Controller
    {
        private assignmentModel db = new assignmentModel();

        // GET: Lectures
        public ActionResult Index()
        {
            return View(db.Lectures.ToList());
        }

        // GET: Lectures/Details/5
        [ValidateInput(true)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lecture lecture = db.Lectures.Find(id);
            if (lecture == null)
            {
                return HttpNotFound();
            }
            return View(lecture);
        }

        // GET: Lectures/Create
        
        public ActionResult Create()
        {
            if (User.IsInRole("Tutor") || User.IsInRole("Administrator"))
            {
                return View();
            }
            else
            {
                TempData["lectureCreationFailedMsg"] = " Only tutors and administrators are allowed to create lectures! ";
                return RedirectToAction("Index");
            }
        }

        // POST: Lectures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lectureId,lectureName,description,date,grade,gradeNumber")] Lecture lecture)
        {
            int i = lecture.gradeNumber;
            if (i < 5)
            {
                return RedirectToAction("Error", "Lectures");
            }
            else { 
                if (ModelState.IsValid)
                {
                db.Lectures.Add(lecture);
                db.SaveChanges();
                return RedirectToAction("Index");
                }
                return View(lecture);
           
            }

            
        }

        public ActionResult Error() 
        {
            return View();
        
        }


        // GET: Lectures/Edit/5
        
        public ActionResult Edit(int? id)
        {
            if (User.IsInRole("Tutor") || User.IsInRole("Administrator"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Lecture lecture = db.Lectures.Find(id);
                if (lecture == null)
                {
                    return HttpNotFound();
                }
                return View(lecture);
            }
            else
            {
                TempData["lectureEditFailedMsg"] = " Only tutors and administrators are allowed to edit lectures! ";
                return RedirectToAction("Index");
            }

        }

        // POST: Lectures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lectureId,lectureName,description,date,grade,gradeNumber")] Lecture lecture)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lecture).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lecture);
        }

        // GET: Lectures/Delete/5
        //[Authorize(Roles = "Tutor")]
        public ActionResult Delete(int? id)
        {

            if (User.IsInRole("Tutor") || User.IsInRole("Administrator"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Lecture lecture = db.Lectures.Find(id);
                if (lecture == null)
                {
                    return HttpNotFound();
                }
                return View(lecture);
            }
            else
            {
                TempData["lectureDeleteFailedMsg"] = " Only tutors and administrators are allowed to delete lectures! ";
                return RedirectToAction("Index");
            }
           
        }

        // POST: Lectures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lecture lecture = db.Lectures.Find(id);
            db.Lectures.Remove(lecture);
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
