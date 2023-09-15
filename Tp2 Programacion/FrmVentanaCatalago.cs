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
    public partial class FrmVentanaCatalago : Form
    {
        private Dictionary<int, List<string>> imagenesPorArticulo; // Diccionario para almacenar URLs de imágenes por ID de artículo.
        private int idArticuloSeleccionado = -1; // ID del artículo seleccionado actualmente.
        
        private List<Articulo> listaArticulos;
        public FrmVentanaCatalago()
        {
            InitializeComponent();
        }


        private void FrmVentanaCatalago_Load(object sender, EventArgs e)
        {
            cargar();
            cboCampo.Items.Add("ID");
            cboCampo.Items.Add("Nombre");
            

            // Inicializa el diccionario de imágenes.
            imagenesPorArticulo = new Dictionary<int, List<string>>();

            // Llena el diccionario con las URLs de imágenes por ID de artículo.
            foreach (Articulo articulo in listaArticulos)
            {
                if (!imagenesPorArticulo.ContainsKey(articulo.ID))
                {
                    imagenesPorArticulo.Add(articulo.ID, new List<string>());
                }
                imagenesPorArticulo[articulo.ID].Add(articulo._UrlImagen);
            }

            // Muestra la imagen del primer artículo (si hay al menos un artículo).
            if (listaArticulos.Count > 0)
            {
                idArticuloSeleccionado = listaArticulos[0].ID;
                MostrarImagenesArticuloSeleccionado();
            }


        }
        private void DgbArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if (DgbArticulos.CurrentRow != null && DgbArticulos.CurrentRow.DataBoundItem != null)
            {
                Articulo seleccionado = (Articulo)DgbArticulos.CurrentRow.DataBoundItem;
                idArticuloSeleccionado = seleccionado.ID;
                MostrarImagenesArticuloSeleccionado();
            }
        }
        private void MostrarImagenesArticuloSeleccionado()
        {
            Articulo seleccionado = (Articulo)DgbArticulos.CurrentRow.DataBoundItem;
            if (seleccionado != null)
            {
                pbxArticulo.Load(seleccionado._UrlImagen);
            }
            else
            {
                pbxArticulo.Load("https://www.pngkey.com/png/detail/233-2332677_ega-png.png");// Manejar el caso donde seleccionado es null, por ejemplo, mostrar una imagen predeterminada.
            }
        }


        private bool validarfiltro()
        {
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("por favor,seleccione el campo para filtrar.");
                return true;
            }
            if (cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("por favor,seleccione el criterio para filtrar.");
                return true;
            }
            if (cboCampo.SelectedItem.ToString()=="ID")
            {
                if (string.IsNullOrEmpty(txtfiltroavanzado.Text))
                {
                    MessageBox.Show("Debes cargar el filtro para numéricos...");
                    return true;
                }
                if (!(solonumeros(txtfiltroavanzado.Text)))
                {
                    MessageBox.Show("solo numeros para filtrar por un campo numerico.");
                    return true;

                }
            }
            
            return false;
        }

        private bool solonumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                    return false;
            }
            return true;
        }

        private void cargar()
        {
            ArticulosNegocio datos = new ArticulosNegocio();

            listaArticulos = datos.listar(); ;
            DgbArticulos.DataSource = listaArticulos ;
            DgbArticulos.Columns["_codArticulo"].Visible = false;
            DgbArticulos.Columns["_descripcion"].Visible = false;
            DgbArticulos.Columns["_categoria"].Visible = false;
            DgbArticulos.Columns["_marca"].Visible = false;
            DgbArticulos.Columns["_precio"].Visible = false;
            DgbArticulos.Columns["_Urlimagen"].Visible = false;
            Text = "Modificar Articulo";


        }

        private void ocultarcolumnas()
        {
            DgbArticulos.Columns["_codArticulo"].Visible = false;
            DgbArticulos.Columns["_descripcion"].Visible = false;
            DgbArticulos.Columns["_categoria"].Visible = false;
            DgbArticulos.Columns["_marca"].Visible = false;
            DgbArticulos.Columns["_precio"].Visible = false;
            DgbArticulos.Columns["_Urlimagen"].Visible = false;

        }

        private void agregarArticuloToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAgregarArticulo ventana = new FrmAgregarArticulo();
            ventana.ShowDialog();
            cargar();
        }

        private void TsmModificar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado;
            seleccionado = (Articulo)DgbArticulos.CurrentRow.DataBoundItem;
            FrmAgregarArticulo modificar = new FrmAgregarArticulo(seleccionado);
            modificar.ShowDialog();
            cargar();

        }

        private void TsmEliminar_Click(object sender, EventArgs e)
        {
            ArticulosNegocio negocio = new ArticulosNegocio();
            Articulo seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("Desea eliminar el articulo definitivamente?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning );
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Articulo)DgbArticulos.CurrentRow.DataBoundItem;
                    negocio.Eliminar(seleccionado.ID);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            cargar();
        }

        private void btnDetalles_Click(object sender, EventArgs e)
        {
           try
            {
                

                if (txtfiltro.Text == " ")
                {
                    MessageBox.Show("la celda se encuentra vacia, intente nuevamente.");
                }
                else
                {
                    Articulo articulo;
                    articulo = (Articulo)DgbArticulos.CurrentRow.DataBoundItem;
                    FrmVerDetalles ventanaDetalles = new FrmVerDetalles(articulo);
                    ventanaDetalles.ShowDialog();
                }
                
                
                
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.ToString());
               
            }
        }

            

        private void txtfiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listafiltrada;
            string filtro = txtfiltro.Text;
            if (filtro != " ")
            {
                listafiltrada = listaArticulos.FindAll(x => x._nombre.ToUpper().Contains(txtfiltro.Text.ToUpper()));
            }
            else
            {
                listafiltrada = listaArticulos;
            }


            DgbArticulos.DataSource = null;
            DgbArticulos.DataSource = listafiltrada;
            ocultarcolumnas();
            
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();
            if (opcion == "ID")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
            }
            else 
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ArticulosNegocio negocio = new ArticulosNegocio();
            try
            {
                if (validarfiltro()) 
                    return;
                
                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtfiltroavanzado.Text;
                DgbArticulos.DataSource = negocio.filtrar(campo, criterio, filtro);
                
                

            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.ToString());
                
            }
            

        }

        

        /*private void DgbArticulos_SelectionChanged(object sender, EventArgs e)
        {
            Articulo seleccionado=(Articulo)DgbArticulos.CurrentRow.DataBoundItem;
            pbxArticulo.Load(seleccionado._UrlImagen);
        }*/


        private void imageload(string imagen)
        {
            try
            {
                pbxArticulo.Load(imagen);
            }
            catch (Exception ex)
            {

                pbxArticulo.Load("https://www.pngkey.com/png/detail/233-2332677_ega-png.png");
            }
        }

        private void btnsiguiente_Click(object sender, EventArgs e)
        {
            if (imagenesPorArticulo.ContainsKey(idArticuloSeleccionado))
            {
                List<string> imagenes = imagenesPorArticulo[idArticuloSeleccionado];
                if (imagenes.Count > 0)
                {
                    // Asegúrate de que el índice de la imagen actual esté dentro del rango.
                    int indiceSiguiente = (imagenes.IndexOf(pbxArticulo.ImageLocation) + 1) % imagenes.Count;
                    pbxArticulo.Load(imagenes[indiceSiguiente]);
                }
            }
        }

        private void btnanterior_Click(object sender, EventArgs e)
        {
            if (imagenesPorArticulo.ContainsKey(idArticuloSeleccionado))
            {
                List<string> imagenes = imagenesPorArticulo[idArticuloSeleccionado];
                if (imagenes.Count > 0)
                {
                    // Asegúrate de que el índice de la imagen actual esté dentro del rango.
                    int indiceAnterior = (imagenes.IndexOf(pbxArticulo.ImageLocation) - 1 + imagenes.Count) % imagenes.Count;
                    pbxArticulo.Load(imagenes[indiceAnterior]);
                }
            }
        }
    }
}
