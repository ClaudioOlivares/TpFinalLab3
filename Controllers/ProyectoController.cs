using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TpFinalLab3.Models;

namespace TpFinalLab3.Controllers
{
    public class ProyectoController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment enviroment;
        private readonly RepositorioProyecto repoProyecto;

        public ProyectoController(IConfiguration configuration, IHostingEnvironment enviroment)
        {
            this.configuration = configuration;
            this.enviroment = enviroment;
            repoProyecto = new RepositorioProyecto(configuration);
        }
        // GET: Proyecto
        public ActionResult Index()
        {
            return View();
        }

        // GET: Proyecto/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Proyecto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Proyecto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Proyecto p)
        {
            try
            {
                p.Portada = "n";
                int res = repoProyecto.Alta(p);
                // TODO: Add insert logic here
                if (p.PortadaFile != null)
                {
                    string wwwpath = enviroment.WebRootPath;
                    string path = Path.Combine(wwwpath, "uploads");
                    if(!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName =  "portada_" + res + Path.GetExtension(p.PortadaFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    p.Portada = Path.Combine("uploads", fileName);
                    using (FileStream stream = new FileStream (pathCompleto, FileMode.Create))
                    {
                        p.PortadaFile.CopyTo(stream);
                    }
                    p.IdProyecto = res;
                    repoProyecto.Modificacion(p);

                }
               
                return RedirectToAction(nameof(Index));
            }
            catch(System.Exception ex)
            {
                return View();
            }
        }

        // GET: Proyecto/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Proyecto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Proyecto/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Proyecto/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}