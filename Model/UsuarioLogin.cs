using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class UsuarioLogin
    {
        public string? NombreUsuario { get; set; }

        public string? Correo { get; set; }

        [Required(ErrorMessage ="La contraseña es Obligatoria")]
        public string? Password { get; set; }
    }
    public class UsuarioRegistro
    {
        [Required(ErrorMessage = "El Usuario es Obligatorio")]
        public string? NombreUsuario { get; set; }

        [Required(ErrorMessage = "El Correo es Obligatorio")]
        public string? Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es Obligatoria")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "El Rol del Usuario es Obligatorio")]
        public string? Rol { get; set; }
    }
    public class UsuarioLoginRespuesta
    {
        public Usuario? Usuario { get; set; }
        public string? UserToken { get; set; }
    }

}
