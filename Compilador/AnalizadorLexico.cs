using System.Text;

namespace Editor.Compilador
{
    public class AnalizadorLexico
    {
        private string _codigo;
        private int _posicion;
        private int _linea;
        private int _columna;

        private static readonly HashSet<string> PalabrasClave = new()
            {
                "int", "return", "if", "else", "while", "for", "void", "char", "float", "double", "struct"
            };

        private static readonly HashSet<char> Operadores = new() { '+', '-', '*', '/', '=', '<', '>', '!' };
        private static readonly HashSet<char> Separadores = new() { '(', ')', '{', '}', '[', ']', ';', ',' };

        public Action<int>? ErrorReturn { get; set; }

        public AnalizadorLexico(string codigo)
        {
            _codigo = codigo;
            _posicion = 0;
            _linea = 1;
            _columna = 1;
        }

        private char VerSiguiente() => _posicion < _codigo.Length ? _codigo[_posicion] : '\0';
        private char Avanzar()
        {
            char actual = _codigo[_posicion++];
            if (actual == '\r' && VerSiguiente() == '\n')
            {
                _posicion++; // Omitir '\n' después de '\r'
                _linea++;
                _columna = 1;
            }
            else if (actual == '\n')
            {
                _linea++;
                _columna = 1;
            }
            else
            {
                _columna++;
            }
            return actual;
        }

        private char VerSiguienteSiguiente() => _posicion + 1 < _codigo.Length ? _codigo[_posicion + 1] : '\0';

        private string LeerMientras(Func<char, bool> condicion)
        {
            StringBuilder resultado = new();
            while (_posicion < _codigo.Length && condicion(VerSiguiente()))
            {
                resultado.Append(Avanzar());
            }
            return resultado.ToString();
        }

