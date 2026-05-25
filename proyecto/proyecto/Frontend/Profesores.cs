using System;
using System.Drawing;
using System.Windows.Forms;

namespace proyecto
{
    public partial class Profesores : Form
    {
        private Panel gridPanel;

        public Profesores()
        {
            InitializeComponent();
            ConfigurarTarjetas();
        }

        private void ConfigurarTarjetas()
        {
            this.panel2.Controls.Clear();
            this.panel2.BackColor = Color.FromArgb(250, 250, 250);

            gridPanel = new Panel();
            gridPanel.Dock = DockStyle.Fill;
            this.panel2.Controls.Add(gridPanel);

            int cardWidth = 220;
            int cardHeight = 250;
            int spacingX = 40;
            int spacingY = 40;

            Panel cardNuevo = CrearTarjeta("🧑‍🏫+", "Ingresar Nuevo", "Registro de nuevo profesor", cardWidth, cardHeight);
            EventHandler clickNuevo = (s, e) => { Form1.Instancia.NavegarA(new proyecto.Ingresar_Nuevo_Profesor()); };
            cardNuevo.Click += clickNuevo;
            AsignarClickHijos(cardNuevo, cardNuevo, clickNuevo);

            Panel cardModificar = CrearTarjeta("✏️", "Modificar Profesor", "Actualización de información", cardWidth, cardHeight);
            EventHandler clickModificar = (s, e) => { Form1.Instancia.NavegarA(new proyecto.Modificar_Profesor_Existente()); };
            cardModificar.Click += clickModificar;
            AsignarClickHijos(cardModificar, cardModificar, clickModificar);

            Panel cardEliminar = CrearTarjeta("🗑️", "Eliminar Profesor", "Borrado de registros", cardWidth, cardHeight);
            EventHandler clickEliminar = (s, e) => { Form1.Instancia.NavegarA(new proyecto.Eliminar_Profesor()); };
            cardEliminar.Click += clickEliminar;
            AsignarClickHijos(cardEliminar, cardEliminar, clickEliminar);

            Panel cardLista = CrearTarjeta("🗂️", "Lista Completa", "Consulta listado de profesores", cardWidth, cardHeight);
            EventHandler clickLista = (s, e) => { Form1.Instancia.NavegarA(new proyecto.Lista_completa_Profesores()); };
            cardLista.Click += clickLista;
            AsignarClickHijos(cardLista, cardLista, clickLista);

            gridPanel.Controls.Add(cardNuevo);
            gridPanel.Controls.Add(cardModificar);
            gridPanel.Controls.Add(cardEliminar);
            gridPanel.Controls.Add(cardLista);

            gridPanel.Resize += (s, e) => {
                int totalW = (cardWidth * 2) + spacingX;
                int totalH = (cardHeight * 2) + spacingY;
                int startX = (gridPanel.Width - totalW) / 2;
                int startY = (gridPanel.Height - totalH) / 2;

                cardNuevo.Location = new Point(startX, startY);
                cardModificar.Location = new Point(startX + cardWidth + spacingX, startY);
                cardEliminar.Location = new Point(startX, startY + cardHeight + spacingY);
                cardLista.Location = new Point(startX + cardWidth + spacingX, startY + cardHeight + spacingY);
            };
        }

        private Panel CrearTarjeta(string iconoTxt, string tituloTxt, string subTxt, int w, int h)
        {
            Panel p = new Panel();
            p.Size = new Size(w, h);
            p.BackColor = Color.White;
            p.Cursor = Cursors.Hand;

            p.Paint += (s, e) => {
                using (Pen pen = new Pen(Color.FromArgb(220, 220, 230), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
            };

            p.MouseEnter += (s, e) => { p.BackColor = Color.FromArgb(245, 245, 255); };
            p.MouseLeave += (s, e) => { p.BackColor = Color.White; };

            Label lblIcon = new Label();
            lblIcon.Text = iconoTxt;
            lblIcon.Font = new Font("Segoe UI Emoji", 40);
            lblIcon.AutoSize = false;
            lblIcon.Size = new Size(w, 80);
            lblIcon.Location = new Point(0, 30);
            lblIcon.TextAlign = ContentAlignment.MiddleCenter;
            p.Controls.Add(lblIcon);

            Label lblTitulo = new Label();
            lblTitulo.Text = tituloTxt;
            lblTitulo.Font = new Font("Segoe UI", 13, FontStyle.Bold);
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

        private void button3_Click(object sender, EventArgs e) { }
        private void button7_Click(object sender, EventArgs e) { }
        private void button4_Click(object sender, EventArgs e) { }
        private void button5_Click(object sender, EventArgs e) { }
        private void button6_Click(object sender, EventArgs e) { }
        private void INICIO_Click(object sender, EventArgs e) { }
        private void button2_Click(object sender, EventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { }
    }
}
