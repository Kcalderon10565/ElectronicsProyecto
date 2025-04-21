using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Electronics.Models
{
    //Acá se visualiza la estructura del Checkout, y los datos de pago.
    public class CheckoutViewModel
    {
        [ValidateNever]
        public IEnumerable<Carrito> Items { get; set; }

        [Display(Name = "Total a Pagar")]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre y Apellidos")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "El número de tarjeta es obligatorio")]
        [Display(Name = "Número de Tarjeta")]
        [StringLength(16, MinimumLength = 16,
            ErrorMessage = "Debe tener exactamente 16 dígitos")]
        [RegularExpression(@"^\d{16}$",
            ErrorMessage = "Sólo dígitos, sin espacios")]
        public string NumeroTarjeta { get; set; }

        [Required(ErrorMessage = "La fecha de expiración es obligatoria")]
        [Display(Name = "Fecha de Expiración")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{4}$",
            ErrorMessage = "Formato MM/YYYY")]
        public string FechaVencimiento { get; set; }

        [Required(ErrorMessage = "El CCV es obligatorio")]
        [Display(Name = "CCV")]
        [StringLength(3, MinimumLength = 3,
            ErrorMessage = "Debe tener 3 dígitos")]
        [RegularExpression(@"^\d{3}$",
            ErrorMessage = "Sólo dígitos")]
        public string CCV { get; set; }
    }
}
