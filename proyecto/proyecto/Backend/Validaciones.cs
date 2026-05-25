using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace proyecto
{
    public static class Validaciones
    {
        // Carreras oficiales del tecnológico
        public static readonly string[] CarrerasPermitidas = new string[]
        {
            "INGENIERIA EN SISTEMAS COMPUTACIONALES",
            "INGENIERIA INDUSTRIAL",
            "INGENIERIA MECATRONICA",
            "INGENIERIA CIVIL",
            "INGENIERIA EN GESTION EMPRESARIAL",
            "LICENCIATURA EN ADMINISTRACION",
            "LICENCIATURA EN ARQUITECTURA"
        };



        public static bool ValidarCamposAlumno(string numControl, string nombre, string apellido, string carrera, string correo, string semestre, string grupo, bool esEdicion = false)
        {
            if (string.IsNullOrWhiteSpace(numControl))
            { Mensaje("El número de control es obligatorio."); return false; }

            if (!Regex.IsMatch(numControl.Trim(), @"^\d{8}$"))
            { Mensaje("El número de control debe tener exactamente 8 dígitos (sólo números, ni más ni menos)."); return false; }

            if (!ValidarNombreApellido(nombre, "Nombre(s)")) return false;
            if (!ValidarNombreApellido(apellido, "Apellidos")) return false;

            if (string.IsNullOrWhiteSpace(carrera))
            { Mensaje("La carrera es obligatoria."); return false; }

            bool carreraValida = false;
            foreach (string c in CarrerasPermitidas)
                if (c.Equals(carrera.Trim().ToUpper(), StringComparison.OrdinalIgnoreCase)) { carreraValida = true; break; }
            if (!carreraValida)
            { Mensaje("La carrera seleccionada no es válida. Selecciona una carrera de la lista."); return false; }

            if (string.IsNullOrWhiteSpace(correo) || !Regex.IsMatch(correo.Trim(), @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            { Mensaje("El correo electrónico no es válido. Ingresa un formato real (ejemplo: usuario@dominio.com)."); return false; }

            int sem;
            if (!int.TryParse(semestre, out sem) || sem < 1 || sem > 14)
            { Mensaje("El semestre debe ser un número entre 1 y 14."); return false; }

            if (string.IsNullOrWhiteSpace(grupo))
            { Mensaje("El grupo es obligatorio."); return false; }

            if (!Regex.IsMatch(grupo.Trim(), "^" + sem + "[A-Za-z]{2}$"))
            { Mensaje($"El grupo debe ser el número {sem} seguido de exactamente 2 letras (ej: {sem}US)."); return false; }

            if (!esEdicion && BaseDeDatos.ExisteAlumno(numControl.Trim()))
            { Mensaje("¡Ese número de control ya está registrado! No puedes ingresar dos alumnos con el mismo número de control."); return false; }

            return true;
        }

        /// <summary>
        /// Valida el RFC de persona física: 13 caracteres con formato AAAA######AAA
        /// 4 letras + 6 dígitos (AAMMDD) + 3 alfanuméricos (homoclave)
        /// </summary>
        public static bool ValidarRFC(string rfc)
        {
            if (string.IsNullOrWhiteSpace(rfc)) return false;
            string rfcTrim = rfc.Trim().ToUpper();
            // Formato: 4 letras + 6 dígitos + 3 alfanuméricos = 13 caracteres
            return Regex.IsMatch(rfcTrim, @"^[A-Z]{4}\d{6}[A-Z0-9]{3}$");
        }

        public static bool ValidarCamposProfesor(string idProfesor, string rfc, string nombre, string apellido, string telefono, string correo, string sexo, bool esEdicion = false)
        {
            if (string.IsNullOrWhiteSpace(idProfesor))
            { Mensaje("El ID del profesor es obligatorio."); return false; }

            int id;
            if (!int.TryParse(idProfesor, out id) || id <= 0)
            { Mensaje("El ID del profesor debe ser un número mayor a cero."); return false; }

            if (string.IsNullOrWhiteSpace(rfc))
            { Mensaje("El RFC es obligatorio."); return false; }

            if (rfc.Trim().Length != 13)
            { Mensaje("El RFC debe tener exactamente 13 caracteres (ej: GODE561231GR8)."); return false; }

            if (!ValidarRFC(rfc))
            { Mensaje("El RFC no tiene un formato válido.\nFormato: 4 letras + 6 dígitos (AAMMDD) + 3 alfanuméricos.\nEjemplo: GODE561231GR8"); return false; }

            int idExcluir = esEdicion ? id : 0;
            if (BaseDeDatos.ExisteRFCProfesor(rfc, idExcluir))
            { Mensaje("El RFC " + rfc.Trim().ToUpper() + " ya está registrado en otro profesor. Verifica el RFC ingresado."); return false; }

            if (!ValidarNombreApellido(nombre, "Nombre(s)")) return false;
            if (!ValidarNombreApellido(apellido, "Apellidos")) return false;

            if (string.IsNullOrWhiteSpace(telefono))
            { Mensaje("El teléfono es obligatorio."); return false; }

            if (!Regex.IsMatch(telefono.Trim(), @"^\d{10}$"))
            { Mensaje("El teléfono debe tener exactamente 10 dígitos numéricos."); return false; }

            if (string.IsNullOrWhiteSpace(correo) || !Regex.IsMatch(correo.Trim(), @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            { Mensaje("El correo electrónico no es válido. Ingresa un formato real (ejemplo: usuario@dominio.com)."); return false; }

            if (string.IsNullOrWhiteSpace(sexo))
            { Mensaje("El sexo es obligatorio."); return false; }

            if (sexo.Trim() != "Hombre" && sexo.Trim() != "Mujer")
            { Mensaje("El sexo debe ser 'Hombre' o 'Mujer'."); return false; }

            return true;
        }

        /// <summary>
        /// Valida que un campo de nombre/apellido tenga casing consistente por palabra
        /// y no tenga espacios consecutivos.
        /// </summary>
        public static bool ValidarNombreApellido(string valor, string campo)
        {
            if (string.IsNullOrWhiteSpace(valor))
            { Mensaje($"El campo {campo} es obligatorio."); return false; }

            if (valor.Length > 30)
            { Mensaje($"El campo {campo} no puede tener más de 30 caracteres."); return false; }

            if (valor.Contains("  "))
            { Mensaje($"El campo {campo} no puede tener espacios consecutivos.\nEjemplo válido: 'Ameli Janeth'"); return false; }

            string v = valor.Trim();
            if (!Regex.IsMatch(v, @"^[Á-Úá-úñÑA-Za-z]+([\s][Á-Úá-úñÑA-Za-z]+)*$"))
            { Mensaje($"El campo {campo} solo debe contener letras y espacios simples (sin números ni caracteres especiales)."); return false; }

            foreach (string word in v.Split(' '))
            {
                if (string.IsNullOrEmpty(word)) continue;
                bool allLower = word == word.ToLower();
                bool allUpper = word == word.ToUpper();
                bool titleCase = char.IsUpper(word[0]) && (word.Length == 1 || word.Substring(1) == word.Substring(1).ToLower());
                if (!allLower && !allUpper && !titleCase)
                { Mensaje($"En el campo {campo}, la palabra '{word}' mezcla mayúsculas y minúsculas incorrectamente.\nEjemplos válidos: 'ameli', 'Ameli', 'AMELI'"); return false; }
            }
            return true;
        }

        public static bool ConfirmarEliminacion(string entidad = "registro")
        {
            DialogResult result = MessageBox.Show(
                "¿Estás seguro que deseas eliminar este " + entidad + "?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            return result == DialogResult.Yes;
        }

        private static void Mensaje(string texto)
        {
            MessageBox.Show(texto, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
