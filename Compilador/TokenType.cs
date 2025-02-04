using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Compilador
{
    public enum TipoToken
    {
        PalabraClave,            // Palabras clave del lenguaje (int, return, if, else, while, etc.)
        Identificador,           // Nombres de variables, funciones y otros identificadores válidos
        Numero,                  // Números enteros o decimales (123, 3.14, -5)
        Operador,                // Operadores matemáticos y lógicos (+, -, *, /, =, <, >, etc.)
        Separador,               // Caracteres separadores como paréntesis, llaves, corchetes, punto y coma, coma
        CadenaTexto,             // Literales de cadena entre comillas ("Hola Mundo")
        Comentario,              // Comentarios de línea (//) y de bloque (/* ... */)
        DirectivaPreprocesador,  // Directivas del preprocesador (#include, #define, #ifdef, #endif, etc.)
        ArchivoIncluido,         // Archivo especificado en #include ("archivo.h" o <archivo.h>)
        Preprocesador,           // Representa elementos relacionados con el preprocesador en general
        LlamadaFuncion,           // Identifica llamadas a funciones dentro del código
        Desconocido,             // Caracteres no reconocidos o inválidos
        FinArchivo              // Indica el final del código fuente
    }
}
