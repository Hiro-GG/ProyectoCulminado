using System;
using System.Data;
using System.Web;
using System.Web.UI;

namespace wssProyecto.CLIENTE
{
    public partial class Formulario_web1 : System.Web.UI.Page
    {
        clsCarrito objCarrito = new clsCarrito();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Valor por defecto
                string soloNombre = "Cliente";

                // Usamos la variable de sesión que inicializas en Global.asax
                // Session["nombreUsuario"]
                if (Session["nombreUsuario"] != null)
                {
                    string nombreCompleto = Session["nombreUsuario"].ToString();
                    soloNombre = ObtenerSoloNombre(nombreCompleto);
                }
                lblNombreCliente.Text = soloNombre;
                if (Session["cveUsuario"] != null)
                {
                    CargarResumenUsuario(int.Parse(Session["cveUsuario"].ToString()));
                }
            }
        }
        private void CargarResumenUsuario(int idUsuario)
        {
            try
            {
                // Llamamos al método que creamos en clsCarrito
                DataSet ds = objCarrito.obtenerResumenCliente(
                    Application["cnnVentas"].ToString(),
                    idUsuario
                );

                if (ds.Tables["Resumen"].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables["Resumen"].Rows[0];

                    // Asignamos los valores a los Labels nuevos
                    lblReservasActivas.Text = dr["Activas"].ToString();
                    lblUltimoViaje.Text = dr["UltimoViaje"].ToString();
                }
            }
            catch (Exception)
            {
                // En caso de error, dejamos valores por defecto
                lblReservasActivas.Text = "0";
                lblUltimoViaje.Text = "N/A";
            }
        }

        /// <summary>
        /// Toma un nombre completo y regresa solo la primera palabra (el nombre).
        /// Ejemplo: "Carlos Ramírez López" → "Carlos"
        /// </summary>
        private string ObtenerSoloNombre(string nombreCompleto)
        {
            if (string.IsNullOrWhiteSpace(nombreCompleto))
                return "Cliente";

            string[] partes = nombreCompleto
                                .Trim()
                                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (partes.Length == 0)
                return "Cliente";

            return partes[0];
        }
    }
}
