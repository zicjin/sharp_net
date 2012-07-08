using System;
using System.Collections.Generic;

namespace zic_dotnet {

    public static class Enumhelp {

        public static List<EnumItem> ListTypeForEnum(Type enumType) {
            List<EnumItem> list = new List<EnumItem>();
            foreach (string i in Enum.GetValues(enumType)) {
                list.Add(new EnumItem {
                    Text = Enum.GetName(enumType, i),
                    Value = i
                });
            }
            return list;
        }
    }

    public class EnumItem {

        public string Value { get; set; }

        public string Text { get; set; }
    }
}