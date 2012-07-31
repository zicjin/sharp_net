using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zic_dotnet.Repositories {
    public abstract class DomainInt {
        [Key]
        public int ID { get; set; }
        public int ActEnum { get; set; }
        public eAct Act {
            get { return (eAct)ActEnum; }
        }
        public DateTime CreatTime { get; set; }
        public virtual void Creat() {
            CreatTime = DateTime.Now;
            ActEnum = (int)eAct.Normal;
        }
    }

    public abstract class DomainIntData {
        public int ID { get; set; }
        public int ActEnum { get; set; }
        public DateTime CreatTime { get; set; }
    }
}
