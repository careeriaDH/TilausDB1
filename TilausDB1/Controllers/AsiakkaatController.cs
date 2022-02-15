using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TilausDB1.Models;
using System.Data;
using System.Data.Entity;
using System.Net;

namespace TilausDB1.Controllers
{
    public class AsiakkaatController : Controller
    {
        // GET: Asiakkaat
        TilausDBEntities2 db = new TilausDBEntities2();

        public object Postinumero { get; private set; }
        

        public ActionResult Index()

        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Home");
            }    
            else
            {
                var model = db.Asiakkaat.Include(t => t.Postitoimipaikat).Include(t => t.Postitoimipaikat);
                return View(model.ToList());
            }

        }
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Asiakkaat asiakkaat = db.Asiakkaat.Find(id);
            if (asiakkaat == null) return HttpNotFound();
            ViewBag.Postinumero = new SelectList(db.Asiakkaat, "Postinumero", "Nimi", selectedValue: Postinumero);
            return View(asiakkaat);

        }
        [HttpPost]
        [ValidateAntiForgeryToken] //Katso https://go.microsoft.com/fwlink/?LinkId=317598
        public ActionResult Edit([Bind(Include = "AsiakasID,Nimi,Osoite,Postitoiminumero")] Asiakkaat asiakkaat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(asiakkaat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(asiakkaat);
        }
        public ActionResult Create()
        {
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postinumero");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Katso https://go.microsoft.com/fwlink/?LinkId=317598
        public ActionResult Create([Bind(Include = "Nimi,Osoite,Postinumero")] Asiakkaat asiakkaat)
        {
            if (ModelState.IsValid)
            {
                db.Asiakkaat.Add(asiakkaat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(asiakkaat);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Asiakkaat asiakkaat = db.Asiakkaat.Find(id);
            if (asiakkaat == null) return HttpNotFound();
            return View(asiakkaat);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Asiakkaat asiakkaat = db.Asiakkaat.Find(id);
            db.Asiakkaat.Remove(asiakkaat);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}