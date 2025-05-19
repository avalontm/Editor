using Editor.Compilador;
using ScintillaNET;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

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
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            EditorHelper.Iniciar(textEditor);
            Nuevo();
        }

        //Menus
        #region Menus

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

        private async void menuCompilar_Click(object sender, EventArgs e)
        {
            await CompilarAsync();
        }

        private async void compilarTraducirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await CompilarAsync("--translate");
        }

        private void menuAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Esta practica esta realizado por:\nJaime Raul Mendez Lopez\nIng. Sistemas Computacionales (5SS)", "Acerca del editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (modificado)
            {
                DialogResult result = MessageBox.Show("¿Desea guardar los cambios?", "Guardar", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

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
        #region Funciones
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
                this.Text = $"{prefix}Sin titulo : Editor";
            }
            else
            {
                this.Text = $"{prefix}{archivo} : Editor";
            }
        }

        void Nuevo()
        {
            archivo = string.Empty;
            //Código para crear un nuevo archivo
            Titulo();

            //Limpiar el contenido del editor
            textEditor.Clear();
        }

        void Abrir()
        {
            //Código para abrir un archivo
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
            //Código para guardar un archivo
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
            //Código para guardar un archivo
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
            //Código para salir de la aplicación

            if (modificado)
            {
                DialogResult result = MessageBox.Show("¿Desea guardar los cambios?", "Guardar", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

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

        private void textEditor_TextChanged(object sender, EventArgs e)
        {
            modificado = true;
            Titulo();
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

        private async Task CompilarAsync(string commandline = "")
        {
            EditorHelper.LimpiarTodasLasMarcas();
            textDebug.Clear();

            try
            {
                string directorioActual = AppDomain.CurrentDomain.BaseDirectory;
                string rutaExe = Path.Combine(directorioActual, "SimpleC.exe");

                var proceso = new Process();
                proceso.StartInfo.FileName = rutaExe;
                proceso.StartInfo.Arguments = $"\"{archivo}\" --external {commandline}";

                proceso.StartInfo.RedirectStandardOutput = true;
                proceso.StartInfo.RedirectStandardError = true;
                proceso.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(850);
                proceso.StartInfo.StandardErrorEncoding = Encoding.GetEncoding(850);
                proceso.StartInfo.UseShellExecute = false;
                proceso.StartInfo.CreateNoWindow = true;

                proceso.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        this.Invoke(() =>
                        {
                            // Inserta texto coloreado (sin prefijos)
                            textDebug.AppendText(e.Data + "\n");
                            Debug.WriteLine($"{e.Data}");
                        });
                    }
                };

                proceso.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        this.Invoke(() =>
                        {
                            textDebug.AppendText(e.Data + "\n");

                        });
                    }
                };

                proceso.Start();

                proceso.BeginOutputReadLine();
                proceso.BeginErrorReadLine();

                await proceso.WaitForExitAsync();
            }
            catch (Exception ex)
            {
                textDebug.AppendText(ex.Message);
            }
        }


        private void OnErrorReturn(int linea)
        {
            EditorHelper.MarcarErrorEnLinea(linea);
            Debug.WriteLine($"MarcarErrorEnLinea: {linea}");
        }

        #endregion
    }
}
 