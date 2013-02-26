using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharp_net.Repositories {

    [SerializableAttribute()]
    public abstract class DomainGuid {
        [Key]
        public Guid Id { get; set; }
        public eAct Act{ get; set; }

        public DateTime CreatTime { get; set; }
        public virtual void Creat() {
            Id = Guid.NewGuid();
            CreatTime = DateTime.Now;
            Act = eAct.Normal;
        }
    }

}
