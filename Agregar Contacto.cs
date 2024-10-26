using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Agenda_de_contacto
{
    public partial class Agregar_Contacto : Form
    {
        public Agregar_Contacto()
        {
            InitializeComponent();
            ConexionBD conexion = new ConexionBD();
            CargarTreeView();
            conexion.listarContactos(dgvContactos);
        }

        Dictionary<int, TreeNode> nodosPorId = new Dictionary<int, TreeNode>();
        Dictionary<string, TreeNode> nodosPorCategoria = new Dictionary<string, TreeNode>();

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            ContactosL nuevoContacto = new ContactosL();
            ConexionBD conexion = new ConexionBD();

            nuevoContacto.id = int.Parse(txtid.Text);
            nuevoContacto.Nombre = txtNombre.Text;
            nuevoContacto.Apellido = txtApellido.Text;
            nuevoContacto.Telefono = int.Parse(txtTelefono.Text);
            nuevoContacto.Correo = txtCorreo.Text;
            nuevoContacto.Categoria = cmbCategorias.Text;

            try
            {
                CargarTreeView();
                conexion.AgregarContactos(nuevoContacto);
                conexion.listarContactos(dgvContactos);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el contacto: " + ex.Message);
            }
        }

        public void CargarTreeView()
        {
            ConexionBD conexion = new ConexionBD();

            // Limpiar el TreeView antes de volver a llenarlo
            treeView.Nodes.Clear();

            // Obtener los datos de los contactos desde la base de datos
            DataTable tablaContactos = conexion.ObtenerContactosPorCategoria();

            // Diccionario para almacenar los nodos de las categorías
            Dictionary<string, TreeNode> nodosPorCategoria = new Dictionary<string, TreeNode>();

            // Crear los nodos del TreeView
            foreach (DataRow row in tablaContactos.Rows)
            {
                string categoria = row["Categoria"].ToString();
                string nombre = row["Nombre"].ToString();
                string apellido = row["Apellido"].ToString();

                // Obtener o crear el nodo de la categoría
                TreeNode nodoCategoria;
                if (!nodosPorCategoria.TryGetValue(categoria, out nodoCategoria))
                {
                    nodoCategoria = new TreeNode(categoria);
                    treeView.Nodes.Add(nodoCategoria);
                    nodosPorCategoria[categoria] = nodoCategoria;
                }

                // Crear el nodo del contacto y agregarlo al nodo de la categoría
                TreeNode nodoContacto = new TreeNode($"{nombre}, {apellido}");
                nodoCategoria.Nodes.Add(nodoContacto);
            }

            // Expandir todos los nodos del TreeView (opcional)
            treeView.ExpandAll();
        }

        private void modificarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Modificar_Contacto frm = new Modificar_Contacto();
            frm.ShowDialog();
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Eliminar_Contacto frm = new Eliminar_Contacto();
            frm.ShowDialog();
        }
    }
}
