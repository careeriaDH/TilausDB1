using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TilausDB1.Models;
using System.Data;
using System.Data.Entity;

namespace TilausDB1.Controllers
{ 
public class TilauksetController : Controller
    {
        // GET: Tilaukset
        TilausDBEntities2 db = new TilausDBEntities2();


        public ActionResult Index()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("login", "home");
            }
            else
            {
                var model = db.Tilaukset.Include(t => t.Asiakkaat).Include(t => t.Postitoimipaikat);
                return View(model.ToList());
            }
        }
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var model = db.Tilaukset.Include(t => t.Asiakkaat).Include(t => t.Postitoimipaikat);
            Tilaukset tilaukset = db.Tilaukset.Find(id);
            if (tilaukset == null) return HttpNotFound();
            ViewBag.AsiakasID = new SelectList(db.Tilaukset, "AsiakasID", "Postinumero", selectedValue: "AsiakasID");
            ViewBag.Postinumero = new SelectList(db.Tilaukset, "AsiakasID", "Postinumero", selectedValue: "Postinumero");
            return View(tilaukset);
        }
        [HttpPost]
        [ValidateAntiForgeryToken] //Katso https://go.microsoft.com/fwlink/?LinkId=317598
        public ActionResult Edit([Bind(Include = "TilausID,AsiakasID,Toimitusosoite,Tilauspvm,Toimituspvm,Nimi,Postitoimipaikka")] Tilaukset tilaukset)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tilaukset).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tilaukset);
        }
        public ActionResult Create()
        {
            ViewBag.AsiakasID = new SelectList(db.Asiakkaat,  "AsiakasID", "Nimi");
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postinumero");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Katso https://go.microsoft.com/fwlink/?LinkId=317598
        public ActionResult Create([Bind(Include = "Toimitusosoite,Tilauspvm,Toimituspvm,Nimi,Postinumero,AsiakasID")] Tilaukset tilaukset)
        {
            if (ModelState.IsValid)
            {
                db.Tilaukset.Add(tilaukset);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi", tilaukset.AsiakasID);
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postinumero", tilaukset.Postinumero);
            return View(tilaukset);
        }

    }
}