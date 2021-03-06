﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace sharp_net.Mvc {
    public class HttpContextLife<T> : LifetimeManager, IDisposable {
        public override object GetValue() {
            return HttpContext.Current.Items[typeof(T).AssemblyQualifiedName];
        }
        public override void RemoveValue() {
            HttpContext.Current.Items.Remove(typeof(T).AssemblyQualifiedName);
        }
        public override void SetValue(object newValue) {
            HttpContext.Current.Items[typeof(T).AssemblyQualifiedName] = newValue;
        }
        public void Dispose() {
            RemoveValue();
        }
    }
}
