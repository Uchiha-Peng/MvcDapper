using DapperORM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DapperORM.Controllers
{
    public class UserController : Controller
    {
        DbContext db = new DbContext();

        // GET: User/Index
        public ActionResult Index()
        {
            List<UserInfo> list = db.GetModels();
            return View(list);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(UserInfo userinfo)
        {
            if (ModelState.IsValid)
            {
                db.Add(userinfo);
                return RedirectToAction("Index");
            }
            return View(userinfo);
        }


        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInfo user = db.GetModel(id ?? 0);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(UserInfo user)
        {
            if (ModelState.IsValid)
            {
                db.Update(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }


        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInfo user = db.GetModel(id ?? 0);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            db.Del(id);
            return RedirectToAction("Index");
        }

    }
}