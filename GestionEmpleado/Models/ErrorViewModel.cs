using GestionEmpleado.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestionEmpleado.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    public class Usuario
    {
        public string Nombre { get; set; }
        [Required(ErrorMessage ="El correo es requerido")]
        [EmailAddress(ErrorMessage = "El correo debe ser válido")]
        public string Correo { get; set; }
        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Password { get; set; }
        public List<Rol> Roles { get; set; }
    }
}
