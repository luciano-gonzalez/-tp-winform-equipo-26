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
    public partial class FrmVerDetalles : Form
    {
        public Articulo articulo { get; set; }
        public FrmVerDetalles()
        {
            InitializeComponent();
        }

        public FrmVerDetalles(Articulo art)
        {
            articulo = art;
            InitializeComponent();
        }


        private void FrmVerDetalles_Load(object sender, EventArgs e)
        {
            txtid.Text = articulo.ID.ToString();
            txtcodigo.Text = articulo._codArticulo.ToString();
            txtnombre.Text = articulo._nombre.ToString();
            txtdescripcion.Text = articulo._descripcion.ToString();
            txtmarca.Text = articulo._marca.ToString();
            txtcategoria.Text = articulo._categoria.ToString();
            txtprecio.Text = articulo._precio.ToString();

        }

       
    }
}
