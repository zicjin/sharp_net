using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace sharp_net {
    public static class T4Common {
        public static string GetMethodReturnType(MethodInfo method) {
            Type type = method.ReturnType;
            if (!type.IsGenericType) {
                if (type == typeof(void))
                    return "void";
                else
                    return type.Name;
            }
            string returnTypeGenericTypeName = type.Name.Replace("`1", "");
            string returnTypeGenericTypeArgumentName = type.GetGenericArguments()[0].Name;
            return string.Format("{0}<{1}>", returnTypeGenericTypeName, returnTypeGenericTypeArgumentName);
        }

        public static string GetTypeName(ParameterInfo parameter) {
            StringBuilder sb = new StringBuilder();
            Type type = parameter.ParameterType;

            if (parameter.IsRetval)
                sb.Append("ref ");
            else if (parameter.IsOut)
                sb.Append("out ");

            if (type.IsGenericType) {
                string returnTypeGenericTypeName = type.Name.Replace("`1", "");
                string returnTypeGenericTypeArgumentName = type.GetGenericArguments()[0].Name;
                sb.Append(string.Format("{0}<{1}>", returnTypeGenericTypeName, returnTypeGenericTypeArgumentName));
            } else {
                sb.Append(type.Name);
            }

            return sb.ToString();
        }

        public static string GetMethodParameterList(MethodInfo method) {
            ParameterInfo[] parameters = method.GetParameters();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < parameters.Length; i++) {
                sb.Append(GetTypeName(parameters[i]));
                sb.Append(" ");
                sb.Append(parameters[i].Name);
                if (i != parameters.Length - 1)
                    sb.Append(", ");
            }
            return sb.ToString();
        }

        public static string GetMethodParameterValueList(MethodInfo method) {
            ParameterInfo[] parameters = method.GetParameters();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < parameters.Length; i++) {
                sb.Append(parameters[i].Name);
                if (i != parameters.Length - 1)
                    sb.Append(", ");
            }
            return sb.ToString();
        }
    }
}
