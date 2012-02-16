using System;
using System.Reflection;
using System.Collections;
using System.Text;

namespace CSharpcommon {
    /// <summary>
    /// 把枚举值按照指定的文本显示
    /// <remarks>
    /// EnumDescription.GetEnumText(typeof(MyEnum));
    /// EnumDescription.GetFieldText(MyEnum.Two);
    /// EnumDescription.GetFieldTexts(typeof(MyEnum)); 
    /// </example>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class EnumDescription : Attribute {
        private string enumDisplayText;
        private int enumRank;
        private FieldInfo fieldIno;

        /// <summary>
        /// 描述枚举值
        /// </summary>
        /// <param name="enumDisplayText">描述内容</param>
        /// <param name="enumRank">排列顺序</param>
        public EnumDescription(string enumDisplayText, int enumRank) {
            this.enumDisplayText = enumDisplayText;
            this.enumRank = enumRank;
        }

        /// <summary>
        /// 描述枚举值，默认排序为5
        /// </summary>
        /// <param name="enumDisplayText">描述内容</param>
        public EnumDescription(string enumDisplayText)
            : this(enumDisplayText, 5) { }

        public string EnumDisplayText {
            get { return this.enumDisplayText; }
        }

        public int EnumRank {
            get { return enumRank; }
        }

        public int EnumValue {
            get { return (int)fieldIno.GetValue(null); }
        }

        public string FieldName {
            get { return fieldIno.Name; }
        }

        #region  对枚举描述属性的解释相关函数

        /// <summary>
        /// 排序类型
        /// </summary>
        public enum SortType {
            /// <summary>
            ///按枚举顺序默认排序
            /// </summary>
            Default,
            /// <summary>
            /// 按描述值排序
            /// </summary>
            DisplayText,
            /// <summary>
            /// 按排序熵
            /// </summary>
            Rank
        }

        private static System.Collections.Hashtable cachedEnum = new Hashtable();


        /// <summary>
        /// 得到对枚举的描述文本
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static string GetEnumText(Type enumType) {
            EnumDescription[] eds = (EnumDescription[])enumType.GetCustomAttributes(typeof(EnumDescription), false);
            if (eds.Length != 1) return string.Empty;
            return eds[0].EnumDisplayText;
        }

        /// <summary>
        /// 获得指定枚举类型中，指定值的描述文本。
        /// </summary>
        /// <param name="enumValue">枚举值，不要作任何类型转换</param>
        /// <returns>描述字符串</returns>
        public static string GetFieldText(object enumValue) {
            EnumDescription[] descriptions = GetFieldTexts(enumValue.GetType(), SortType.Default);
            foreach (EnumDescription ed in descriptions) {
                if (ed.fieldIno.Name == enumValue.ToString()) return ed.EnumDisplayText;
            }
            return string.Empty;
        }


        /// <summary>
        /// 得到枚举类型定义的所有文本，按定义的顺序返回
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        /// <param name="enumType">枚举类型</param>
        /// <returns>所有定义的文本</returns>
        public static EnumDescription[] GetFieldTexts(Type enumType) {
            return GetFieldTexts(enumType, SortType.Default);
        }

        /// <summary>
        /// 得到枚举类型定义的所有文本
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        /// <param name="enumType">枚举类型</param>
        /// <param name="sortType">指定排序类型</param>
        /// <returns>所有定义的文本</returns>
        public static EnumDescription[] GetFieldTexts(Type enumType, SortType sortType) {
            EnumDescription[] descriptions = null;
            //缓存中没有找到，通过反射获得字段的描述信息
            if (cachedEnum.Contains(enumType.FullName) == false) {
                FieldInfo[] fields = enumType.GetFields();
                ArrayList edAL = new ArrayList();
                foreach (FieldInfo fi in fields) {
                    object[] eds = fi.GetCustomAttributes(typeof(EnumDescription), false);
                    if (eds.Length != 1) continue;
                    ((EnumDescription)eds[0]).fieldIno = fi;
                    edAL.Add(eds[0]);
                }

                cachedEnum.Add(enumType.FullName, (EnumDescription[])edAL.ToArray(typeof(EnumDescription)));
            }
            descriptions = (EnumDescription[])cachedEnum[enumType.FullName];
            if (descriptions.Length <= 0) throw new NotSupportedException("枚举类型[" + enumType.Name + "]未定义属性EnumValueDescription");

            //按指定的属性冒泡排序
            for (int m = 0; m < descriptions.Length; m++) {
                //默认就不排序了
                if (sortType == SortType.Default) break;

                for (int n = m; n < descriptions.Length; n++) {
                    EnumDescription temp;
                    bool swap = false;

                    switch (sortType) {
                        case SortType.Default:
                            break;
                        case SortType.DisplayText:
                            if (string.Compare(descriptions[m].EnumDisplayText, descriptions[n].EnumDisplayText) > 0) swap = true;
                            break;
                        case SortType.Rank:
                            if (descriptions[m].EnumRank > descriptions[n].EnumRank) swap = true;
                            break;
                    }

                    if (swap) {
                        temp = descriptions[m];
                        descriptions[m] = descriptions[n];
                        descriptions[n] = temp;
                    }
                }
            }

            return descriptions;
        }

        #endregion
    }
}
