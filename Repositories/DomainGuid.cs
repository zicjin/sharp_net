using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zic_dotnet.Repositories {
    public abstract class DomainGuid {
        [Key]
        public Guid ID { get; set; }
        public int ActEnum { get; set; }
        public eAct Act {
            get { return (eAct)ActEnum; }
            set { ActEnum = (int)value; }
        }
        public DateTime CreatTime { get; set; }
        public virtual void Creat() {
            ID = Guid.NewGuid();
            CreatTime = DateTime.Now;
            Act = eAct.Normal;
        }
        [Timestamp]
        public Byte[] Timestamp { get; set; }
    }

    public abstract class DomainGuidData {
        public string ID { get; set; }
        public int ActEnum { get; set; }
        public DateTime CreatTime { get; set; }
        public Byte[] Timestamp { get; set; }
    }
}
