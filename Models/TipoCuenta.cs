using ManejoPresupuesto.Validaciones;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
    public class TipoCuenta : IValidatableObject
    {
        public int Id { get; set; }

        [Required (ErrorMessage = "{0} es requerido")]
        [StringLength(maximumLength:50,MinimumLength = 3,ErrorMessage = "La longitud del campo {0} es de mínimo 3 caracteres y máximo 50.")]
        [Display(Name = "Nombre del tipo de cuenta")]
        [Remote(action:"VerificarExisteTipoCuenta", controller:"TiposCuentas")]
        //[PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        public int UsuarioId { get; set; }
        public int Orden { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Nombre != null && Nombre.Length>0)
            {
                var primeraLetra = Nombre[0].ToString();

                if(primeraLetra != primeraLetra.ToUpper())
                    {
                    yield return new ValidationResult("La primera letra debe ser mayúscula",
                        new[] { nameof(Nombre) });
                }
            }   
        }

        /*Pruebas de otras validaciones por defecto*/

        //[Required(ErrorMessage = "{0} es requerido")]
        //[EmailAddress(ErrorMessage = "El campo {0} debe tener un correo electrónico válido")]
        //public string Email { get; set; }

        //[Range(minimum:18, maximum:130, ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]

        //public int Edad { get; set; }
        //[Url(ErrorMessage = "El campo {0} debe ser una URL válida")]
        //public string URL { get; set; }
        //[CreditCard(ErrorMessage = "La tarjeta de crédito no es válida")]
        //[Display(Name = "Tarjeta de crédito")]
        //public string TarjetaDeCredito { get; set; }

    }
}
