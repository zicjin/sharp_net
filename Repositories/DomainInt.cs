using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharp_net.Repositories {
    public abstract class DomainInt {
        [Key]
        public int Id { get; set; }
        public eAct Act { get; set; }

        public DateTime CreatTime { get; set; }
        public virtual void Creat() {
            CreatTime = DateTime.Now;
            Act = eAct.Normal;
        }
    }
}
