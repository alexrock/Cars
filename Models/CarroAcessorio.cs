using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cars.Models
{
    public class CarroAcessorio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CarroAcessorioId { get; set; }
        public int CarroId { get; set; }
        public int AcessorioId { get; set; }
        [NotMapped]
        public Acessorio Acessorio { get; set; }
    }
}