using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Cliente
    {

        public int IdCliente { get; set; }

        [Required(ErrorMessage ="La Categoría del Cliente es un campo requerido")]
        public string Categoria { get; set; }

        [Required(ErrorMessage = "El Nombre del Cliente es un campo requerido")]
        public string NombreCliente { get; set; }

        [Required(ErrorMessage = "El Apellido del Cliente es un campo requerido")]
        public string ApellidoCliente { get; set; }


    }
}