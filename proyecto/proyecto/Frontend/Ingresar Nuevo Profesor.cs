using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace proyecto
{
    public partial class Ingresar_Nuevo_Profesor : Form
    {
        private ComboBox cmbSexo;
        private TextBox apellido_prof = new TextBox();

        public Ingresar_Nuevo_Profesor()
        {
            InitializeComponent();
            ConfigurarRestricciones();
            ConfigurarComboSexo();
            apellido_prof.MaxLength = 30;
            apellido_prof.KeyPress += (s, e) => { if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar)) e.Handled = true; };
            
            AplicarDisenoModerno();

            this.Load += (s, e) =>
            {
                dataGridView1.DataSource = BaseDeDatos.ObtenerProfesores();
                idprofesor.Text = BaseDeDatos.ObtenerSiguienteIdProfesor().ToString();
                idprofesor.ReadOnly = true;
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
            lblTitulo.Text = "Ingresar Nuevo Profesor";
            lblTitulo.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(30, 41, 59);
            lblTitulo.Dock = DockStyle.Top; lblTitulo.Height = 36;
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            leftCard.Controls.Add(lblTitulo);

            Panel sep = new Panel(); sep.Height = 2; sep.Dock = DockStyle.Top;
            sep.BackColor = Color.FromArgb(226, 232, 240);
            leftCard.Controls.Add(sep);

            Panel botonesPanel = new Panel();
            botonesPanel.Dock = DockStyle.Bottom; botonesPanel.Height = 48;
            botonesPanel.BackColor = Color.White; botonesPanel.Padding = new Padding(0, 8, 0, 0);
            leftCard.Controls.Add(botonesPanel);

            EstilizarBoton(button2, "💾  Guardar Nuevo", Color.FromArgb(59, 130, 246));
            button2.Size = new Size(145, 34); button2.Location = new Point(0, 8);
            botonesPanel.Controls.Add(button2);

            EstilizarBoton(button3, "🧹  Limpiar", Color.FromArgb(100, 116, 139));
            button3.Size = new Size(100, 34); button3.Location = new Point(153, 8);
            botonesPanel.Controls.Add(button3);

            EstilizarBotonSecundario(button4, "← Regresar");
            button4.Size = new Size(100, 34); button4.Location = new Point(261, 8);
            button4.Click -= button4_Click;
            button4.Click += (s, e) => { Form1.Instancia.NavegarA(new proyecto.Profesores()); };
            botonesPanel.Controls.Add(button4);

            Panel fieldsPanel = new Panel();
            fieldsPanel.Dock = DockStyle.Fill;
            fieldsPanel.AutoScroll = true;
            fieldsPanel.Padding = new Padding(0, 12, 0, 0);
            leftCard.Controls.Add(fieldsPanel);
            fieldsPanel.BringToFront();

            int yPos = 16;
            yPos = AgregarDosCampos(fieldsPanel, "ID Profesor", idprofesor, "RFC", cedprofesional, yPos);
            yPos = AgregarCampo(fieldsPanel, "Nombre(s)", nombre, yPos);
            yPos = AgregarCampo(fieldsPanel, "Apellidos", apellido_prof, yPos);
            yPos = AgregarCampo(fieldsPanel, "Correo Electrónico", correo, yPos);
            yPos = AgregarDosCampos(fieldsPanel, "Teléfono", telefono, "Sexo", cmbSexo, yPos);

            Panel rightCard = new Panel();
            rightCard.Dock = DockStyle.Fill;
            rightCard.BackColor = Color.White;
            rightCard.Padding = new Padding(16, 12, 16, 12);
            rightCard.Margin = new Padding(8, 16, 16, 16);
            EstilizarCard(rightCard);
            split.Controls.Add(rightCard, 1, 0);

            Label lblLista = new Label();
            lblLista.Text = "Lista de Profesores";
            lblLista.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblLista.ForeColor = Color.FromArgb(30, 41, 59);
            lblLista.Dock = DockStyle.Top; lblLista.Height = 34;
            lblLista.TextAlign = ContentAlignment.MiddleLeft;
            rightCard.Controls.Add(lblLista);

            Panel sep2 = new Panel(); sep2.Height = 2; sep2.Dock = DockStyle.Top;
            sep2.BackColor = Color.FromArgb(226, 232, 240);
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

            dgv.DataBindingComplete += (s, ev) => {
                if (dgv.Columns.Count > 1) dgv.Columns[1].HeaderText = "RFC";
            };
        }

        // ─── LÓGICA DE NEGOCIO (SIN CAMBIOS) ─────────────────────────────────

        private void ConfigurarRestricciones()
        {
            if (idprofesor != null) { idprofesor.BackColor = System.Drawing.Color.LightGray; }
            if (cedprofesional != null)
            {
                cedprofesional.MaxLength = 13;
                cedprofesional.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
                cedprofesional.KeyPress += (sender, e) => {
                    if (!char.IsControl(e.KeyChar) && !char.IsLetterOrDigit(e.KeyChar)) e.Handled = true;
                };
            }
            if (nombre != null)
            {
                nombre.MaxLength = 50;
                nombre.KeyPress += (sender, e) => {
                    if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar)) e.Handled = true;
                };
            }
            if (telefono != null)
            {
                telefono.MaxLength = 10;
                telefono.KeyPress += (sender, e) => {
                    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
                };
            }
        }

        private void ConfigurarComboSexo()
        {
            Control viejo = null;
            foreach (Control ctrl in panel5.Controls) if (ctrl.Name == "sexo") { viejo = ctrl; break; }
            cmbSexo = new ComboBox(); cmbSexo.Name = "sexo"; cmbSexo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSexo.Font = viejo != null ? viejo.Font : this.Font;
            cmbSexo.Location = viejo != null ? viejo.Location : new System.Drawing.Point(15, 175);
            cmbSexo.Size = new System.Drawing.Size(290, 21); cmbSexo.TabIndex = viejo != null ? viejo.TabIndex : 8;
            cmbSexo.Items.Add("Hombre"); cmbSexo.Items.Add("Mujer");
            if (viejo != null) panel5.Controls.Remove(viejo);
            panel5.Controls.Add(cmbSexo);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sexoSeleccionado = cmbSexo.SelectedItem != null ? cmbSexo.SelectedItem.ToString() : "";
            if (!Validaciones.ValidarCamposProfesor(idprofesor.Text, cedprofesional.Text, nombre.Text, apellido_prof.Text, telefono.Text, correo.Text, sexoSeleccionado))
                return;
            if (BaseDeDatos.InsertarProfesor(int.Parse(idprofesor.Text), cedprofesional.Text.Trim().ToUpper(),
                nombre.Text.Trim(), apellido_prof.Text.Trim(), telefono.Text.Trim(), correo.Text.Trim(), sexoSeleccionado))
            {
                MessageBox.Show("¡Profesor guardado exitosamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView1.DataSource = BaseDeDatos.ObtenerProfesores();
                cedprofesional.Clear(); nombre.Clear(); apellido_prof.Clear(); telefono.Clear(); correo.Clear(); cmbSexo.SelectedIndex = -1;
                idprofesor.Text = BaseDeDatos.ObtenerSiguienteIdProfesor().ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cedprofesional.Clear(); nombre.Clear(); apellido_prof.Clear(); telefono.Clear(); correo.Clear(); cmbSexo.SelectedIndex = -1;
            idprofesor.Text = BaseDeDatos.ObtenerSiguienteIdProfesor().ToString();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                idprofesor.Text    = dataGridView1.CurrentRow.Cells["id_profesor"].Value?.ToString() ?? "";
                cedprofesional.Text= dataGridView1.CurrentRow.Cells["ced_profesional"].Value?.ToString() ?? "";
                nombre.Text        = dataGridView1.CurrentRow.Cells["nombre"].Value?.ToString() ?? "";
                apellido_prof.Text = dataGridView1.CurrentRow.Cells["apellido"].Value?.ToString() ?? "";
                telefono.Text      = dataGridView1.CurrentRow.Cells["telefono"].Value?.ToString() ?? "";
                correo.Text        = dataGridView1.CurrentRow.Cells["correo"].Value?.ToString() ?? "";
                string sexoVal     = dataGridView1.CurrentRow.Cells["sexo"].Value?.ToString() ?? "";
                cmbSexo.SelectedItem = sexoVal;
                if (cmbSexo.SelectedItem == null) cmbSexo.Text = sexoVal;
            }
            catch { }
        }

        private void button4_Click(object sender, EventArgs e) { this.Close(); }
        private void panel2_Paint(object sender, PaintEventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void panel3_Paint(object sender, PaintEventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void INICIO_Click(object sender, EventArgs e) { this.Close(); }
        private void button1_Click(object sender, EventArgs e) { }
        private void PROFESORES_Click(object sender, EventArgs e) { this.Close(); }
        private void panel4_Paint(object sender, PaintEventArgs e) { }
        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void richTextBox6_TextChanged(object sender, EventArgs e) { }
        private void panel5_Paint(object sender, PaintEventArgs e) { }
        private void richTextBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void richTextBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void richTextBox5_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void progressBar1_Click(object sender, EventArgs e) { }
        private void panel6_Paint(object sender, PaintEventArgs e) { }
        private void groupBox1_Enter(object sender, EventArgs e) { }
    }
}
