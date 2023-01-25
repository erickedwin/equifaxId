using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace equifaxIdWin.Models
{
    public class equifaxTabla
    {
        [Table("t_equifax")]
        public class equifaxAws
        {
            [Key]
            public int id { get; set; }
            public string? ruc { get; set; }
            public string? ce { get; set; }
            public string? dni { get; set; }
            public string? nombre { get; set; }
            public string? nombre1 { get; set; }
            public string? ape_pat { get; set; }
            public string? ape_mat { get; set; }
            public string? score { get; set; }
            public string? trama_completa { get; set; }
            public DateTime? crea_data { get; set; }
            public string? origen { get; set; }
            public DateTime? crea_data_ruc { get; set; }
        }
        
        [Table("t_reglas")]
        public class intermediaMensaje
        {
            [Key]
            public int id { get; set; }
            public string? codigo { get; set; }
            public string regla { get; set; }
        }
        [Keyless]
        public class awsdataequi
        {
            public string? ruc { get; set; }
            public string? ce { get; set; }
            public string? dni { get; set; }
        }
    }
}
