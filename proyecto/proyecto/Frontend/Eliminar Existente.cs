using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace proyecto
{
    public partial class Eliminar_Existente : Form
    {
        private bool _gridCargado = false;

        public Eliminar_Existente()
        {
            InitializeComponent();

            if (NumeroControl != null)
            {
                NumeroControl.MaxLength = 8;
                NumeroControl.KeyPress += (s, e) => {
                    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
                };
            }

            AplicarDisenoModerno();

            this.Load += (s, e) => {
                CargarGrid();
            };
        }

        private void AplicarDisenoModerno()
        {
            this.SuspendLayout();
            panel2.Visible = false;
            panel2.Dock = DockStyle.None;

            TableLayoutPanel split = new TableLayoutPanel();
            split.Dock = DockStyle.Fill;
            split.BackColor = Color.FromArgb(245, 247, 250);
            split.ColumnCount = 2;
            split.RowCount = 1;
            split.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
            split.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60f));
            split.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.Controls.Add(split);
            split.BringToFront();

            Panel leftCard = new Panel();
            leftCard.Dock = DockStyle.Fill;
            leftCard.BackColor = Color.White;
            leftCard.Padding = new Padding(24);
            leftCard.Margin = new Padding(16, 16, 8, 16);
            EstilizarCard(leftCard);
            split.Controls.Add(leftCard, 0, 0);

            Label lblTitulo = new Label();
            lblTitulo.Text = "Eliminar Estudiante";
            lblTitulo.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(220, 53, 69);
            lblTitulo.Dock = DockStyle.Top; lblTitulo.Height = 36;
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            leftCard.Controls.Add(lblTitulo);

            Panel sep = new Panel(); sep.Height = 2; sep.Dock = DockStyle.Top;
            sep.BackColor = Color.FromArgb(254, 202, 202);
            leftCard.Controls.Add(sep);

            Label lblAviso = new Label();
            lblAviso.Text = "⚠️  Selecciona un alumno de la lista. Acción irreversible.";
            lblAviso.Font = new Font("Segoe UI", 8.5f, FontStyle.Italic);
            lblAviso.ForeColor = Color.FromArgb(153, 27, 27);
            lblAviso.BackColor = Color.FromArgb(254, 242, 242);
            lblAviso.Dock = DockStyle.Top; lblAviso.Height = 36;
            lblAviso.Padding = new Padding(4, 8, 4, 4);
            leftCard.Controls.Add(lblAviso);

            Panel botonesPanel = new Panel();
            botonesPanel.Dock = DockStyle.Bottom; botonesPanel.Height = 48;
            botonesPanel.BackColor = Color.White; botonesPanel.Padding = new Padding(0, 8, 0, 0);
            leftCard.Controls.Add(botonesPanel);

            EstilizarBoton(button2, "🗑️  Eliminar", Color.FromArgb(220, 53, 69));
            button2.Size = new Size(130, 34); button2.Location = new Point(0, 8);
            botonesPanel.Controls.Add(button2);

            EstilizarBotonSecundario(button4, "← Regresar");
            button4.Size = new Size(110, 34); button4.Location = new Point(138, 8);
            button4.Click -= button4_Click;
            button4.Click += (s, e) => { Form1.Instancia.NavegarA(new proyecto.Estudiantes()); };
            botonesPanel.Controls.Add(button4);

            Panel fieldsPanel = new Panel();
            fieldsPanel.Dock = DockStyle.Fill;
            fieldsPanel.AutoScroll = true;
            fieldsPanel.Padding = new Padding(0, 12, 0, 0);
            leftCard.Controls.Add(fieldsPanel);
            fieldsPanel.BringToFront();

            EstilizarBoton(btnBuscar, "Buscar →", Color.FromArgb(59, 130, 246));

            int yPos = 16;
            yPos = AgregarDosCampos(fieldsPanel, "Número de Control", NumeroControl, "Buscar", btnBuscar, yPos);
            yPos = AgregarCampo(fieldsPanel, "Nombre Completo", Nombre, yPos);
            yPos = AgregarCampo(fieldsPanel, "Carrera", carrera, yPos);
            yPos = AgregarCampo(fieldsPanel, "Correo Electrónico", correo, yPos);
            yPos = AgregarDosCampos(fieldsPanel, "Semestre", semestre, "Grupo", grupo, yPos);

            Nombre.ReadOnly = true; carrera.ReadOnly = true;
            correo.ReadOnly = true; semestre.ReadOnly = true; grupo.ReadOnly = true;

            Panel rightCard = new Panel();
            rightCard.Dock = DockStyle.Fill;
            rightCard.BackColor = Color.White;
            rightCard.Padding = new Padding(16, 12, 16, 12);
            rightCard.Margin = new Padding(8, 16, 16, 16);
            EstilizarCard(rightCard);
            split.Controls.Add(rightCard, 1, 0);

            Label lblLista = new Label();
            lblLista.Text = "Selecciona el alumno a eliminar";
            lblLista.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblLista.ForeColor = Color.FromArgb(30, 41, 59);
            lblLista.Dock = DockStyle.Top; lblLista.Height = 34;
            lblLista.TextAlign = ContentAlignment.MiddleLeft;
            rightCard.Controls.Add(lblLista);

            Panel sep2 = new Panel(); sep2.Height = 2; sep2.Dock = DockStyle.Top;
            sep2.BackColor = Color.FromArgb(254, 202, 202);
            rightCard.Controls.Add(sep2);

            if (dataGridView1.Parent != null) dataGridView1.Parent.Controls.Remove(dataGridView1);
            EstilizarGrid(dataGridView1);
            dataGridView1.Dock = DockStyle.Fill;
            rightCard.Controls.Add(dataGridView1);
            dataGridView1.BringToFront();

            this.ResumeLayout(false);
        }

        private Control EnvolverSiComboBox(Control c)
        {
            if (c is ComboBox cmb)
            {
                Panel p = new Panel();
                p.BackColor = Color.FromArgb(248, 250, 252);
                p.BorderStyle = BorderStyle.FixedSingle;
                p.Height = cmb.PreferredHeight;
                cmb.FlatStyle = FlatStyle.Flat;
                cmb.Dock = DockStyle.Fill;
                p.Controls.Add(cmb);
                return p;
            }
            return c;
        }

        private int AgregarCampo(Panel parent, string etiqueta, Control campo, int y)
        {
            Label lbl = new Label();
            lbl.Text = etiqueta;
            lbl.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lbl.ForeColor = Color.FromArgb(71, 85, 105);
            lbl.Location = new Point(0, y);
            lbl.AutoSize = true;
            parent.Controls.Add(lbl);

            if (campo.Parent != null) campo.Parent.Controls.Remove(campo);
            
            AplicarEstiloInput(campo);
            Control cFinal = EnvolverSiComboBox(campo);
            
            cFinal.Location = new Point(0, y + 20);
            parent.Resize += (s, e) => { cFinal.Width = parent.ClientSize.Width - 10; };
            if(parent.ClientSize.Width > 0) cFinal.Width = parent.ClientSize.Width - 10;
            
            parent.Controls.Add(cFinal);
            return y + 20 + cFinal.Height + 16;
        }

        private int AgregarDosCampos(Panel parent, string lbl1, Control ctrl1, string lbl2, Control ctrl2, int y)
        {
            Label lA = new Label();
            lA.Text = lbl1;
            lA.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lA.ForeColor = Color.FromArgb(71, 85, 105);
            lA.Location = new Point(0, y);
            lA.AutoSize = true;
            parent.Controls.Add(lA);

            if (ctrl1.Parent != null) ctrl1.Parent.Controls.Remove(ctrl1);
            if (ctrl2.Parent != null) ctrl2.Parent.Controls.Remove(ctrl2);

            AplicarEstiloInput(ctrl1);
            if (!(ctrl2 is Button)) AplicarEstiloInput(ctrl2);

            Control c1 = EnvolverSiComboBox(ctrl1);
            Control c2 = EnvolverSiComboBox(ctrl2);

            if (!(ctrl2 is Button)) {
                Label lB = new Label();
                lB.Text = lbl2;
                lB.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                lB.ForeColor = Color.FromArgb(71, 85, 105);
                lB.AutoSize = true;
                parent.Controls.Add(lB);
                
                parent.Resize += (s, e) => {
                    int halfW = (parent.ClientSize.Width - 20) / 2;
                    c1.Location = new Point(0, y + 20); c1.Width = halfW;
                    lB.Location = new Point(halfW + 10, y);
                    c2.Location = new Point(halfW + 10, y + 20); c2.Width = halfW;
                };
                if(parent.ClientSize.Width > 0) {
                    int halfW = (parent.ClientSize.Width - 20) / 2;
                    c1.Location = new Point(0, y + 20); c1.Width = halfW;
                    lB.Location = new Point(halfW + 10, y);
                    c2.Location = new Point(halfW + 10, y + 20); c2.Width = halfW;
                }
            } else {
                parent.Resize += (s, e) => {
                    int halfW = (parent.ClientSize.Width - 20) / 2;
                    c1.Location = new Point(0, y + 20); c1.Width = halfW;
                    c2.Location = new Point(halfW + 10, y + 18); c2.Width = halfW;
                };
                if(parent.ClientSize.Width > 0) {
                    int halfW = (parent.ClientSize.Width - 20) / 2;
                    c1.Location = new Point(0, y + 20); c1.Width = halfW;
                    c2.Location = new Point(halfW + 10, y + 18); c2.Width = halfW;
                }
            }

            parent.Controls.Add(c1);
            parent.Controls.Add(c2);
            return y + 20 + Math.Max(c1.Height, c2.Height) + 16;
        }

        private void AplicarEstiloInput(Control c)
        {
            c.Font = new Font("Segoe UI", 10);
            if (c is TextBox tb) { 
                tb.BorderStyle = BorderStyle.FixedSingle; 
                tb.BackColor = Color.FromArgb(248, 250, 252); 
            }
            else if (c is ComboBox cmb) { 
                cmb.BackColor = Color.FromArgb(248, 250, 252); 
            }
        }

        private void EstilizarCard(Panel p)
        {
            p.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(Color.FromArgb(226, 232, 240), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
            };
        }

        private void EstilizarBoton(Button btn, string texto, Color color)
        {
            btn.Text = texto; btn.BackColor = color; btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat; btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 9, FontStyle.Bold); btn.Cursor = Cursors.Hand;
        }

        private void EstilizarBotonSecundario(Button btn, string texto)
        {
            btn.Text = texto; btn.BackColor = Color.White; btn.ForeColor = Color.FromArgb(71, 85, 105);
            btn.FlatStyle = FlatStyle.Flat; btn.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btn.FlatAppearance.BorderSize = 1; btn.Font = new Font("Segoe UI", 9, FontStyle.Bold); btn.Cursor = Cursors.Hand;
        }

        private void EstilizarGrid(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White; dgv.BorderStyle = BorderStyle.None;
            dgv.GridColor = Color.FromArgb(226, 232, 240); dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false; dgv.ReadOnly = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(180, 50, 50);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 36; dgv.EnableHeadersVisualStyles = false;
            
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgv.DefaultCellStyle.Padding = new Padding(4, 0, 4, 0);
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 248);
            dgv.RowTemplate.Height = 30;
        }

        // ─── LÓGICA (SIN CAMBIOS) ─────────────────────────────────────────────

        private void CargarGrid() { dataGridView1.DataSource = BaseDeDatos.ObtenerAlumnos(); }
        private void actualizar() { }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NumeroControl.Text))
            { MessageBox.Show("Selecciona un alumno para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (!Validaciones.ConfirmarEliminacion("alumno")) return;
            if (BaseDeDatos.EliminarAlumno(NumeroControl.Text.Trim()))
            {
                MessageBox.Show("¡Alumno eliminado correctamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarGrid();
                NumeroControl.Clear(); Nombre.Clear(); carrera.Clear();
                correo.Clear(); semestre.Clear(); grupo.Clear();
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e) { }
        private void Guardar() { }
        private void button4_Click(object sender, EventArgs e) { this.Close(); }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                NumeroControl.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                Nombre.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                carrera.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                correo.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                semestre.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                grupo.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            }
            catch { }
        }

        private void INICIO_Click(object sender, EventArgs e) { this.Close(); }
        private void button1_Click(object sender, EventArgs e) { this.Close(); }
        private void PROFESORES_Click(object sender, EventArgs e) { }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NumeroControl.Text))
            { MessageBox.Show("Ingresa un número de control para buscar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            DataRow alumno = BaseDeDatos.ObtenerAlumnoPorControl(NumeroControl.Text.Trim());
            if (alumno != null)
            {
                Nombre.Text = alumno["Nombre"].ToString(); carrera.Text = alumno["carrera"].ToString();
                correo.Text = alumno["correo"].ToString(); semestre.Text = alumno["semestre"].ToString();
                grupo.Text = alumno["grupo"].ToString();
            }
            else { MessageBox.Show("No se encontró ningún alumno con ese número de control.", "No encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }
    }
}
