using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticulosController : Controller
    {

        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly ApplicationDbContext _context;

        public ArticulosController(IContenedorTrabajo contenedorTrabajo, ApplicationDbContext context)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _context = context;
        }



        public IActionResult Index()
        {
            return View();
        }



        public IActionResult Create()
        {
            return View();
        }




        #region
        [HttpGet]
        public IActionResult GetAll()
        {
            // Opcion 1
            return Json(new { data = _contenedorTrabajo.Articulo.GetAll() });
        }

        #endregion

    }
}
