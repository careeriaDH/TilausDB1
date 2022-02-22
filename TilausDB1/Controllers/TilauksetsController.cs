using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TilausDB1.Models;
using TilausDB1.ViewModels;

namespace TilausDB1.Controllers
{
    public class TilauksetsController : Controller
    {
        private TilausDBEntities2 db = new TilausDBEntities2();

        // GET: Tilauksets
        public ActionResult Index()
        {
            var tilaukset = db.Tilaukset.Include(t => t.Asiakkaat).Include(t => t.Postitoimipaikat);
            return View(tilaukset.ToList());
        }

        // GET: Tilauksets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tilaukset tilaukset = db.Tilaukset.Find(id);
            if (tilaukset == null)
            {
                return HttpNotFound();
            }
            return View(tilaukset);
        }

        // GET: Tilauksets/Create
        public ActionResult Create()
        {
            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi");
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka");
            return View();
        }

        // POST: Tilauksets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TilausID,AsiakasID,Toimitusosoite,Postinumero,Tilauspvm,Toimituspvm")] Tilaukset tilaukset)
        {
            if (ModelState.IsValid)
            {
                db.Tilaukset.Add(tilaukset);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi", tilaukset.AsiakasID);
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka", tilaukset.Postinumero);
            return View(tilaukset);
        }

        // GET: Tilauksets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tilaukset tilaukset = db.Tilaukset.Find(id);
            if (tilaukset == null)
            {
                return HttpNotFound();
            }
            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi", tilaukset.AsiakasID);
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka", tilaukset.Postinumero);
            return View(tilaukset);
        }

        // POST: Tilauksets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TilausID,AsiakasID,Toimitusosoite,Postinumero,Tilauspvm,Toimituspvm")] Tilaukset tilaukset)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tilaukset).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi", tilaukset.AsiakasID);
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka", tilaukset.Postinumero);
            return View(tilaukset);
        }

        // GET: Tilauksets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tilaukset tilaukset = db.Tilaukset.Find(id);
            if (tilaukset == null)
            {
                return HttpNotFound();
            }
            return View(tilaukset);
        }

        // POST: Tilauksets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tilaukset tilaukset = db.Tilaukset.Find(id);
            db.Tilaukset.Remove(tilaukset);
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

        public ActionResult Tilaussummary()
        {
            var tilausSummary = from a in db.Asiakkaat
                                join t in db.Tilaukset on a.AsiakasID equals t.AsiakasID
                                //where-lause
                                //orderby-lause
                                select new TilausTietoData
                                {
                                    TilausID = t.TilausID,
                                    AsiakasID = (int)a.AsiakasID,
                                    Tilauspvm = (DateTime)t.Tilauspvm,
                                    Toimituspvm = (DateTime)t.Toimituspvm,
                                    Toimitusosoite = t.Toimitusosoite,
                                    Osoite = a.Osoite,
                                    Postinumero = a.Postinumero,
                                    Nimi = a.Nimi,
                                };

            return View(tilausSummary);
        }
    }
}
