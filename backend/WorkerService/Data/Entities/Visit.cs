using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkerService.Entities
{
    public class Visit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int id { get; set; }
        public Barber barber { get; set; }
        public string date { get; set; }
        public int hour { get; set; }
        public Client client { get; set; }
    }
}
