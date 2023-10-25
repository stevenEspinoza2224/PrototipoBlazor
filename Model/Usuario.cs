using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Correo { get; set; }
        public string? PasswordHash { get; set; }
        public string? Salt { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string? UsuarioCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; }
        public int Activo { get; set; }
    }
}