        public List<Token> AnalizarTokens()
        {
            List<Token> tokens = new();

            try
            {
                while (_posicion < _codigo.Length)
                {
                    char actual = VerSiguiente();

                    if (char.IsWhiteSpace(actual))
                    {
                        Avanzar();
                        continue;
                    }

                    if (actual == '#')  // Directivas de preprocesador
                    {
                        Avanzar(); // Omitimos el '#'

                        // Leemos el nombre de la directiva (ej. "include", "define")
                        string directiva = LeerMientras(char.IsLetter);

                        // Si la directiva es "include", capturamos el archivo o librería entre <> o ""
                        if (directiva == "include")
                        {
                            LeerMientras(char.IsWhiteSpace); // Saltamos espacios

                            char delimitador = VerSiguiente();
                            if (delimitador == '<' || delimitador == '"')
                            {
                                char _delimitador1 = Avanzar(); // Omitimos '<' o '"'
                                string contenido = LeerMientras(ch => ch != '>' && ch != '"' && ch != '\n' && ch != '\r');

                                // Si encontramos un salto de línea antes de cerrar el delimitador, es un error
                                if (VerSiguiente() == '\n' || VerSiguiente() == '\r')
                                {
                                    tokens.Add(new Token(TipoToken.Error, $"Error: Directiva #include no cerrada en la misma línea (línea {_linea}, carácter {_columna})"));
                                    ErrorReturn?.Invoke(_linea);
                                    break;
                                }

                                // Verificamos que se cierre correctamente con '>' o '"'
                                if (VerSiguiente() == '>' || VerSiguiente() == '"')
                                {
                                    char _delimitador2 = Avanzar(); // Omitimos '>' o '"'
                                    tokens.Add(new Token(TipoToken.Preprocesador, $"#include {_delimitador1}{contenido}{_delimitador2}"));
                                }
                                else
                                {
                                    tokens.Add(new Token(TipoToken.Error, $"Error: Falta cierre en directiva #include en línea {_linea}, carácter {_columna}"));
                                    ErrorReturn?.Invoke(_linea);
                                    break;
                                }
                            }
                            else
                            {
                                tokens.Add(new Token(TipoToken.Error, $"Error: Sintaxis incorrecta en #include en línea {_linea}, carácter {_columna}"));
                                ErrorReturn?.Invoke(_linea);
                                break;
                            }
                        }
                        else
                        {
                            // Capturamos la línea completa para otras directivas como #define, #pragma
                            string contenido = LeerMientras(ch => ch != '\n' && ch != '\r');
                            tokens.Add(new Token(TipoToken.Preprocesador, $"#{directiva} {contenido}"));
                        }
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

                    if (actual == '/' && VerSiguienteSiguiente() == '/')  // Comentarios de una línea
                    {
                        Avanzar(); Avanzar(); // Omitir "//"
                        string comentario = LeerMientras(ch => ch != '\n' && ch != '\r'); // Ahora se detiene al final de la línea
                        tokens.Add(new Token(TipoToken.Comentario, $"// {comentario}"));
                        continue;
                    }

                    if (actual == '/' && VerSiguienteSiguiente() == '*')  // Comentarios multilínea
                    {
                        Avanzar(); Avanzar(); // Omitir "/*"
                        string comentario = LeerMientras(ch => !(VerSiguiente() == '*' && VerSiguienteSiguiente() == '/'));
                        Avanzar(); Avanzar(); // Omitir "*/"
                        tokens.Add(new Token(TipoToken.Comentario, $"/* {comentario} */"));
                        continue;
                    }

                    if (actual == '"')  // Cadenas de texto
                    {
                        Avanzar(); // Omitir la primera comilla
                        string cadena = LeerMientras(ch => ch != '"' && ch != '\0' && ch != '\n' && ch != '\r'); // Detenerse en la comilla de cierre o un salto de línea

                        if (VerSiguiente() != '"')  // Verifica si falta la comilla de cierre
                        {
                            tokens.Add(new Token(TipoToken.Error, $"Error: cadena de texto no cerrada en línea {_linea}, carácter {_columna}"));
                            ErrorReturn?.Invoke(_linea);
                            break;
                        }

                        // Si la cadena contiene saltos de línea
                        if (cadena.Contains("\n") || cadena.Contains("\r"))
                        {
                            tokens.Add(new Token(TipoToken.Error, $"Error: cadena de texto contiene un salto de línea en línea {_linea}, carácter {_columna}"));
                            ErrorReturn?.Invoke(_linea);
                            break;
                        }

                        Avanzar(); // Omitir la última comilla
                        tokens.Add(new Token(TipoToken.CadenaTexto, cadena));
                        continue;
                    }

                    if (char.IsDigit(actual) || actual == '.')  // Números y decimales
                    {
                        string numero = LeerMientras(ch => char.IsDigit(ch) || ch == '.');

                        // Verificar si el número contiene más de un punto decimal (esto es un error)
                        if (numero.Count(c => c == '.') > 1)
                        {
                            tokens.Add(new Token(TipoToken.Error, $"Error: número con más de un punto decimal en en línea {_linea}, carácter {_columna}"));
                            ErrorReturn?.Invoke(_linea);
                            break;
                        }

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

                    // Verificación de punto y coma faltante
                    if (actual == ';') // Verifica si el punto y coma está presente después de una declaración
                    {
                        Avanzar();
                        continue;
                    }

                    if (actual == '\n' || actual == '\r') // Fin de línea o salto de línea
                    {
                        // Verificar si la línea anterior terminó con un punto y coma, solo si es una declaración
                        if (tokens.Count > 0)
                        {
                            var ultimoToken = tokens.Last();
                            if (ultimoToken.Type == TipoToken.Identificador ||
                                ultimoToken.Type == TipoToken.Numero ||
                                ultimoToken.Type == TipoToken.LlamadaFuncion ||
                                ultimoToken.Type == TipoToken.Operador)
                            {
                                if (tokens.Count > 1 && tokens[tokens.Count - 2].Value != ";")
                                {
                                    tokens.Add(new Token(TipoToken.Error, $"Error: punto y coma faltante en línea {_linea}, carácter {_columna}"));
                                    ErrorReturn?.Invoke(_linea);
                                    break;
                                }
                            }
                        }
                    }

                    tokens.Add(new Token(TipoToken.Desconocido, actual.ToString()));
                    Avanzar();
                }

                tokens.Add(new Token(TipoToken.FinArchivo, ""));
            }
            catch (Exception ex)
            {
                tokens.Add(new Token(TipoToken.Error, $"Error: {ex.Message} en línea {_linea}, carácter {_columna}"));
                ErrorReturn?.Invoke(_linea);
            }

            return tokens;
        }
    }
}
