using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zic_dotnet {
    public static class Enumhelp {
        public static List<EnumItem> ListTypeForEnum(Type enumType) {
            List<EnumItem> list = new List<EnumItem>();
            foreach (int i in Enum.GetValues(enumType)) {
                list.Add(new EnumItem {
                    Text = Enum.GetName(enumType, i),
                    Value = i
                });
            }
            return list;
        }
    }

    public class EnumItem {
        public int Value { get; set; }
        public string Text { get; set; }
    }
}
