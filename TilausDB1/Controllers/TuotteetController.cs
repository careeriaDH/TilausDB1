using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TilausDB1.Models;
using PagedList;

namespace TilausDB1.Controllers
{
    public class TuotteetController : Controller
    {
        // GET: Tuotteet
        TilausDBEntities2 db = new TilausDBEntities2();

        public ActionResult Index(string sortOrder, string currentFilter1, string searchString1, string ProductCategory, string currentProductCategory, int? page, int? pagesize)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.ProductNameSortParm = String.IsNullOrEmpty(sortOrder) ? "productname_desc" : "";
            ViewBag.UnitPriceSortParm = sortOrder == "UnitPrice" ? "unitPrice_desc" : "UnitPrice";

            if(searchString1 != null)
            {
                page = 1;
            }
            else
            {
                searchString1 = currentFilter1;
            }

            ViewBag.currentFilter1 = searchString1;

            if ((ProductCategory != null) && (ProductCategory != "0"))
            {
                page = 1;
            }
            else
            {
                ProductCategory = currentProductCategory;
            }
            ViewBag.currentProductCategory = ProductCategory;


            TilausDBEntities2 db = new TilausDBEntities2();
            
            var tuotteet = from t in db.Tuotteet
                           select t;

            if (!String.IsNullOrEmpty(searchString1)) //Jos hakufiltteri on käynnissä, niin käytetään sitä ja sen lisäksi lajitellaan tulokset
            {
                switch (sortOrder)
                {
                    case "productname_desc":
                        tuotteet = tuotteet.Where(t => t.Nimi.Contains(searchString1)).OrderByDescending(t => t.Nimi);
                        break;
                    case "UnitPrice":
                        tuotteet = tuotteet.Where(t => t.Nimi.Contains(searchString1)).OrderBy(t => t.Ahinta);
                        break;
                    case "UnitPrice_desc":
                        tuotteet = tuotteet.Where(t => t.Nimi.Contains(searchString1)).OrderByDescending(t => t.Ahinta);
                        break;
                    default:
                        tuotteet = tuotteet.Where(t => t.Nimi.Contains(searchString1)).OrderBy(t => t.Nimi);
                        break;
                }
            }
            else if (!String.IsNullOrEmpty(ProductCategory) && (ProductCategory != "0"))//Jos käytössä on tuoteryhmärajaus, niin käytetään sitä ja sen lisäksi lajitellaan tulokset
            {
                int para = int.Parse(ProductCategory);
                switch (sortOrder)
                {
                    case "productname_desc":
                        tuotteet = tuotteet.Where(t => t.Kategoria == para).OrderByDescending(t => t.Nimi);
                        break;
                    case "UnitPrice":
                        tuotteet = tuotteet.Where(t => t.Kategoria == para).OrderBy(t => t.Ahinta);
                        break;
                    case "UnitPrice_desc":
                        tuotteet = tuotteet.Where(t => t.Kategoria == para).OrderByDescending(t => t.Ahinta);
                        break;
                    default:
                        tuotteet = tuotteet.Where(t => t.Kategoria == para).OrderBy(t => t.Nimi);
                        break;
                }
            } else{ //Hakufiltteri ei ole käynnissä, joten lajitellaan ilman suodatuksia
                switch (sortOrder)
                {
                    case "productname_desc":
                        tuotteet = tuotteet.OrderByDescending(t => t.Nimi);
                        break;
                    case "UnitPrice":
                        tuotteet = tuotteet.OrderBy(t => t.Ahinta);
                        break;
                    case "UnitPrice_desc":
                        tuotteet = tuotteet.OrderByDescending(t => t.Ahinta);
                        break;
                    default:
                        tuotteet = tuotteet.OrderBy(t => t.Nimi);
                        break;
                }
            };

            List<Kategoriat> lstKategoriat = new List<Kategoriat>();

            var kategoriaList = from kat in db.Kategoriat
                               select kat;

            Kategoriat tyhjaKategoria = new Kategoriat();
            tyhjaKategoria.KategoriaId = 0;
            tyhjaKategoria.KategoriaNimi = "";
            tyhjaKategoria.KategoriaIDKategoriaNimi = "";
            lstKategoriat.Add(tyhjaKategoria);

            foreach (Kategoriat kategoria in kategoriaList)
            {
                Kategoriat yksiKategoria = new Kategoriat();
                yksiKategoria.KategoriaId = kategoria.KategoriaId;
                yksiKategoria.KategoriaNimi = kategoria.KategoriaNimi;
                yksiKategoria.KategoriaIDKategoriaNimi = kategoria.KategoriaId.ToString() + " - " + kategoria.KategoriaNimi;
                lstKategoriat.Add(yksiKategoria);
            }
            ViewBag.CategoryID = new SelectList(lstKategoriat, "KategoriaId", "KategoriaIDKategoriaNimi", ProductCategory);

            int pageSize = (pagesize ?? 10);
            int pageNumber = (page ?? 1);
            return View(tuotteet.ToPagedList(pageNumber, pageSize));
        }


        //public ActionResult Index3()
        //{
        //    if (Session["Username"] == null)
        //    {
        //        return RedirectToAction("login", "home");
        //    }
        //    else
        //    {
        //        TilausDBEntities2 db = new TilausDBEntities2();
        //        List<Tuotteet> model = db.Tuotteet.ToList();
        //        return View(model);
        //    }
        //}

        public ActionResult Index2()
        {
                TilausDBEntities2 db = new TilausDBEntities2();
                List<Tuotteet> model = db.Tuotteet.ToList();

                return View(model);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Tuotteet tuotteet = db.Tuotteet.Find(id);
            if (tuotteet == null) return HttpNotFound();
            return View(tuotteet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Katso https://go.microsoft.com/fwlink/?LinkId=317598
        public ActionResult Edit([Bind(Include = "TuoteID,Nimi,Ahinta,Kuva")] Tuotteet tuotteet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tuotteet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tuotteet);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Katso https://go.microsoft.com/fwlink/?LinkId=317598
        public ActionResult Create([Bind(Include = "TuoteID,Nimi,Ahinta,")] Tuotteet tuotteet)
        {
            if (ModelState.IsValid)
            {
                db.Tuotteet.Add(tuotteet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tuotteet);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Tuotteet tuotteet = db.Tuotteet.Find(id);
            if (tuotteet == null) return HttpNotFound();
            return View(tuotteet);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tuotteet tuotteet = db.Tuotteet.Find(id);
            db.Tuotteet.Remove(tuotteet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}