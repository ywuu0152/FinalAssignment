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

        //List<Lecture> lectures = new List<Lecture>();
        //List<int> grades = new List<int>();

        public Lecture[] lectures = new Lecture[5];
        public int[] grades = new int[5];
        public static Lecture temp = new Lecture();


        // GET: Lectures
        public ActionResult Index()
        {
            return View(db.Lectures.ToList());
        }


        public ActionResult Chart() 
        {
            int i = 0;
            foreach (Lecture item in db.Lectures.ToList())
            {
                double a = double.Parse(item.grade);
                // 100 points largest
                a = a * 20;
                grades[i] = (int)a;
                i++;
            }
            ViewBag.Chart = grades;
            return View();
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

        // From ActionResult Details
        public ActionResult Rate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lecture lecture = db.Lectures.Find(id);
            temp = lecture;
            if (lecture == null)
            {
                return HttpNotFound();
            }

            List<SelectListItem> selection = new List<SelectListItem>();
            selection.Add(new SelectListItem() { Text = "⭐", Value = "1", Selected = false });
            selection.Add(new SelectListItem() { Text = "⭐⭐", Value = "2", Selected = false });
            selection.Add(new SelectListItem() { Text = "⭐⭐⭐", Value = "3", Selected = true });
            selection.Add(new SelectListItem() { Text = "⭐⭐⭐⭐", Value = "4", Selected = false });
            selection.Add(new SelectListItem() { Text = "⭐⭐⭐⭐⭐", Value = "5", Selected = false });

            ViewBag.Select = selection;

            return View(lecture);
        }
        [HttpPost]
        public ActionResult Rate(FormCollection form)
        {
            string rate = form["Select"];
            int rateScore = 0;
            int.TryParse(rate, out rateScore);
            double dou = double.Parse(temp.grade);
            double finalScore = (dou * temp.gradeNumber + rateScore) / (temp.gradeNumber + 1);

            //Accurate to two decimal places
            temp.grade = String.Format("{0:F}", finalScore);
            temp.gradeNumber++;


            if (ModelState.IsValid)
            {
                db.Entry(temp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View(temp);
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
            Boolean a = true;
            foreach (Lecture lec in db.Lectures.ToList())
            {
                if (lec.date == lecture.date)
                {
                    a = false;
                }
            }

            if (a == true)
            {
                int i = lecture.gradeNumber;
                if (i == 0)
                {
                    return RedirectToAction("Error", "Lectures");
                }

                if (ModelState.IsValid)
                {
                    db.Lectures.Add(lecture);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(lecture);
            }
            else
            {
                return RedirectToAction("Error1", "Lectures");
            }
          
        }

        public ActionResult Error() 
        {
            return View();       
        }

        public ActionResult Error1()
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
