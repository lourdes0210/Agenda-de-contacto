using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;

namespace Agenda_de_contacto
{
    public class ConexionBD
    {
        OleDbConnection conexion;
        OleDbCommand comando;
        OleDbDataAdapter adaptador;

        string cadena;
        public ConexionBD()
        {
            cadena = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source= ContactosL.accdb" ;
        }

        public DataTable ObtenerContactosPorCategoria()
        {
            using (OleDbConnection conexion = new OleDbConnection(cadena))
            {
                using (OleDbCommand comando = new OleDbCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = "SELECT * FROM Contacto ORDER BY Categoria"; // Ordenar por categoría para una mejor visualización

                    using (OleDbDataAdapter adaptador = new OleDbDataAdapter(comando))
                    {
                        DataTable tabla = new DataTable();
                        adaptador.Fill(tabla);
                        return tabla;
                    }
                }
            }
        }

        // Método para listar todos los contactos
        public void listarContactos(DataGridView dgvcontactos)
        {
            try
            {
                conexion = new OleDbConnection(cadena);
                comando = new OleDbCommand();

                comando.Connection = conexion;
                comando.CommandType = CommandType.Text;
                comando.CommandText = "SELECT * FROM Contacto";// Selecciona todos los contactos.

                DataTable tablaContactos = new DataTable(); // Crea un DataTable para almacenar los datos.

                adaptador = new OleDbDataAdapter(comando);// Adaptador para llenar el DataTable
                adaptador.Fill(tablaContactos); // Llenar el DataTable con datos de la base de datos

                dgvcontactos.DataSource = tablaContactos; // Asignar el DataTable al DataGridView para mostrar los productos.
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void AgregarContactos(ContactosL nuevoContacto)
        {
            using (OleDbConnection conexion = new OleDbConnection(cadena))
            {
                using (OleDbCommand comando = new OleDbCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandType = CommandType.Text;
                    comando.CommandText = "INSERT INTO Contacto (Nombre, Apellido, Telefono,Correo, Categoria) " +
                               "VALUES (@Nombre, @Apellido, @Telefono, @Correo, @Categoria)";

                    comando.Parameters.AddWithValue("@Nombre", nuevoContacto.Nombre);
                    comando.Parameters.AddWithValue("@Apellido", nuevoContacto.Apellido);
                    comando.Parameters.AddWithValue("@Telefono", nuevoContacto.Telefono);
                    comando.Parameters.AddWithValue("@Correo", nuevoContacto.Correo);
                    comando.Parameters.AddWithValue("@Categoria", nuevoContacto.Categoria);

                    try
                    {
                        conexion.Open();
                        comando.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Manejar la excepción (por ejemplo, mostrar un mensaje de error al usuario)
                        MessageBox.Show("Error al agregar el contacto: " + ex.Message);
                    }
                }
            }
        }

        public void ModificarContacto(ContactosL contacto)
        {
            using (OleDbConnection conexion = new OleDbConnection(cadena))
            {
                using (OleDbCommand comando = new OleDbCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandType = CommandType.Text;
                    comando.CommandText
                         = @"UPDATE Contacto
                                 SET Nombre = @nuevoNombre,
                                     Apellido = @nuevoApellido,
                                     Telefono = @nuevoTelefono,
                                     Correo = @nuevoCorreo,
                                     Categoria = @nuevaCategoria
                                     WHERE Id = @id";

                    comando.Parameters.AddWithValue("@nuevoNombre", contacto.Nombre);
                    comando.Parameters.AddWithValue("@nuevoApellido", contacto.Apellido);
                    comando.Parameters.AddWithValue("@nuevoTelefono", contacto.Telefono);
                    comando.Parameters.AddWithValue("@nuevoCorreo", contacto.Correo);
                    comando.Parameters.AddWithValue("@nuevaCategoria", contacto.Categoria);
                    comando.Parameters.AddWithValue("@id", contacto.id);




                    try
                    {
                        conexion.Open();
                        int filasAfectadas = comando.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            MessageBox.Show("Contacto modificado correctamente.");
                        }
                        else
                        {
                            MessageBox.Show("No se encontró ningún producto con el ID especificado.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al modificar el producto: " + ex.Message);
                    }
                }
            }

        }

        public void EliminarContacto(int id)
        {
            using (OleDbConnection conexion = new OleDbConnection(cadena))
            {
                using (OleDbCommand comando = new OleDbCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandType = CommandType.Text;
                    comando.CommandText = @"DELETE FROM Contacto WHERE Id = @id";

                    // Agregar el parámetro
                    comando.Parameters.AddWithValue("@id", id);

                    try
                    {
                        conexion.Open();
                        int filasAfectadas = comando.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            MessageBox.Show("Contacto eliminado correctamente.");
                        }
                        else
                        {
                            MessageBox.Show("No se encontró ningún producto con el ID especificado.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar el producto: " + ex.Message);
                    }
                }
            }

        }
    }
}
