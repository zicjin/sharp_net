﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace sharp_net.Repositories {
    public abstract class DomainMongo {
        [Key]
        public ObjectId Id { get; set; }
        public int ActEnum { get; set; }
        public eAct Act {
            get { return (eAct)ActEnum; }
            set { ActEnum = (int)value; }
        }

        public DateTime CreatTime { get; set; }
        public virtual void Creat() {
            CreatTime = DateTime.Now;
            ActEnum = (int)eAct.Normal;
        }

        protected void Update(DomainMongoData data) {
            if (data.ActEnum != 0)
                this.ActEnum = data.ActEnum;
        }
    }

    public abstract class DomainMongoData {
        public string Id { get; set; }
        public int ActEnum { get; set; }
        public DateTime CreatTime { get; set; }
    }
}
