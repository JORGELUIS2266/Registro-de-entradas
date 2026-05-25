using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace proyecto
{
    public partial class Form1 : Form
    {
        public static Form1 Instancia;
        private Panel panelContenedorSecundario;
        private Panel panelDashboard;
        private Panel panelNavbar;
        
        // Colores del navbar
        private Color tabNormal = Color.FromArgb(240, 240, 240);
        private Color tabHover = Color.FromArgb(220, 220, 220);
        private Color tabActive = Color.White;
        private Color tabLine = Color.FromArgb(59, 77, 155); // #3b4d9b

        // Botones del navbar
        private Button btnInicio;
        private Button btnEstudiantes;
        private Button btnProfesores;
        private Button btnUsuarios;
        private Button btnCerrarSesion;

        private Button botonActivo = null;

        public Form1()
        {
            InitializeComponent();
            Instancia = this;

            // Ocultar elementos QR
            foreach (Control ctrl in this.panel2.Controls)
            {
                if (ctrl.Name == "pictureBox3" || ctrl.Name == "richTextBox1")
                    ctrl.Visible = false;
            }

            // Ocultar el panel4 (la barra de navegación vieja de Form1)
            if (this.panel4 != null) this.panel4.Visible = false;

            string etiquetaRol = Session.EsSuperusuario ? "👑 Superusuario" : "👤 Usuario";
            this.Text = "Sistema de Entradas — " + Session.UsuarioActual + " (" + etiquetaRol + ")";
            this.BackColor = Color.FromArgb(245, 245, 245);

            this.FormClosing += Form1_FormClosing;
            
            ConfigurarInterfaz();
        }

        private void ConfigurarInterfaz()
        {
            // 1. Header (Encabezado)
            // Hacer el panel Interfaz transparente y dibujarle el gradiente
            this.Interfaz.BackColor = Color.Transparent;
            this.Interfaz.Paint -= panel1_Paint;
            this.Interfaz.Paint += (s, e) => {
                // Linear gradient de #004d4d a #3b4d9b
                using (LinearGradientBrush brush = new LinearGradientBrush(this.Interfaz.ClientRectangle, 
                    ColorTranslator.FromHtml("#004d4d"), ColorTranslator.FromHtml("#3b4d9b"), 0F))
                {
                    e.Graphics.FillRectangle(brush, this.Interfaz.ClientRectangle);
                }
            };
            // Asegurar que los picturebox tengan fondo transparente
            this.pictureBox1.BackColor = Color.Transparent;
            this.pictureBox2.BackColor = Color.Transparent;
            if(this.panel3 != null) this.panel3.BackColor = Color.Transparent; // donde estaba el logo derecho

            // 2. Navigation Bar (Barra de Navegación)
            panelNavbar = new Panel();
            panelNavbar.Dock = DockStyle.Top;
            panelNavbar.Height = 45;
            panelNavbar.BackColor = tabNormal;
            panelNavbar.Padding = new Padding(20, 0, 20, 0); // Padding lateral
            this.Controls.Add(panelNavbar);
            panelNavbar.BringToFront(); // Debe ir debajo del Interfaz, por lo que el orden Docking importa.
            // Para asegurar el orden correcto de Dock (Interfaz arriba, luego Navbar, luego Contenido):
            this.Interfaz.SendToBack();

            // Botones (Pestañas)
            int posX = 20;
            btnInicio = CrearTab("INICIO", ref posX);
            btnEstudiantes = CrearTab("ESTUDIANTES", ref posX);
            btnProfesores = CrearTab("PROFESORES", ref posX);
            btnUsuarios = CrearTab("USUARIOS", ref posX);

            // Cerrar Sesion a la derecha
            btnCerrarSesion = new Button();
            btnCerrarSesion.Text = "Cerrar Sesión";
            btnCerrarSesion.FlatStyle = FlatStyle.Flat;
            btnCerrarSesion.FlatAppearance.BorderSize = 0;
            btnCerrarSesion.BackColor = Color.FromArgb(220, 53, 69);
            btnCerrarSesion.ForeColor = Color.White;
            btnCerrarSesion.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnCerrarSesion.Size = new Size(120, 30);
            btnCerrarSesion.Cursor = Cursors.Hand;
            btnCerrarSesion.Click += LOGOUT_Click;
            panelNavbar.Controls.Add(btnCerrarSesion);

            // Ajustar posición de Cerrar Sesion al redimensionar (margin-left: auto)
            panelNavbar.Resize += (s, e) => {
                btnCerrarSesion.Location = new Point(panelNavbar.Width - btnCerrarSesion.Width - 20, (panelNavbar.Height - btnCerrarSesion.Height) / 2);
            };

            // Eventos de Pestañas
            btnInicio.Click += (s, e) => { ActivarTab(btnInicio); MostrarDashboard(); };
            btnEstudiantes.Click += (s, e) => { ActivarTab(btnEstudiantes); NavegarA(new proyecto.Estudiantes()); };
            btnProfesores.Click += (s, e) => { ActivarTab(btnProfesores); NavegarA(new proyecto.Profesores()); };
            
            if (Session.EsSuperusuario) {
                btnUsuarios.Click += (s, e) => {
                    // Mantenemos la lógica de pop-up como pidieron, 
                    // o lo metemos dentro? "exepto el apartado de usuarios ese si que se abra la pestaña tal y como esta".
                    // Entonces abrimos Dialog pero sin cambiar el Tab Activo
                    new GestionUsuarios().ShowDialog(this);
                };
            } else {
                btnUsuarios.Visible = false;
            }

            // 3. Main Content Area
            panelContenedorSecundario = new Panel();
            panelContenedorSecundario.Dock = DockStyle.Fill;
            panelContenedorSecundario.BackColor = Color.FromArgb(250, 250, 250);
            this.Controls.Add(panelContenedorSecundario);
            panelContenedorSecundario.BringToFront();

            // Dashboard
            ConstruirDashboard();

            // Iniciar en la pestaña Inicio
            ActivarTab(btnInicio);
            MostrarDashboard();
        }

        private Button CrearTab(string texto, ref int x)
        {
            Button btn = new Button();
            btn.Text = texto;
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = tabNormal;
            btn.ForeColor = Color.DimGray;
            btn.Cursor = Cursors.Hand;
            
            // Medir el texto para dar tamaño dinámico
            using (Graphics g = CreateGraphics()) {
                SizeF size = g.MeasureString(texto, btn.Font);
                btn.Size = new Size((int)size.Width + 30, panelNavbar.Height);
            }
            
            btn.Location = new Point(x, 0);
            x += btn.Width;

            // Hover events
            btn.MouseEnter += (s, e) => { if(botonActivo != btn) btn.BackColor = tabHover; };
            btn.MouseLeave += (s, e) => { if(botonActivo != btn) btn.BackColor = tabNormal; };

            // Dibujar la linea inferior si esta activo
            btn.Paint += (s, e) => {
                if(botonActivo == btn) {
                    using(SolidBrush brush = new SolidBrush(tabLine)) {
                        e.Graphics.FillRectangle(brush, 0, btn.Height - 4, btn.Width, 4);
                    }
                }
            };

            panelNavbar.Controls.Add(btn);
            return btn;
        }

        private void ActivarTab(Button btn)
        {
            if(botonActivo != null) {
                botonActivo.BackColor = tabNormal;
                botonActivo.ForeColor = Color.DimGray;
            }
            botonActivo = btn;
            botonActivo.BackColor = tabActive;
            botonActivo.ForeColor = Color.Black;
            panelNavbar.Invalidate(true);
        }

        private void ConstruirDashboard()
        {
            panelDashboard = new Panel();
            panelDashboard.Dock = DockStyle.Fill;
            panelDashboard.BackColor = Color.FromArgb(250, 250, 250);

            int cardWidth = 220;
            int cardHeight = 250;
            int spacing = 40;
            int totalWidth = (cardWidth * 3) + (spacing * 2);

            Panel cardEstudiantes = CrearTarjeta("🎓", "Estudiantes", "Sistema de información estudiantil", cardWidth, cardHeight);
            EventHandler clickEstudiantes = (s, e) => { ActivarTab(btnEstudiantes); NavegarA(new proyecto.Estudiantes()); };
            cardEstudiantes.Click += clickEstudiantes;
            AsignarClickHijos(cardEstudiantes, cardEstudiantes, clickEstudiantes);

            Panel cardProfesores = CrearTarjeta("👨‍🏫", "Profesores", "Administración de Docentes", cardWidth, cardHeight);
            EventHandler clickProfesores = (s, e) => { ActivarTab(btnProfesores); NavegarA(new proyecto.Profesores()); };
            cardProfesores.Click += clickProfesores;
            AsignarClickHijos(cardProfesores, cardProfesores, clickProfesores);

            Panel cardUsuarios = CrearTarjeta("👥", "Usuarios", "Configuración de Cuentas", cardWidth, cardHeight);
            if (!Session.EsSuperusuario) cardUsuarios.Visible = false;
            EventHandler clickUsuarios = (s, e) => {
                new GestionUsuarios().ShowDialog(this);
            };
            cardUsuarios.Click += clickUsuarios;
            AsignarClickHijos(cardUsuarios, cardUsuarios, clickUsuarios);

            panelDashboard.Controls.Add(cardEstudiantes);
            panelDashboard.Controls.Add(cardProfesores);
            panelDashboard.Controls.Add(cardUsuarios);
            
            // Centrar
            panelDashboard.Resize += (s, e) => {
                int startX = (panelDashboard.Width - totalWidth) / 2;
                int startY = (panelDashboard.Height - cardHeight) / 2;
                cardEstudiantes.Location = new Point(startX, startY);
                cardProfesores.Location = new Point(startX + cardWidth + spacing, startY);
                cardUsuarios.Location = new Point(startX + (cardWidth + spacing) * 2, startY);
            };
        }

        private Panel CrearTarjeta(string iconoTxt, string tituloTxt, string subTxt, int w, int h)
        {
            Panel p = new Panel();
            p.Size = new Size(w, h);
            p.BackColor = Color.White;
            p.Cursor = Cursors.Hand;

            p.Paint += (s, e) => {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                // Sombra suave (muy basica, pero funcional)
                using (Pen shadow = new Pen(Color.FromArgb(50, 0,0,0), 3))
                {
                    g.DrawRoundedRectangle(shadow, 2, 2, w - 3, h - 3, 15);
                }
                using (Pen pen = new Pen(Color.LightGray, 1))
                {
                    g.DrawRoundedRectangle(pen, 0, 0, w - 3, h - 3, 15);
                }
            };

            p.MouseEnter += (s, e) => { p.BackColor = Color.FromArgb(245, 245, 255); };
            p.MouseLeave += (s, e) => { p.BackColor = Color.White; };

            Label lblIcon = new Label();
            lblIcon.Text = iconoTxt;
            lblIcon.Font = new Font("Segoe UI Emoji", 48);
            lblIcon.AutoSize = false;
            lblIcon.Size = new Size(w, 80);
            lblIcon.Location = new Point(0, 30);
            lblIcon.TextAlign = ContentAlignment.MiddleCenter;
            p.Controls.Add(lblIcon);

            Label lblTitulo = new Label();
            lblTitulo.Text = tituloTxt;
            lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitulo.AutoSize = false;
            lblTitulo.Size = new Size(w, 30);
            lblTitulo.Location = new Point(0, 120);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            p.Controls.Add(lblTitulo);

            Label lblSub = new Label();
            lblSub.Text = subTxt;
            lblSub.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblSub.ForeColor = Color.DimGray;
            lblSub.AutoSize = false;
            lblSub.Size = new Size(w - 20, 60);
            lblSub.Location = new Point(10, 160);
            lblSub.TextAlign = ContentAlignment.TopCenter;
            p.Controls.Add(lblSub);

            return p;
        }

        private void AsignarClickHijos(Control contenedor, Panel tarjeta, EventHandler action)
        {
            foreach (Control c in contenedor.Controls)
            {
                c.Cursor = Cursors.Hand;
                c.Click += action;
                c.MouseEnter += (s, e) => { tarjeta.BackColor = Color.FromArgb(245, 245, 255); };
                c.MouseLeave += (s, e) => { tarjeta.BackColor = Color.White; };
            }
        }

        public void MostrarDashboard()
        {
            panelContenedorSecundario.Controls.Clear();
            panelContenedorSecundario.Controls.Add(panelDashboard);
        }

        public void NavegarA(Form frm)
        {
            panelContenedorSecundario.Controls.Clear();

            frm.TopLevel = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            
            // Ocultar las interfaces viejas del hijo para que no se dupliquen
            OcultarNavegacionVieja(frm);

            panelContenedorSecundario.Controls.Add(frm);
            frm.Show();

            frm.FormClosed += (s, e) => {
                if (panelContenedorSecundario.Controls.Count == 0) {
                    ActivarTab(btnInicio);
                    MostrarDashboard();
                }
            };
        }

        private void OcultarNavegacionVieja(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c.Name == "Interfaz" || c.Name == "panel1" || c.Name == "panel4") 
                {
                    c.Visible = false;
                }
                
                if (c.HasChildren)
                {
                    OcultarNavegacionVieja(c);
                }
            }
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int hparam);

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void LOGOUT_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea cerrar la sesión actual?", "Cerrar Sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                BaseDeDatos.CerrarSesion(Session.UsuarioActual);
                Session.Limpiar();
                this.DialogResult = DialogResult.Retry;
                this.Close();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.Retry)
            {
                BaseDeDatos.CerrarSesion(Session.UsuarioActual);
            }
        }
        
        // Mantener vacíos los eventos que se generaron en el Designer viejo para que no explote
        private void panel3_Paint(object sender, PaintEventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void panel2_Paint(object sender, PaintEventArgs e) { }
        private void panel4_Paint(object sender, PaintEventArgs e) { }
        private void INICIO_Click(object sender, EventArgs e) { }
        private void ESTUDIANTES_Click(object sender, EventArgs e) { }
        private void button3_Click(object sender, EventArgs e) { }
        private void btnGestionUsuarios_Click(object sender, EventArgs e) { }
    }

    public static class GraphicsExtensions
    {
        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, int x, int y, int width, int height, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(x, y, radius * 2, radius * 2, 180, 90);
            path.AddArc(x + width - (radius * 2), y, radius * 2, radius * 2, 270, 90);
            path.AddArc(x + width - (radius * 2), y + height - (radius * 2), radius * 2, radius * 2, 0, 90);
            path.AddArc(x, y + height - (radius * 2), radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            graphics.DrawPath(pen, path);
        }
    }
}
