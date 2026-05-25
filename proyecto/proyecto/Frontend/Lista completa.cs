using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace proyecto
{
    public partial class Lista_completa : Form
    {
        private ComboBox cmbOrdenar;
        private TextBox txtBuscar;
        private Button btnBuscar;
        private Button btnLimpiar;
        private Label lblBuscar;

        // Panel y grid para mostrar resultados de búsqueda
        private Panel panelResultadoBusqueda;
        private DataGridView dgvResultados;
        private Label lblResultadoTitulo;

        public Lista_completa()
        {
            InitializeComponent();
            // Aplicar tema moderno limpio
            this.panel2.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
            // Botones de acción: estilo moderno
            EstilizarBoton(btnActualizar, System.Drawing.Color.FromArgb(59, 130, 246));
            EstilizarBoton(btnSalir, System.Drawing.Color.FromArgb(220, 53, 69));
            CrearPanelFiltros();
            CrearPanelResultados();
            this.Load += (s, e) =>
            {
                AjustarLayoutTabControl();
                CargarTabsPorCarrera();
            };
            // Reajustar si el usuario redimensiona la ventana
            this.Resize += (s, e) => AjustarLayoutTabControl();
        }

        private void EstilizarBoton(System.Windows.Forms.Button btn, System.Drawing.Color color)
        {
            btn.BackColor = color;
            btn.ForeColor = System.Drawing.Color.White;
            btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            btn.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        // ─────────────────────────────────────────────────────────────
        //  BARRA SUPERIOR: Ordenar + Búsqueda
        // ─────────────────────────────────────────────────────────────

        private void CrearPanelFiltros()
        {
            Panel panelFiltros = new Panel();
            panelFiltros.Height = 60;
            panelFiltros.Dock = DockStyle.Top;
            panelFiltros.BackColor = Color.FromArgb(250, 250, 250);
            this.panel2.Controls.Add(panelFiltros);
            panelFiltros.BringToFront();

            // ── Búsqueda ────────────────────────────────────────────
            lblBuscar = new Label();
            lblBuscar.Text = "🔍  Buscar alumno:";
            lblBuscar.Location = new Point(14, 20);
            lblBuscar.AutoSize = true;
            lblBuscar.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblBuscar.ForeColor = Color.DarkSlateBlue;
            panelFiltros.Controls.Add(lblBuscar);

            txtBuscar = new TextBox();
            txtBuscar.Location = new Point(135, 18);
            txtBuscar.Width = 180;
            txtBuscar.Font = new Font("Segoe UI", 9.5f);
            
            string placeholderText = "Núm. control  o  nombre...";
            txtBuscar.Text = placeholderText;
            txtBuscar.ForeColor = Color.Gray;
            txtBuscar.GotFocus += (s, e) =>
            {
                if (txtBuscar.Text == placeholderText)
                {
                    txtBuscar.Text = "";
                    txtBuscar.ForeColor = Color.Black;
                }
            };
            txtBuscar.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtBuscar.Text))
                {
                    txtBuscar.Text = placeholderText;
                    txtBuscar.ForeColor = Color.Gray;
                }
            };
            txtBuscar.TextChanged += TxtBuscar_TextChanged;
            txtBuscar.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) EjecutarBusqueda(); };
            panelFiltros.Controls.Add(txtBuscar);

            btnBuscar = new Button();
            btnBuscar.Text = "Buscar";
            btnBuscar.Location = new Point(325, 16);
            btnBuscar.Size = new Size(72, 26);
            btnBuscar.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            btnBuscar.BackColor = Color.SteelBlue;
            btnBuscar.ForeColor = Color.White;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.Cursor = Cursors.Hand;
            btnBuscar.Click += (s, e) => EjecutarBusqueda();
            panelFiltros.Controls.Add(btnBuscar);

            btnLimpiar = new Button();
            btnLimpiar.Text = "✕ Limpiar";
            btnLimpiar.Location = new Point(405, 16);
            btnLimpiar.Size = new Size(80, 26);
            btnLimpiar.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            btnLimpiar.BackColor = Color.Tomato;
            btnLimpiar.ForeColor = Color.White;
            btnLimpiar.FlatStyle = FlatStyle.Flat;
            btnLimpiar.FlatAppearance.BorderSize = 0;
            btnLimpiar.Cursor = Cursors.Hand;
            btnLimpiar.Visible = false;
            btnLimpiar.Click += BtnLimpiar_Click;
            panelFiltros.Controls.Add(btnLimpiar);

            // ── Ordenar ────────────────────────────────────────────
            Label lblOrdenar = new Label();
            lblOrdenar.Text = "Ordenar por:";
            lblOrdenar.Location = new Point(510, 20);
            lblOrdenar.AutoSize = true;
            lblOrdenar.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblOrdenar.ForeColor = Color.DarkSlateBlue;
            panelFiltros.Controls.Add(lblOrdenar);

            cmbOrdenar = new ComboBox();
            cmbOrdenar.Location = new Point(600, 18);
            cmbOrdenar.Width = 150;
            cmbOrdenar.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbOrdenar.Items.Add("Nombre (A-Z)");
            cmbOrdenar.Items.Add("Número de Control");
            cmbOrdenar.Items.Add("Semestre y Grupo");
            cmbOrdenar.Items.Add("Fecha de Creación");
            cmbOrdenar.SelectedIndex = 0;
            cmbOrdenar.SelectedIndexChanged += (s, e) => CargarTabsPorCarrera();
            panelFiltros.Controls.Add(cmbOrdenar);
        }

        private void AjustarLayoutTabControl()
        {
            int tabY    = 68; // Debajo del panelFiltros (60px) + margen
            int tabX    = 8;
            int tabW    = panel2.Width - 16;
            int botY    = btnActualizar.Top;
            int tabH    = botY - tabY - 6;
            if (tabH < 80) tabH = 80;

            tabControl1.Location            = new Point(tabX, tabY);
            tabControl1.Size                = new Size(tabW, tabH);
            panelResultadoBusqueda.Location = new Point(tabX, tabY);
            panelResultadoBusqueda.Size     = new Size(tabW, tabH);
        }

        // ─────────────────────────────────────────────────────────────
        //  PANEL DE RESULTADOS DE BÚSQUEDA
        // ─────────────────────────────────────────────────────────────

        private void CrearPanelResultados()
        {
            // Panel que cubre el tabControl cuando hay una búsqueda activa
            panelResultadoBusqueda = new Panel();
            panelResultadoBusqueda.BackColor = Color.FromArgb(250, 250, 250);
            panelResultadoBusqueda.Location = this.tabControl1.Location;
            panelResultadoBusqueda.Size = this.tabControl1.Size;
            panelResultadoBusqueda.Anchor = AnchorStyles.None;   // controlado por AjustarLayoutTabControl
            panelResultadoBusqueda.Visible = false;

            // Título dentro del panel
            lblResultadoTitulo = new Label();
            lblResultadoTitulo.Text = "Resultados de búsqueda";
            lblResultadoTitulo.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold);
            lblResultadoTitulo.ForeColor = Color.DarkSlateBlue;
            lblResultadoTitulo.Location = new Point(6, 6);
            lblResultadoTitulo.AutoSize = true;
            panelResultadoBusqueda.Controls.Add(lblResultadoTitulo);

            // Grid de resultados
            dgvResultados = new DataGridView();
            dgvResultados.Location = new Point(6, 30);
            dgvResultados.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvResultados.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvResultados.ReadOnly = true;
            dgvResultados.AllowUserToAddRows = false;
            dgvResultados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvResultados.BackgroundColor = Color.White;
            dgvResultados.BorderStyle = BorderStyle.FixedSingle;
            dgvResultados.GridColor = Color.FromArgb(220, 220, 220);
            dgvResultados.DefaultCellStyle.BackColor = Color.White;
            dgvResultados.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 250);
            dgvResultados.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(59, 77, 155);
            dgvResultados.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvResultados.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            panelResultadoBusqueda.Controls.Add(dgvResultados);

            if (this.panel2 != null)
                this.panel2.Controls.Add(panelResultadoBusqueda);

            // Ajustar el tamaño del grid al panel
            panelResultadoBusqueda.Resize += (s, e) =>
            {
                dgvResultados.Size = new Size(
                    panelResultadoBusqueda.Width - 12,
                    panelResultadoBusqueda.Height - 36);
            };
        }

        // ─────────────────────────────────────────────────────────────
        //  LÓGICA DE BÚSQUEDA
        // ─────────────────────────────────────────────────────────────

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            // Ignorar cuando el texto mostrado es el placeholder (color gris)
            if (txtBuscar.ForeColor == Color.Gray) return;

            string texto = txtBuscar.Text.Trim();

            // Mostrar / ocultar botón Limpiar
            btnLimpiar.Visible = texto.Length > 0;

            if (texto.Length == 0)
            {
                // Sin texto → mostrar pestañas normales
                MostrarModoPestañas();
                return;
            }

            // Búsqueda en tiempo real desde 2 caracteres
            if (texto.Length >= 2)
                EjecutarBusqueda();
        }

        private void EjecutarBusqueda()
        {
            // No buscar si se muestra el placeholder
            if (txtBuscar.ForeColor == Color.Gray) return;

            string texto = txtBuscar.Text.Trim();
            if (string.IsNullOrEmpty(texto)) return;

            DataTable dt = BaseDeDatos.BuscarAlumnos(texto);

            int total = dt.Rows.Count;
            lblResultadoTitulo.Text = total > 0
                ? $"✅ {total} alumno(s) encontrado(s) para: \"{texto}\""
                : $"❌ No se encontraron alumnos para: \"{texto}\"";

            dgvResultados.DataSource = dt;

            MostrarModoResultados();
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            txtBuscar.Clear();
            txtBuscar.Focus();
            MostrarModoPestañas();
        }

        // ─────────────────────────────────────────────────────────────
        //  ALTERNAR ENTRE PESTAÑAS Y RESULTADOS
        // ─────────────────────────────────────────────────────────────

        private void MostrarModoResultados()
        {
            tabControl1.Visible = false;
            panelResultadoBusqueda.Location = tabControl1.Location;
            panelResultadoBusqueda.Size = tabControl1.Size;
            panelResultadoBusqueda.Anchor = tabControl1.Anchor;
            panelResultadoBusqueda.Visible = true;
            panelResultadoBusqueda.BringToFront();
        }

        private void MostrarModoPestañas()
        {
            panelResultadoBusqueda.Visible = false;
            tabControl1.Visible = true;
        }

        // ─────────────────────────────────────────────────────────────
        //  CARGA DE PESTAÑAS POR CARRERA
        // ─────────────────────────────────────────────────────────────

        private void CargarTabsPorCarrera()
        {
            tabControl1.TabPages.Clear();

            string criterio = "Nombre";
            if (cmbOrdenar != null)
            {
                if (cmbOrdenar.SelectedIndex == 1) criterio = "Numero_control_alumno ASC";
                else if (cmbOrdenar.SelectedIndex == 2) criterio = "semestre ASC, grupo ASC, Nombre ASC";
                else if (cmbOrdenar.SelectedIndex == 3) criterio = ""; // Orden natural (inserción)
            }

            foreach (string carrera in Validaciones.CarrerasPermitidas)
            {
                DataTable dt = BaseDeDatos.ObtenerAlumnosPorCarrera(carrera, criterio);

                TabPage tab = new TabPage();
                tab.Text = CarreraAbreviada(carrera);
                tab.ToolTipText = carrera;
                tab.BackColor = Color.FromArgb(250, 250, 250);

                Label lbl = new Label();
                lbl.Text = carrera;
                lbl.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold);
                lbl.AutoSize = true;
                lbl.Location = new Point(6, 8);
                lbl.ForeColor = Color.DarkSlateBlue;
                tab.Controls.Add(lbl);

                DataGridView dgv = new DataGridView();
                dgv.Location = new Point(6, 30);
                dgv.Size = new Size(tab.Width - 20, tab.Height - 45);
                dgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dgv.ReadOnly = true;
                dgv.AllowUserToAddRows = false;
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgv.DataSource = dt;
                tab.Controls.Add(dgv);

                tabControl1.TabPages.Add(tab);
            }
        }

        // ─────────────────────────────────────────────────────────────
        //  AUXILIARES
        // ─────────────────────────────────────────────────────────────

        private string CarreraAbreviada(string carrera)
        {
            if (carrera.Contains("SISTEMAS"))      return "Sistemas";
            if (carrera.Contains("INDUSTRIAL"))    return "Industrial";
            if (carrera.Contains("MECATRONICA"))   return "Mecatrónica";
            if (carrera.Contains("CIVIL"))         return "Civil";
            if (carrera.Contains("GESTION"))       return "Gestión";
            if (carrera.Contains("ADMINISTRACION"))return "Administración";
            if (carrera.Contains("ARQUITECTURA"))  return "Arquitectura";
            return carrera.Substring(0, Math.Min(12, carrera.Length));
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            // Limpiar búsqueda y recargar las pestañas
            if (txtBuscar != null) txtBuscar.Clear();
            MostrarModoPestañas();
            CargarTabsPorCarrera();
        }

        private void btnSalir_Click(object sender, EventArgs e) => Form1.Instancia.NavegarA(new proyecto.Estudiantes());
        private void INICIO_Click(object sender, EventArgs e)   => Form1.Instancia.NavegarA(new proyecto.Estudiantes());
        private void button1_Click(object sender, EventArgs e)  => this.Close();
    }
}
