using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Compilador
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Lexer
    {
        private string _codigo;
        private int _posicion;

        private static readonly HashSet<string> PalabrasClave = new()
        {
            "int", "return", "if", "else", "while", "for", "void", "char", "float", "double", "struct"
        };

        private static readonly HashSet<char> Operadores = new() { '+', '-', '*', '/', '=', '<', '>', '!' };
        private static readonly HashSet<char> Separadores = new() { '(', ')', '{', '}', '[', ']', ';', ',' };

        public Lexer(string codigo)
        {
            _codigo = codigo;
            _posicion = 0;
        }

        private char VerSiguiente() => _posicion < _codigo.Length ? _codigo[_posicion] : '\0';
        private char Avanzar() => _codigo[_posicion++];

        public List<Token> AnalizarTokens()
        {
            List<Token> tokens = new();

            while (_posicion < _codigo.Length)
            {
                char actual = VerSiguiente();

                if (char.IsWhiteSpace(actual))
                {
                    Avanzar();
                    continue;
                }

                if (actual == '#')  // Directivas de preprocesador (Ej: #include)
                {
                    string directiva = LeerMientras(ch => !char.IsWhiteSpace(ch));
                    tokens.Add(new Token(TipoToken.Preprocesador, directiva));
                    continue;
                }

                if (char.IsLetter(actual) || actual == '_')  // Identificadores, palabras clave y funciones
                {
                    string palabra = LeerMientras(char.IsLetterOrDigit);

                    if (PalabrasClave.Contains(palabra))
                    {
                        tokens.Add(new Token(TipoToken.PalabraClave, palabra));
                    }
                    else if (VerSiguiente() == '(') // Llamada a función
                    {
                        tokens.Add(new Token(TipoToken.LlamadaFuncion, palabra));
                    }
                    else
                    {
                        tokens.Add(new Token(TipoToken.Identificador, palabra));
                    }
                    continue;
                }

                if (char.IsDigit(actual))  // Números
                {
                    string numero = LeerMientras(char.IsDigit);
                    tokens.Add(new Token(TipoToken.Numero, numero));
                    continue;
                }

                if (Operadores.Contains(actual))  // Operadores
                {
                    string operador = LeerMientras(ch => Operadores.Contains(ch));
                    tokens.Add(new Token(TipoToken.Operador, operador));
                    continue;
                }

                if (Separadores.Contains(actual))  // Separadores
                {
                    tokens.Add(new Token(TipoToken.Separador, actual.ToString()));
                    Avanzar();
                    continue;
                }

                if (actual == '"')  // Cadenas de texto
                {
                    Avanzar(); // Omitir la primera comilla
                    string cadena = LeerMientras(ch => ch != '"' && ch != '\0');
                    Avanzar(); // Omitir la última comilla
                    tokens.Add(new Token(TipoToken.CadenaTexto, cadena));
                    continue;
                }

                if (actual == '/' && VerSiguienteSiguiente() == '/')  // Comentarios de una línea
                {
                    LeerMientras(ch => ch != '\n');
                    tokens.Add(new Token(TipoToken.Comentario, "// Comentario"));
                    continue;
                }

                if (actual == '/' && VerSiguienteSiguiente() == '*')  // Comentarios multilínea
                {
                    Avanzar(); Avanzar(); // Omitir "/*"
                    LeerMientras(ch => !(VerSiguiente() == '*' && VerSiguienteSiguiente() == '/'));
                    Avanzar(); Avanzar(); // Omitir "*/"
                    tokens.Add(new Token(TipoToken.Comentario, "/* Comentario */"));
                    continue;
                }

                tokens.Add(new Token(TipoToken.Desconocido, actual.ToString()));
                Avanzar();
            }

            tokens.Add(new Token(TipoToken.FinArchivo, ""));
            return tokens;
        }

        private string LeerMientras(Func<char, bool> condicion)
        {
            StringBuilder resultado = new();
            while (_posicion < _codigo.Length && condicion(VerSiguiente()))
            {
                resultado.Append(Avanzar());
            }
            return resultado.ToString();
        }

        private char VerSiguienteSiguiente() => _posicion + 1 < _codigo.Length ? _codigo[_posicion + 1] : '\0';
    }
}
