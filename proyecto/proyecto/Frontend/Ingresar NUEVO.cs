using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace proyecto
{
    public partial class Ingresar_NUEVO : Form
    {
        private ComboBox cmbCarrera;
        private ComboBox cmbSemestre;
        private TextBox Apellido = new TextBox();

        public Ingresar_NUEVO()
        {
            InitializeComponent();

            // Validaciones (SIN CAMBIOS)
            if (NumeroControl != null)
            {
                NumeroControl.MaxLength = 8;
                NumeroControl.KeyPress += (s, e) => {
                    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
                };
            }
            if (Nombre != null)
            {
                Nombre.MaxLength = 30;
                Nombre.KeyPress += (s, e) => { if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar)) e.Handled = true; };
            }
            Apellido.MaxLength = 30;
            Apellido.KeyPress += (s, e) => { if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar)) e.Handled = true; };

            if (Grupo != null)
            {
                Grupo.MaxLength = 2;
                Grupo.KeyPress += (s, e) => {
                    if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar)) e.Handled = true;
                };
                Grupo.CharacterCasing = CharacterCasing.Upper;
            }

            ConfigurarComboCarrera();
            ConfigurarComboSemestre();

            AplicarDisenoModerno();

            this.Load += (s, e) => {
                RefrescarTabla();
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
            lblTitulo.Text = "Ingresar Nuevo Estudiante";
            lblTitulo.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(30, 41, 59);
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Height = 36;
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            leftCard.Controls.Add(lblTitulo);

            Panel sep = new Panel();
            sep.Height = 2; sep.Dock = DockStyle.Top;
            sep.BackColor = Color.FromArgb(226, 232, 240);
            leftCard.Controls.Add(sep);

            Panel botonesPanel = new Panel();
            botonesPanel.Dock = DockStyle.Bottom;
            botonesPanel.Height = 48;
            botonesPanel.BackColor = Color.White;
            botonesPanel.Padding = new Padding(0, 8, 0, 0);
            leftCard.Controls.Add(botonesPanel);

            Button btnGuardar = button2;
            EstilizarBoton(btnGuardar, "💾  Guardar Nuevo", Color.FromArgb(59, 130, 246));
            btnGuardar.Size = new Size(145, 34); btnGuardar.Location = new Point(0, 8);
            botonesPanel.Controls.Add(btnGuardar);

            Button btnLimpiar = button3;
            EstilizarBoton(btnLimpiar, "🧹  Limpiar", Color.FromArgb(100, 116, 139));
            btnLimpiar.Size = new Size(100, 34); btnLimpiar.Location = new Point(153, 8);
            btnLimpiar.Click -= button3_Click;
            btnLimpiar.Click += (s, e) => LimpiarCampos();
            botonesPanel.Controls.Add(btnLimpiar);

            Button btnRegresar = button4;
            EstilizarBotonSecundario(btnRegresar, "← Regresar");
            btnRegresar.Size = new Size(100, 34); btnRegresar.Location = new Point(261, 8);
            btnRegresar.Click -= button4_Click;
            btnRegresar.Click += (s, e) => { Form1.Instancia.NavegarA(new proyecto.Estudiantes()); };
            botonesPanel.Controls.Add(btnRegresar);

            Panel fieldsPanel = new Panel();
            fieldsPanel.Dock = DockStyle.Fill;
            fieldsPanel.AutoScroll = true;
            fieldsPanel.Padding = new Padding(0, 12, 0, 0);
            leftCard.Controls.Add(fieldsPanel);
            fieldsPanel.BringToFront();

            int yPos = 16;
            yPos = AgregarCampo(fieldsPanel, "Nombre(s)", Nombre, yPos);
            yPos = AgregarCampo(fieldsPanel, "Apellidos", Apellido, yPos);
            yPos = AgregarCampo(fieldsPanel, "Carrera", cmbCarrera, yPos);
            yPos = AgregarCampo(fieldsPanel, "Número de Control", NumeroControl, yPos);
            yPos = AgregarCampo(fieldsPanel, "Correo Electrónico", correo, yPos);
            yPos = AgregarDosCampos(fieldsPanel, "Semestre", cmbSemestre, "Grupo", Grupo, yPos);

            Panel rightCard = new Panel();
            rightCard.Dock = DockStyle.Fill;
            rightCard.BackColor = Color.White;
            rightCard.Padding = new Padding(16, 12, 16, 12);
            rightCard.Margin = new Padding(8, 16, 16, 16);
            EstilizarCard(rightCard);
            split.Controls.Add(rightCard, 1, 0);

            Label lblLista = new Label();
            lblLista.Text = "Lista de Alumnos";
            lblLista.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblLista.ForeColor = Color.FromArgb(30, 41, 59);
            lblLista.Dock = DockStyle.Top; lblLista.Height = 34;
            lblLista.TextAlign = ContentAlignment.MiddleLeft;
            rightCard.Controls.Add(lblLista);

            Panel sep2 = new Panel();
            sep2.Height = 2; sep2.Dock = DockStyle.Top;
            sep2.BackColor = Color.FromArgb(226, 232, 240);
            rightCard.Controls.Add(sep2);

            if (dataGridView1.Parent != null) dataGridView1.Parent.Controls.Remove(dataGridView1);
            EstilizarGrid(dataGridView1);
            dataGridView1.Dock = DockStyle.Fill;
            rightCard.Controls.Add(dataGridView1);
            dataGridView1.BringToFront();
            // Validación dinámica de Semestre y Grupo
            cmbSemestre.SelectedIndexChanged += (s, ev) => {
                string sem = cmbSemestre.SelectedItem?.ToString();
                if (!string.IsNullOrEmpty(sem))
                {
                    Grupo.Text = sem;
                    Grupo.MaxLength = sem.Length + 2;
                    Grupo.SelectionStart = Grupo.Text.Length;
                }
            };

            Grupo.KeyPress += (s, ev) => {
                string sem = cmbSemestre.SelectedItem?.ToString() ?? "";
                
                // Permitir borrar (Backspace) pero NO el prefijo del semestre
                if (ev.KeyChar == (char)Keys.Back)
                {
                    if (Grupo.SelectionStart <= sem.Length && Grupo.SelectionLength == 0)
                        ev.Handled = true;
                    else if (Grupo.SelectionStart < sem.Length)
                        ev.Handled = true;
                    return;
                }

                // Solo permitir letras
                if (!char.IsLetter(ev.KeyChar))
                {
                    ev.Handled = true;
                    return;
                }
                
                // Convertir a mayúscula automáticamente
                ev.KeyChar = char.ToUpper(ev.KeyChar);

                // Prevenir escritura antes o dentro del prefijo
                if (Grupo.SelectionStart < sem.Length)
                {
                    ev.Handled = true;
                }
            };

            Grupo.TextChanged += (s, ev) => {
                string sem = cmbSemestre.SelectedItem?.ToString() ?? "";
                if (!string.IsNullOrEmpty(sem) && !Grupo.Text.StartsWith(sem))
                {
                    Grupo.Text = sem;
                    Grupo.SelectionStart = Grupo.Text.Length;
                }
            };

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

            Label lB = new Label();
            lB.Text = lbl2;
            lB.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lB.ForeColor = Color.FromArgb(71, 85, 105);
            lB.AutoSize = true;
            parent.Controls.Add(lB);

            if (ctrl1.Parent != null) ctrl1.Parent.Controls.Remove(ctrl1);
            if (ctrl2.Parent != null) ctrl2.Parent.Controls.Remove(ctrl2);

            AplicarEstiloInput(ctrl1);
            if (!(ctrl2 is Button)) AplicarEstiloInput(ctrl2);

            Control c1 = EnvolverSiComboBox(ctrl1);
            Control c2 = EnvolverSiComboBox(ctrl2);

            parent.Resize += (s, e) => {
                int halfW = (parent.ClientSize.Width - 20) / 2;
                c1.Location = new Point(0, y + 20);
                c1.Width = halfW;
                lB.Location = new Point(halfW + 10, y);
                c2.Location = new Point(halfW + 10, y + 20);
                c2.Width = halfW;
            };
            if(parent.ClientSize.Width > 0) {
                int halfW = (parent.ClientSize.Width - 20) / 2;
                c1.Location = new Point(0, y + 20); c1.Width = halfW;
                lB.Location = new Point(halfW + 10, y);
                c2.Location = new Point(halfW + 10, y + 20); c2.Width = halfW;
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
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(59, 77, 155);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 36; dgv.EnableHeadersVisualStyles = false;
            
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgv.DefaultCellStyle.Padding = new Padding(4, 0, 4, 0);
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            dgv.RowTemplate.Height = 30;
        }

        // ─── COMBOS (SIN CAMBIOS) ─────────────────────────────────────────────

        private void ConfigurarComboCarrera()
        {
            Control viejo = null;
            foreach (Control ctrl in panel5.Controls)
                if (ctrl.Name == "Carrera") { viejo = ctrl; break; }
            cmbCarrera = new ComboBox();
            cmbCarrera.Name = "Carrera"; cmbCarrera.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCarrera.Font = viejo != null ? viejo.Font : this.Font;
            cmbCarrera.Location = viejo != null ? viejo.Location : new Point(15, 105);
            cmbCarrera.Size = new Size(290, 21);
            cmbCarrera.TabIndex = viejo != null ? viejo.TabIndex : 6;
            cmbCarrera.Items.AddRange(Validaciones.CarrerasPermitidas);
            if (viejo != null) panel5.Controls.Remove(viejo);
            panel5.Controls.Add(cmbCarrera);
        }

        private void ConfigurarComboSemestre()
        {
            Control viejo = null;
            foreach (Control ctrl in panel5.Controls)
                if (ctrl.Name == "Semestre") { viejo = ctrl; break; }
            cmbSemestre = new ComboBox();
            cmbSemestre.Name = "Semestre"; cmbSemestre.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSemestre.Font = viejo != null ? viejo.Font : this.Font;
            cmbSemestre.Location = new Point(15, 156); cmbSemestre.Size = new Size(125, 21);
            cmbSemestre.TabIndex = viejo != null ? viejo.TabIndex : 7;
            for (int i = 1; i <= 14; i++) cmbSemestre.Items.Add(i.ToString());
            if (viejo != null) panel5.Controls.Remove(viejo);
            panel5.Controls.Add(cmbSemestre);
            if (Grupo != null) { Grupo.Location = new Point(165, 156); Grupo.Size = new Size(60, 20); }
            foreach (Control ctrl in panel5.Controls)
                if (ctrl.Name == "richTextBox4") { ctrl.Location = new Point(165, 131); break; }
        }

        // ─── LÓGICA DE NEGOCIO (SIN CAMBIOS) ─────────────────────────────────

        private void RefrescarTabla() { dataGridView1.DataSource = BaseDeDatos.ObtenerAlumnos(); }

        private void button2_Click(object sender, EventArgs e)
        {
            string carreraSeleccionada = cmbCarrera.SelectedItem != null ? cmbCarrera.SelectedItem.ToString() : "";
            string semestreSeleccionado = cmbSemestre.SelectedItem != null ? cmbSemestre.SelectedItem.ToString() : "";
            if (!Validaciones.ValidarCamposAlumno(NumeroControl.Text, Nombre.Text, Apellido.Text, carreraSeleccionada, correo.Text, semestreSeleccionado, Grupo.Text))
                return;
            if (BaseDeDatos.InsertarAlumno(NumeroControl.Text.Trim(), Nombre.Text.Trim(), Apellido.Text.Trim(), carreraSeleccionada,
                correo.Text.Trim(), int.Parse(semestreSeleccionado), Grupo.Text.Trim()))
            {
                MessageBox.Show("¡Alumno guardado exitosamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefrescarTabla(); LimpiarCampos();
            }
        }

        private void LimpiarCampos()
        {
            NumeroControl.Clear(); Nombre.Clear(); Apellido.Clear();
            cmbCarrera.SelectedIndex = -1; correo.Clear();
            cmbSemestre.SelectedIndex = -1; Grupo.Clear();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                NumeroControl.Text = dataGridView1.CurrentRow.Cells["Numero_control_alumno"].Value?.ToString() ?? "";
                Nombre.Text = dataGridView1.CurrentRow.Cells["Nombre"].Value?.ToString() ?? "";
                Apellido.Text = dataGridView1.CurrentRow.Cells["apellido"].Value?.ToString() ?? "";
                string carreraVal = dataGridView1.CurrentRow.Cells["carrera"].Value?.ToString() ?? "";
                cmbCarrera.SelectedItem = carreraVal;
                if (cmbCarrera.SelectedItem == null) cmbCarrera.Text = carreraVal;
                correo.Text = dataGridView1.CurrentRow.Cells["correo"].Value?.ToString() ?? "";
                string semVal = dataGridView1.CurrentRow.Cells["semestre"].Value?.ToString() ?? "";
                cmbSemestre.SelectedItem = semVal;
                if (cmbSemestre.SelectedItem == null) cmbSemestre.Text = semVal;
                Grupo.Text = dataGridView1.CurrentRow.Cells["grupo"].Value?.ToString() ?? "";
            }
            catch { }
        }

        private void button4_Click(object sender, EventArgs e) { this.Close(); }
        private void button1_Click(object sender, EventArgs e) { this.Close(); }
        private void button3_Click(object sender, EventArgs e) { LimpiarCampos(); }
        private void button5_Click(object sender, EventArgs e) { }
        private void richTextBox2_TextChanged(object sender, EventArgs e) { }
        private void richTextBox3_TextChanged(object sender, EventArgs e) { }
        private void richTextBox7_TextChanged(object sender, EventArgs e) { }
        private void progressBar2_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void progressBar1_Click(object sender, EventArgs e) { }
        private void PROFESORES_Click(object sender, EventArgs e) { }
        private void INICIO_Click(object sender, EventArgs e) { this.Close(); }
    }
}
