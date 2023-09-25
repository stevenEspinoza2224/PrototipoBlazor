using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using APICORE.Models;
using Model;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace APICORE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {

        private readonly string cadenaSQl;

        public ClienteController(IConfiguration config)
        {
            cadenaSQl = config.GetConnectionString("CadenaSQL");

        }

        [HttpGet]
        [Route("Lista")]

        public IActionResult Lista()
        {
            List<Cliente> lista = new();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQl))
                {
                    lista = conexion.Query<Cliente>(
                        sql: "select * from Cliente WHERE Activo = 1",
                        commandType: CommandType.Text).ToList();

                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Ok", response = lista });

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, respuesta = lista });
            }


        }



        [HttpGet]
        [Route("Obtener/{IdCliente:int}")]

        public IActionResult Obtener([FromRoute] int IdCliente)
        {
            Cliente c = new();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQl))
                {
                    c = conexion.Query<Cliente>(sql: "select * from Cliente",
                        commandType: CommandType.Text)
                        .FirstOrDefault(x => x.IdCliente == IdCliente);
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Ok", response = c });

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, respuesta = c });
            }

        }


        [HttpPost]
        [Route("Guardar")]

        public IActionResult Guardar([FromBody] Cliente c)
        {

            try
            {
                using (var conexion = new SqlConnection(cadenaSQl))
                {
                    conexion.Execute(
                        sql: "Insert into Cliente(Categoria, nombreCliente, apellidoCliente) values(@Param1, @Param2, @Param3)",
                        param: new
                        {
                            Param1 = c.Categoria,
                            Param2 = c.NombreCliente,
                            Param3 = c.ApellidoCliente
                        },
                        commandType: CommandType.Text);
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Ok" });

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }


        }

        [HttpDelete]
        [Route("Eliminar/{IdCliente:int}")]

        public IActionResult Eliminar([FromRoute] int IdCliente)
        {

            try
            {
                using (var conexion = new SqlConnection(cadenaSQl))
                {
                    conexion.Execute(sql: "UPDATE Cliente SET Activo = 0 WHERE IdCliente = @Param1",
                        param: new
                        {
                            Param1 = IdCliente
                        },
                        commandType: CommandType.Text);

                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Eliminado" });

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }


        }



        [HttpPut]
        [Route("Editar")]

        public IActionResult Editar([FromBody] Cliente c)
        {

            try
            {
                using (var conexion = new SqlConnection(cadenaSQl))
                {
                    conexion.Execute(sql: "UPDATE Cliente set Categoria = @Param1, nombreCliente = @Param2, apellidoCliente = @param3 where IdCliente = @Param4",
                        param: new
                        {
                            Param1 = c.Categoria,
                            Param2 = c.NombreCliente,
                            Param3 = c.ApellidoCliente,
                            Param4 = c.IdCliente
                        },
                        commandType: CommandType.Text);

                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Editado" });

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }


        }

    }
}
