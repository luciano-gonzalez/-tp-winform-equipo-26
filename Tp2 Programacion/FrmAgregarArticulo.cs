using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace Tp2_Programacion
{
    public partial class FrmAgregarArticulo : Form
    {
        private Articulo articulo= null;
        public FrmAgregarArticulo()
        {
            InitializeComponent();
        }
        public FrmAgregarArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
        }



        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            
            ArticulosNegocio articulosNegocio = new ArticulosNegocio();
            try
            {
                if(articulo == null)
                {
                    articulo = new Articulo();
                }
                articulo._codArticulo = txtCodigo.Text;
                articulo._nombre = txtNombre.Text;
                articulo._descripcion = txtDescripcion.Text;
                articulo._precio = float.Parse(txtPrecio.Text);
                articulo._marca = (Marca)cboMarca.SelectedItem;
                articulo._categoria = (Categoria)cboCategoria.SelectedItem;
                  

                if (articulo.ID != 0)
                {
                    articulosNegocio.Modificar(articulo);
                    MessageBox.Show("Modificado exitosamente");
                }
                else
                {
                    articulosNegocio.Agregar(articulo);
                    MessageBox.Show("Agregado exitosamente");
                }
                Close();
            }
            catch (FormatException ex)
            {
                MessageBox.Show("En la sección de precio,Solamente se permiten números, intente nuevamente. ");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            
        }

        private void FrmAgregarArticulo_Load(object sender, EventArgs e)
        {
            ArticulosNegocio articulosNegocio = new ArticulosNegocio();

            try
            {
                cboMarca.DataSource = articulosNegocio.listarMarcas();
                cboMarca.ValueMember = "_idMarca";
                cboMarca.DisplayMember = "_nombre";
                cboCategoria.DataSource = articulosNegocio.listarCategorias();
                cboCategoria.ValueMember = "_idCategoria";
                cboCategoria.DisplayMember = "_descripcion";
               

                if (articulo != null )
                {
                    txtCodigo.Text = articulo._codArticulo;
                    txtNombre.Text = articulo._nombre;
                    txtDescripcion.Text = articulo._descripcion;
                    txtPrecio.Text = articulo._precio.ToString();
                    cboMarca.SelectedValue = articulo._marca._idMarca;
                    cboCategoria.SelectedValue = articulo._categoria._idCategoria;
                   

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void cboMarca_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
    }
}
