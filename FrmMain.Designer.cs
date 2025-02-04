namespace Editor
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            menuStrip1 = new MenuStrip();
            toolStripMenuItem1 = new ToolStripMenuItem();
            menuNuevo = new ToolStripMenuItem();
            menuAbrir = new ToolStripMenuItem();
            menuGuardar = new ToolStripMenuItem();
            menuGuardarComo = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            menuSalir = new ToolStripMenuItem();
            ayudaToolStripMenuItem = new ToolStripMenuItem();
            menuAbout = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            toolStripStatusLabel2 = new ToolStripStatusLabel();
            openFileDialog1 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            textEditor = new ScintillaNET.Scintilla();
            compilardorToolStripMenuItem = new ToolStripMenuItem();
            menuCompilar = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, compilardorToolStripMenuItem, ayudaToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { menuNuevo, menuAbrir, menuGuardar, menuGuardarComo, toolStripSeparator1, menuSalir });
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(60, 20);
            toolStripMenuItem1.Text = "Archivo";
            // 
            // menuNuevo
            // 
            menuNuevo.Name = "menuNuevo";
            menuNuevo.Size = new Size(161, 22);
            menuNuevo.Text = "Nuevo";
            menuNuevo.Click += menuNuevo_Click;
            // 
            // menuAbrir
            // 
            menuAbrir.Name = "menuAbrir";
            menuAbrir.Size = new Size(161, 22);
            menuAbrir.Text = "Abrir";
            menuAbrir.Click += menuAbrir_Click;
            // 
            // menuGuardar
            // 
            menuGuardar.Name = "menuGuardar";
            menuGuardar.Size = new Size(161, 22);
            menuGuardar.Text = "Guardar";
            menuGuardar.Click += menuGuardar_Click;
            // 
            // menuGuardarComo
            // 
            menuGuardarComo.Name = "menuGuardarComo";
            menuGuardarComo.Size = new Size(161, 22);
            menuGuardarComo.Text = "Guardar Como...";
            menuGuardarComo.Click += menuGuardarComo_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(158, 6);
            // 
            // menuSalir
            // 
            menuSalir.Name = "menuSalir";
            menuSalir.Size = new Size(161, 22);
            menuSalir.Text = "Salir";
            menuSalir.Click += menuSalir_Click;
            // 
            // ayudaToolStripMenuItem
            // 
            ayudaToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { menuAbout });
            ayudaToolStripMenuItem.Name = "ayudaToolStripMenuItem";
            ayudaToolStripMenuItem.Size = new Size(53, 20);
            ayudaToolStripMenuItem.Text = "Ayuda";
            // 
            // menuAbout
            // 
            menuAbout.Name = "menuAbout";
            menuAbout.Size = new Size(163, 22);
            menuAbout.Text = "Acerca del Editor";
            menuAbout.Click += menuAbout_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1, toolStripStatusLabel2 });
            statusStrip1.Location = new Point(0, 428);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(800, 22);
            statusStrip1.TabIndex = 3;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(94, 17);
            toolStripStatusLabel1.Text = "Windows (CRLF)";
            // 
            // toolStripStatusLabel2
            // 
            toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            toolStripStatusLabel2.Size = new Size(38, 17);
            toolStripStatusLabel2.Text = "UTF-8";
            // 
            // openFileDialog1
            // 
            openFileDialog1.DefaultExt = "*.c";
            openFileDialog1.Filter = "Codigo fuente (c++)|*.c";
            // 
            // saveFileDialog1
            // 
            saveFileDialog1.DefaultExt = "*.c";
            saveFileDialog1.Filter = "Codigo fuente (c++)|*.c";
            // 
            // textEditor
            // 
            textEditor.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textEditor.AutoCMaxHeight = 9;
            textEditor.Location = new Point(0, 27);
            textEditor.Name = "textEditor";
            textEditor.Size = new Size(800, 398);
            textEditor.TabIndex = 4;
            // 
            // compilardorToolStripMenuItem
            // 
            compilardorToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { menuCompilar });
            compilardorToolStripMenuItem.Name = "compilardorToolStripMenuItem";
            compilardorToolStripMenuItem.Size = new Size(82, 20);
            compilardorToolStripMenuItem.Text = "Compilador";
            // 
            // menuCompilar
            // 
            menuCompilar.Name = "menuCompilar";
            menuCompilar.Size = new Size(180, 22);
            menuCompilar.Text = "Compilar";
            menuCompilar.Click += menuCompilar_Click;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(textEditor);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "FrmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sin titulo: Block de notas";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip menuStrip1;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem menuNuevo;
        private ToolStripMenuItem menuAbrir;
        private ToolStripMenuItem menuGuardar;
        private ToolStripMenuItem menuGuardarComo;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem menuSalir;
        private ToolStripMenuItem ayudaToolStripMenuItem;
        private ToolStripMenuItem menuAbout;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
        private ScintillaNET.Scintilla textEditor;
        private ToolStripMenuItem compilardorToolStripMenuItem;
        private ToolStripMenuItem menuCompilar;
    }
}
