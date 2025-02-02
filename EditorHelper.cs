using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Editor
{
    public static class EditorHelper
    {

        /// <summary>
        /// Color de fondo del área de texto
        /// </summary>
        const int COLOR_FONDO = 0xFFFFFF;

        /// <summary>
        /// Color del texto por defecto en el área de texto
        /// </summary>
        const int COLOR_TEXTO = 0x000000;

        /// <summary>
        /// Margen para mostrar los números de línea
        /// </summary>
        const int MARGEN_NUMERO = 1;

        /// <summary>
        /// Margen para mostrar los marcadores (bookmarks/breakpoints)
        /// </summary>
        const int MARGEN_MARCADOR = 2;
        const int MARCADOR_INDICE = 2;

        /// <summary>
        /// Margen para mostrar la estructura de plegado de código (+/-)
        /// </summary>
        const int MARGEN_PLEGADO = 3;

        /// <summary>
        /// Define si los botones de plegado de código serán circulares
        /// </summary>
        static bool PLEGADO_CIRCULAR = false;

        static Scintilla? textEditor { set; get; }

        public static void Iniciar(Scintilla editor, bool plegado_circular = false)
        {
            PLEGADO_CIRCULAR = plegado_circular;
            textEditor = editor;
            IniciarEditor();
            ConfigurarMargenNumero();
            ConfigurarMargenMarcador();
            ConfigurarPlegadoCodigo();
        }

        static Color IntToColor(int rgb)
        {
            return Color.FromArgb(255, (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
        }

        static void IniciarEditor()
        {
            // Configure the default style
            textEditor.StyleResetDefault();
            textEditor.Styles[Style.Default].Font = "Consolas";
            textEditor.Styles[Style.Default].Size = 10;
            textEditor.Styles[Style.Default].BackColor = IntToColor(COLOR_FONDO);
            textEditor.Styles[Style.Default].ForeColor = IntToColor(COLOR_TEXTO);
            textEditor.StyleClearAll();

            // Configuración del lenguaje C++ en el editor
            textEditor.Lexer = ScintillaNET.Lexer.Cpp;

            // Aplicar estilos de color similares a los de un IDE para C++
            textEditor.Styles[Style.Cpp.Identifier].ForeColor = IntToColor(0x000000); // Azul para identificadores
            textEditor.Styles[Style.Cpp.Comment].ForeColor = IntToColor(0x008000); // Verde para comentarios
            textEditor.Styles[Style.Cpp.CommentLine].ForeColor = IntToColor(0x008000); // Verde para comentarios de línea
            textEditor.Styles[Style.Cpp.CommentDoc].ForeColor = IntToColor(0x008000); // Verde para comentarios de documentación
            textEditor.Styles[Style.Cpp.Number].ForeColor = IntToColor(0xFF0000); // Rojo para números
            textEditor.Styles[Style.Cpp.String].ForeColor = IntToColor(0xA31515); // Rojo oscuro para cadenas
            textEditor.Styles[Style.Cpp.Character].ForeColor = IntToColor(0xA31515); // Rojo oscuro para caracteres
            textEditor.Styles[Style.Cpp.Preprocessor].ForeColor = IntToColor(0x808080); // Gris para preprocesadores
            textEditor.Styles[Style.Cpp.Operator].ForeColor = IntToColor(0x000000); // Negro para operadores
            textEditor.Styles[Style.Cpp.Regex].ForeColor = IntToColor(0x800080); // Púrpura para expresiones regulares
            textEditor.Styles[Style.Cpp.CommentLineDoc].ForeColor = IntToColor(0x008000); // Verde para comentarios de línea en documentación
            textEditor.Styles[Style.Cpp.Word].ForeColor = IntToColor(0x0000FF); // Azul para palabras clave
            textEditor.Styles[Style.Cpp.Word2].ForeColor = IntToColor(0x0000FF); // Azul para palabras clave secundarias
            textEditor.Styles[Style.Cpp.CommentDocKeyword].ForeColor = IntToColor(0x008000); // Verde para palabras clave en comentarios de documentación
            textEditor.Styles[Style.Cpp.CommentDocKeywordError].ForeColor = IntToColor(0xFF0000); // Rojo para errores en palabras clave en comentarios de documentación
            textEditor.Styles[Style.Cpp.GlobalClass].ForeColor = IntToColor(0x0000FF); // Azul para clases globales

            // Texto normal (código no especificado) en negro
            textEditor.Styles[Style.Cpp.Default].ForeColor = IntToColor(0x000000); // Negro para texto normal

            // Opciones adicionales del editor
            textEditor.SetKeywords(0, "int float double char void bool return if else for while switch case break continue default struct class public private protected static const virtual override namespace using new delete sizeof typedef template inline asm this true false nullptr");
            textEditor.SetKeywords(1, "std cout cin endl string vector map set unordered_map unordered_set pair tuple optional variant any typeid dynamic_cast static_cast reinterpret_cast const_cast noexcept constexpr explicit mutable final override nullptr noexcept");

            textEditor.ResumeLayout();
        }


        static void ConfigurarMargenNumero()
        {
            textEditor.Styles[Style.LineNumber].BackColor = IntToColor(COLOR_FONDO);
            textEditor.Styles[Style.LineNumber].ForeColor = IntToColor(0x606060);
            textEditor.Styles[Style.IndentGuide].ForeColor = IntToColor(0xE0E0E0);

            var margen = textEditor.Margins[MARGEN_NUMERO];
            margen.Width = 20;
            margen.Type = MarginType.Number;
            margen.Sensitive = true;
            margen.Mask = 0;

            textEditor.MarginClick += textEditor_MarginClick;
        }

        static void textEditor_MarginClick(object sender, MarginClickEventArgs e)
        {
            if (e.Margin == MARGEN_MARCADOR)
            {
                const uint mask = (1 << MARCADOR_INDICE);
                var linea = textEditor.Lines[textEditor.LineFromPosition(e.Position)];
                if ((linea.MarkerGet() & mask) > 0)
                {
                    linea.MarkerDelete(MARCADOR_INDICE);
                }
                else
                {
                    linea.MarkerAdd(MARCADOR_INDICE);
                }
            }
        }

        static void ConfigurarMargenMarcador()
        {
            var margen = textEditor.Margins[MARGEN_MARCADOR];
            margen.Width = 20;
            margen.Sensitive = true;
            margen.Type = MarginType.Symbol;
            margen.Mask = (1 << MARCADOR_INDICE);

            var marcador = textEditor.Markers[MARCADOR_INDICE];
            marcador.Symbol = MarkerSymbol.Circle;
            marcador.SetBackColor(IntToColor(0xFF003B)); // Rojo intenso
            marcador.SetForeColor(IntToColor(0x000000)); // Negro
            marcador.SetAlpha(100);
        }

        static void ConfigurarPlegadoCodigo()
        {
            textEditor.SetFoldMarginColor(true, IntToColor(COLOR_FONDO));
            textEditor.SetFoldMarginHighlightColor(true, IntToColor(COLOR_FONDO));

            textEditor.SetProperty("fold", "1");
            textEditor.SetProperty("fold.compact", "1");

            textEditor.Margins[MARGEN_PLEGADO].Type = MarginType.Symbol;
            textEditor.Margins[MARGEN_PLEGADO].Mask = Marker.MaskFolders;
            textEditor.Margins[MARGEN_PLEGADO].Sensitive = true;
            textEditor.Margins[MARGEN_PLEGADO].Width = 20;

            for (int i = 25; i <= 31; i++)
            {
                textEditor.Markers[i].SetForeColor(IntToColor(COLOR_FONDO));
                textEditor.Markers[i].SetBackColor(IntToColor(0x606060));
            }

            textEditor.Markers[Marker.Folder].Symbol = PLEGADO_CIRCULAR ? MarkerSymbol.CirclePlus : MarkerSymbol.BoxPlus;
            textEditor.Markers[Marker.FolderOpen].Symbol = PLEGADO_CIRCULAR ? MarkerSymbol.CircleMinus : MarkerSymbol.BoxMinus;
            textEditor.Markers[Marker.FolderEnd].Symbol = PLEGADO_CIRCULAR ? MarkerSymbol.CirclePlusConnected : MarkerSymbol.BoxPlusConnected;
            textEditor.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            textEditor.Markers[Marker.FolderOpenMid].Symbol = PLEGADO_CIRCULAR ? MarkerSymbol.CircleMinusConnected : MarkerSymbol.BoxMinusConnected;
            textEditor.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            textEditor.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            textEditor.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);
        }
    }
}
