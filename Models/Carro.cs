using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cars.Models
{
    public enum Cor
    {
        Amarelo,
        Azul,
        Branco,
        Prata,
        Preto,
        Vermelho
    }

    public class Carro
    {
        public static readonly IEnumerable<SelectListItem> Cores =
            from Cor cor in Enum.GetValues(typeof(Cor))
            select new SelectListItem {
                Text = cor.ToString(),
                Value = ((int)cor).ToString()
            };

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CarroId { get; set; }
        [MaxLength(50)]
        [Required]
        public string Marca { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataCompra { get; set; }
        public string Descricao { get; set; }
        public Cor Cor { get; set; }

        [NotMapped]
        public List<CarroAcessorio> CarroAcessorios { get; set; }
        [NotMapped]
        public List<Acessorio> Acessorios { get; set; }
    }
}