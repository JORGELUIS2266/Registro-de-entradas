using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace proyecto
{
    public partial class Lista_completa_Profesores : Form
    {
        public Lista_completa_Profesores()
        {
            InitializeComponent();
            AplicarDisenoModerno();

            this.Load += (s, e) => { dataGridView1.DataSource = BaseDeDatos.ObtenerProfesores(); };
        }

        private void AplicarDisenoModerno()
        {
            this.SuspendLayout();
            panel2.Visible = false;
            panel2.Dock = DockStyle.None;

            Panel mainCard = new Panel();
            mainCard.Dock = DockStyle.Fill;
            mainCard.BackColor = Color.White;
            mainCard.Padding = new Padding(24);
            mainCard.Margin = new Padding(16);
            
            mainCard.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(Color.FromArgb(226, 232, 240), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, mainCard.Width - 1, mainCard.Height - 1);
            };

            Panel bgPanel = new Panel();
            bgPanel.Dock = DockStyle.Fill;
            bgPanel.BackColor = Color.FromArgb(245, 247, 250);
            bgPanel.Padding = new Padding(16);
            bgPanel.Controls.Add(mainCard);
            
            this.Controls.Add(bgPanel);
            bgPanel.BringToFront();

            Label lblTitulo = new Label();
            lblTitulo.Text = "Lista Completa de Profesores";
            lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(30, 41, 59);
            lblTitulo.Dock = DockStyle.Top; lblTitulo.Height = 40;
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            mainCard.Controls.Add(lblTitulo);

            Panel sep = new Panel(); sep.Height = 2; sep.Dock = DockStyle.Top;
            sep.BackColor = Color.FromArgb(226, 232, 240);
            mainCard.Controls.Add(sep);

            Panel headerSpace = new Panel(); headerSpace.Height = 16; headerSpace.Dock = DockStyle.Top;
            mainCard.Controls.Add(headerSpace);

            Panel gridPanel = new Panel();
            gridPanel.Dock = DockStyle.Fill;
            gridPanel.BorderStyle = BorderStyle.FixedSingle;
            mainCard.Controls.Add(gridPanel);
            gridPanel.BringToFront();

            if (dataGridView1.Parent != null) dataGridView1.Parent.Controls.Remove(dataGridView1);
            EstilizarGrid(dataGridView1);
            dataGridView1.Dock = DockStyle.Fill;
            gridPanel.Controls.Add(dataGridView1);
            dataGridView1.BringToFront();

            Panel botonesPanel = new Panel();
            botonesPanel.Dock = DockStyle.Bottom; botonesPanel.Height = 60;
            botonesPanel.BackColor = Color.White;
            mainCard.Controls.Add(botonesPanel);

            Button btnRegresar = new Button();
            btnRegresar.Text = "← Regresar al Menú";
            btnRegresar.BackColor = Color.White; btnRegresar.ForeColor = Color.FromArgb(71, 85, 105);
            btnRegresar.FlatStyle = FlatStyle.Flat; btnRegresar.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnRegresar.FlatAppearance.BorderSize = 1; btnRegresar.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold); 
            btnRegresar.Cursor = Cursors.Hand;
            btnRegresar.Size = new Size(160, 36);
            btnRegresar.Location = new Point(0, 16);
            btnRegresar.Click += (s, e) => { Form1.Instancia.NavegarA(new proyecto.Profesores()); };
            botonesPanel.Controls.Add(btnRegresar);

            Button btnActualizar = new Button();
            btnActualizar.Text = "🔄 Actualizar Lista";
            btnActualizar.BackColor = Color.FromArgb(59, 130, 246); btnActualizar.ForeColor = Color.White;
            btnActualizar.FlatStyle = FlatStyle.Flat; btnActualizar.FlatAppearance.BorderSize = 0;
            btnActualizar.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold); 
            btnActualizar.Cursor = Cursors.Hand;
            btnActualizar.Size = new Size(150, 36);
            btnActualizar.Location = new Point(170, 16);
            btnActualizar.Click += (s, e) => { dataGridView1.DataSource = BaseDeDatos.ObtenerProfesores(); };
            botonesPanel.Controls.Add(btnActualizar);

            this.ResumeLayout(false);
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
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 40; dgv.EnableHeadersVisualStyles = false;
            
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f);
            dgv.DefaultCellStyle.Padding = new Padding(4, 0, 4, 0);
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            dgv.RowTemplate.Height = 35;

            dgv.DataBindingComplete += (s, ev) => {
                if (dgv.Columns.Count > 1) dgv.Columns[1].HeaderText = "RFC";
            };
        }

        private void button2_Click(object sender, EventArgs e) { this.Close(); }
        private void INICIO_Click(object sender, EventArgs e) { this.Close(); }
        private void panel2_Paint(object sender, PaintEventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { this.Close(); }
        private void PROFESORES_Click(object sender, EventArgs e) { this.Close(); }
    }
}
