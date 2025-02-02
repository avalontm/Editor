using ScintillaNET;
using System.ComponentModel;

namespace Editor
{
    public partial class FrmMain : Form
    {
        // Variables
        string archivo = string.Empty; // Almacena la ruta del archivo
        bool modificado = false; // Indica si el archivo ha sido modificado

        public FrmMain()
        {
            InitializeComponent();
            EditorHelper.Iniciar(textEditor);
            Nuevo();
        }

        //Menus

        private void menuNuevo_Click(object sender, EventArgs e)
        {
            Nuevo();
        }

        private void menuAbrir_Click(object sender, EventArgs e)
        {
            Abrir();
        }

        private void menuGuardar_Click(object sender, EventArgs e)
        {
            Guardar();
        }

        private void menuGuardarComo_Click(object sender, EventArgs e)
        {
            GuardarComo();
        }

        private void menuSalir_Click(object sender, EventArgs e)
        {
            Salir();
        }

        private void menuAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Esta practica estas realizado por:\nJaime Raul Mendez Lopez\nIng. Sistemas Computacionales (5SS)", "Acerca del editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (modificado)
            {
                DialogResult result = MessageBox.Show("�Desea guardar los cambios?", "Guardar", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    Guardar();
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }

            Application.Exit();
        }

        //Funciones

        void Titulo()
        {
            string prefix = string.Empty;

            if (modificado)
            {
                prefix = "*";
            }
            else
            {
                prefix = string.Empty;
            }

            if (string.IsNullOrEmpty(archivo))
            {
                this.Text = $"{prefix}Sin titulo : Block de notas";
            }
            else
            {
                this.Text = $"{prefix}{archivo} : Bloack de notas";
            }
        }

        void Nuevo()
        {
            //C�digo para crear un nuevo archivo
            Titulo();

            //Limpiar el contenido del editor
            textEditor.Clear();
        }

        void Abrir()
        {
            //C�digo para abrir un archivo
            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.No || result == DialogResult.Cancel)
            {
                return;
            }

            archivo = openFileDialog1.FileName;

            if (string.IsNullOrEmpty(archivo))
            {
                return;
            }

            Titulo();

           textEditor.Text = File.ReadAllText(archivo);
        }


        void Guardar()
        {
            //C�digo para guardar un archivo
            if (!string.IsNullOrEmpty(archivo))
            {
                if (File.Exists(archivo))
                {
                    GuadarArchivo();
                    return;
                }
            }

            DialogResult result = saveFileDialog1.ShowDialog();

            if (result == DialogResult.No || result == DialogResult.Cancel)
            {
                return;
            }

            archivo = saveFileDialog1.FileName;

            if (string.IsNullOrEmpty(archivo))
            {
                return;
            }

            Titulo();

            GuadarArchivo();
        }

        void GuardarComo()
        {
            //C�digo para guardar un archivo
            DialogResult result = saveFileDialog1.ShowDialog();

            if (result == DialogResult.No || result == DialogResult.Cancel)
            {
                return;
            }

            archivo = saveFileDialog1.FileName;

            if (string.IsNullOrEmpty(archivo))
            {
                return;
            }

            Titulo();

            GuadarArchivo();
        }

        void Salir()
        {
            //C�digo para salir de la aplicaci�n

            if (modificado)
            {
                DialogResult result = MessageBox.Show("�Desea guardar los cambios?", "Guardar", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    Guardar();
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            Application.Exit();
        }

        void GuadarArchivo()
        {
            try
            {
                File.WriteAllText(archivo, textEditor.Text);
                modificado = false;
                Titulo();
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textEditor_TextChanged(object sender, EventArgs e)
        {
            modificado = true;
            Titulo();
        }
    }
}
