namespace QRCode
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.buttonGenerar = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxHTTP = new System.Windows.Forms.TextBox();
            this.textBoxPrefijo = new System.Windows.Forms.TextBox();
            this.numericUpDownCantidad = new System.Windows.Forms.NumericUpDown();
            this.labelUltimo = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.labelCarpeta = new System.Windows.Forms.Label();
            this.labelCarpetaValue = new System.Windows.Forms.Label();
            this.buttonCarpeta = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCantidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(30, 209);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.progressBar1.Maximum = 999;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(649, 21);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 7;
            // 
            // buttonGenerar
            // 
            this.buttonGenerar.Location = new System.Drawing.Point(497, 108);
            this.buttonGenerar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonGenerar.Name = "buttonGenerar";
            this.buttonGenerar.Size = new System.Drawing.Size(100, 28);
            this.buttonGenerar.TabIndex = 5;
            this.buttonGenerar.Text = "Generar";
            this.buttonGenerar.UseVisualStyleBackColor = true;
            this.buttonGenerar.Click += new System.EventHandler(this.buttonGenerar_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(115, 58);
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(203, 22);
            this.dateTimePicker1.TabIndex = 2;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.buttonCancel.Enabled = false;
            this.buttonCancel.Location = new System.Drawing.Point(497, 56);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 28);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancelar";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Visible = false;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "HTTP Inicial";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 34);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "Prefijo";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 62);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 16);
            this.label3.TabIndex = 10;
            this.label3.Text = "Fecha/Hora";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 87);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 16);
            this.label4.TabIndex = 11;
            this.label4.Text = "Cantidad a generar";
            // 
            // textBoxHTTP
            // 
            this.textBoxHTTP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxHTTP.HideSelection = false;
            this.textBoxHTTP.Location = new System.Drawing.Point(115, 7);
            this.textBoxHTTP.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxHTTP.Name = "textBoxHTTP";
            this.textBoxHTTP.Size = new System.Drawing.Size(564, 22);
            this.textBoxHTTP.TabIndex = 0;
            // 
            // textBoxPrefijo
            // 
            this.textBoxPrefijo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxPrefijo.Location = new System.Drawing.Point(115, 32);
            this.textBoxPrefijo.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxPrefijo.Name = "textBoxPrefijo";
            this.textBoxPrefijo.Size = new System.Drawing.Size(564, 22);
            this.textBoxPrefijo.TabIndex = 1;
            // 
            // numericUpDownCantidad
            // 
            this.numericUpDownCantidad.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownCantidad.Location = new System.Drawing.Point(140, 84);
            this.numericUpDownCantidad.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numericUpDownCantidad.Name = "numericUpDownCantidad";
            this.numericUpDownCantidad.Size = new System.Drawing.Size(179, 22);
            this.numericUpDownCantidad.TabIndex = 3;
            this.numericUpDownCantidad.ValueChanged += new System.EventHandler(this.numericUpDownCantidad_ValueChanged);
            // 
            // labelUltimo
            // 
            this.labelUltimo.AutoSize = true;
            this.labelUltimo.Location = new System.Drawing.Point(12, 172);
            this.labelUltimo.Name = "labelUltimo";
            this.labelUltimo.Size = new System.Drawing.Size(0, 16);
            this.labelUltimo.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.label5.Location = new System.Drawing.Point(11, 114);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 16);
            this.label5.TabIndex = 13;
            this.label5.Text = "Tipo de archivo";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Location = new System.Drawing.Point(140, 110);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(177, 24);
            this.comboBox1.TabIndex = 4;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // labelCarpeta
            // 
            this.labelCarpeta.AutoSize = true;
            this.labelCarpeta.Location = new System.Drawing.Point(12, 142);
            this.labelCarpeta.Name = "labelCarpeta";
            this.labelCarpeta.Size = new System.Drawing.Size(55, 16);
            this.labelCarpeta.TabIndex = 14;
            this.labelCarpeta.Text = "Carpeta";
            // 
            // labelCarpetaValue
            // 
            this.labelCarpetaValue.AutoEllipsis = true;
            this.labelCarpetaValue.Location = new System.Drawing.Point(115, 142);
            this.labelCarpetaValue.Name = "labelCarpetaValue";
            this.labelCarpetaValue.Size = new System.Drawing.Size(482, 16);
            this.labelCarpetaValue.TabIndex = 15;
            this.labelCarpetaValue.Text = "Valor carpeta";
            // 
            // buttonCarpeta
            // 
            this.buttonCarpeta.Location = new System.Drawing.Point(614, 134);
            this.buttonCarpeta.Name = "buttonCarpeta";
            this.buttonCarpeta.Size = new System.Drawing.Size(89, 31);
            this.buttonCarpeta.TabIndex = 6;
            this.buttonCarpeta.Text = "Seleccione";
            this.buttonCarpeta.UseVisualStyleBackColor = true;
            this.buttonCarpeta.Click += new System.EventHandler(this.buttonCarpeta_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(715, 254);
            this.Controls.Add(this.buttonCarpeta);
            this.Controls.Add(this.labelCarpetaValue);
            this.Controls.Add(this.labelCarpeta);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.labelUltimo);
            this.Controls.Add(this.numericUpDownCantidad);
            this.Controls.Add(this.textBoxPrefijo);
            this.Controls.Add(this.textBoxHTTP);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.buttonGenerar);
            this.Controls.Add(this.progressBar1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Generado de códigos QR";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCantidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button buttonGenerar;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxHTTP;
        private System.Windows.Forms.TextBox textBoxPrefijo;
        private System.Windows.Forms.NumericUpDown numericUpDownCantidad;
        private System.Windows.Forms.Label labelUltimo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label labelCarpeta;
        private System.Windows.Forms.Label labelCarpetaValue;
        private System.Windows.Forms.Button buttonCarpeta;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

