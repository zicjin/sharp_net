using System;

namespace sharp_net {

    internal static class ExpendObj {

        public static T ParseTo<T>(this object input) {
            object tmp = ChangeType(input, typeof(T));
            if (tmp == null) return default(T);
            return (T)tmp;
        }

        public static object ChangeType(this object input, Type type) {
            if (input == null) return null;
            if (type.IsInstanceOfType(input))
                return input;

            try {
                if (type.IsEnum)
                    return Enum.Parse(type, input.ToString(), true);
                else
                    return Convert.ChangeType(input, type);
            } catch { }
            return null;
        }
    }
}