using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace yMatouFlow.Unite {
    /// <summary>
    /// IList类型与datatable类型的转换
    /// </summary>
    public class uConvert {
        public static DataTable ListToDataTable<T>(List<T> entitys) {
            if (entitys == null || entitys.Count < 1) return null;

            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            DataTable dt = new DataTable();
            for (int i = 0; i < entityProperties.Length; i++) {
                //dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
                dt.Columns.Add(entityProperties[i].Name);
            }

            foreach (object entity in entitys) {
                if (entity.GetType() != entityType) {
                    throw new Exception("要转换的集合元素类型不一致");
                }
                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++) {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);
                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }

        public static IList<T> DataTableToList<T>(DataTable dt) {
            if (dt == null) return null;
            IList<T> result = new List<T>();
            for (int j = 0; j < dt.Rows.Count; j++) {
                T _t = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertys = _t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys) {
                    for (int i = 0; i < dt.Columns.Count; i++) {
                        if (pi.Name.Equals(dt.Columns[i].ColumnName)) {
                            if (dt.Rows[j][i] != DBNull.Value) {
                                switch (pi.PropertyType.ToString()) {
                                    case "System.Int32":
                                        pi.SetValue(_t, Int32.Parse(dt.Rows[j][i].ToString()), null);
                                        break;
                                    case "System.DateTime":
                                        pi.SetValue(_t, Convert.ToDateTime(dt.Rows[j][i].ToString()), null);
                                        break;
                                    case "System.String":
                                        pi.SetValue(_t, dt.Rows[j][i].ToString(), null);
                                        break;
                                    case "System.Boolean":
                                        pi.SetValue(_t, Convert.ToBoolean(dt.Rows[j][i].ToString()), null);
                                        break;
                                    case "System.Double":
                                        pi.SetValue(_t, Convert.ToDouble(dt.Rows[j][i].ToString()), null);
                                        break;
                                    case "System.Decimal":
                                        pi.SetValue(_t, Convert.ToDecimal(dt.Rows[j][i].ToString()), null);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        }
                    }
                }
                result.Add(_t);
            }
            return result;
        }
    }
}
