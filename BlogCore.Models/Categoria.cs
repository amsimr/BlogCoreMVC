using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="El nombre de la categoria es requerido")]
        [Display(Name ="Nombre Categoria")]
        public string Nombre { get; set; }

        [Display(Name = "Orden de Visualizacion")]
        public int? Orden { get; set; }


    }
}
