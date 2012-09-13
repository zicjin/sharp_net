using System;
using System.Collections.Generic;
using System.Web.UI;

namespace sharp_net.Winform {

    internal static class ExpendWinform {

        //Usage
        //禁用所有Button：
        //this.GetControls<Button>(null).ForEach(b => b.Enabled = false);
        //反选groupBox1中CheckBox：
        //this.GetControls<CheckBox>(c => c.Parent == groupBox1).ForEach(c => c.Checked = !c.Checked);
        //将label1的前景色设为红色：
        //this.GetControls<Label>(l => l.Name == "label1").FirstOrDefault().ForeColor = Color.Red;
        public static IEnumerable<T> GetControls<T>(this Control control, Func<T, bool> filter) where T : Control {
            foreach (Control c in control.Controls) {
                if (c is T) {
                    T t = c as T;
                    if (filter != null) {
                        if (filter(t)) {
                            yield return t;
                        } else {
                            foreach (T _t in GetControls<T>(c, filter))
                                yield return _t;
                        }
                    } else
                        yield return t;
                } else {
                    foreach (T _t in GetControls<T>(c, filter))
                        yield return _t;
                }
            }
        }
    }
}