using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Junto.Models
{
    public class Usuario
    {
        public int idUsuario = 0;

        public string Message { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [DisplayName("Matricula")]
        [StringLength(7,MinimumLength = 7,ErrorMessage ="Matricula deve conter 7 caracteres!")]
        public string strMatricula { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Senha")]
        public string strSenha { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(1000, MinimumLength = 5, ErrorMessage = "E-mail deve conter no minimo 5 caracteres!")]
        [DisplayName("E-mail")]
        public string strEmail { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(1000, MinimumLength = 5, ErrorMessage = "Nome deve conter no minimo 5 caracteres!")]
        [DisplayName("Nome")]
        public string strNome { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "token deve conter 6 caracteres!")]
        [DisplayName("Token")]
        public string strToken { get; set; }


    }
}