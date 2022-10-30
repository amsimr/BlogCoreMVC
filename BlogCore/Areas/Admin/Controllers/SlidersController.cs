using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlidersController : Controller
    {

        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public SlidersController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment hostEnvironment)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _hostingEnvironment = hostEnvironment;
        }



        // Metodo Index

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }




        // Metodo Crear

        [HttpGet]
        public IActionResult Create()
        {
          
            return View();
        }



        // Subida de imagen del slider

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Slider slider)
        {        
                
                if (ModelState.IsValid)
                {
                    string rutaPrincipal = _hostingEnvironment.WebRootPath;
                    var archivos = HttpContext.Request.Form.Files;

                    // Nuevo slider
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\sliders");
                    var extension = Path.GetExtension(archivos[0].FileName);

                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    slider.UrlImagen = @"\imagenes\sliders\" + nombreArchivo + extension;

                    _contenedorTrabajo.Slider.Add(slider);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));
                }
            

            return View();
        }




        // Metodo para Editar

        [HttpGet]
        public IActionResult Edit(int? id)
        {         

            if (id != null)
            {
                var slider = _contenedorTrabajo.Slider.Get(id.GetValueOrDefault());
            }

            return View();
        }





        // Editar articulo y imagen
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Slider slider)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;


                var sliderDesdeDb = _contenedorTrabajo.Slider.Get(slider.Id);



                if (archivos.Count() > 0)
                {
                    // Nuevo imagen para el slider
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\sliders");
                    var extension = Path.GetExtension(archivos[0].FileName);
                    var nuevaExtension = Path.GetExtension(archivos[0].FileName);


                    var rutaImagen = Path.Combine(rutaPrincipal, sliderDesdeDb.UrlImagen.TrimStart('\\'));
                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }

                    // Otra vez se sube la imagen

                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    slider.UrlImagen = @"\imagenes\sliders\" + nombreArchivo + extension;

                    _contenedorTrabajo.Slider.Update(slider);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Aqui cuando la imagen ya existe y se conserva
                    slider.UrlImagen = sliderDesdeDb.UrlImagen;
                }

                _contenedorTrabajo.Slider.Update(slider);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));

            }

            return View();
        }




        #region
        [HttpGet]
        public IActionResult GetAll()
        {
            // Opcion 1
            return Json(new { data = _contenedorTrabajo.Slider.GetAll() });
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {

            var sliderDesdeDb = _contenedorTrabajo.Slider.Get(id);
           
            if (sliderDesdeDb == null)
            {
                return Json(new { success = false, message = "Error borrando slider" });

            }
           
            _contenedorTrabajo.Slider.Remove(sliderDesdeDb);
            _contenedorTrabajo.Save();
            return Json(new { success = true, message = "Slider borrado correctamente" });
        }



        #endregion

    }
}
