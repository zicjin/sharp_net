using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace sharp_net.Infrastructure {

    //<?xml version="1.0" encoding="utf-8" ?>
    //<config>
    //  <tag name="*">
    //    <attr name="style">^((font|color)\s*:\s*[^;]+;\s*)*$</attr>
    //  </tag>
    //  <tag name="a">
    //    <attr name="href">^[a-zA-Z]+://.+$</attr>
    //    <attr name="title">.+</attr>
    //  </tag>
    //  ...
    //</config>
    //http://blog.zhaojie.me/2010/10/html-xss-filter-with-whilte-list.html

    public class TagXmlConfig : Dictionary<string, Regex> {
        public TagXmlConfig()
            : base(StringComparer.OrdinalIgnoreCase) { }

        public TagXmlConfig(XElement config) {
            foreach (var ele in config.Elements("attr")) {
                this.Add(ele.Attribute("name").Value, new Regex(ele.Value));
            }
        }
    }

    public class FilterXmlConfig : Dictionary<string, TagXmlConfig> {
        public FilterXmlConfig()
            : base(StringComparer.OrdinalIgnoreCase) { }

        public FilterXmlConfig(XElement config) {
            var wildcardElement = config
                .Elements("tag")
                .SingleOrDefault(e => e.Attribute("name").Value == "*");

            var wildcardConfig = wildcardElement == null ? null :
                new TagXmlConfig(wildcardElement);

            foreach (var ele in config
                .Elements("tag")
                .Where(e => e.Attribute("name").Value != "*")) {
                var name = ele.Attribute("name").Value;
                var tagConfig = new TagXmlConfig(ele);

                if (wildcardConfig != null) {
                    foreach (var pair in wildcardConfig) {
                        if (!tagConfig.ContainsKey(pair.Key)) {
                            tagConfig.Add(pair.Key, pair.Value);
                        }
                    }
                }

                this.Add(name, tagConfig);
            }
        }
    }

    public class HtmlFilter {
        private static readonly RegexOptions REGEX_OPTIONS =
            RegexOptions.Compiled |
            RegexOptions.IgnoreCase |
            RegexOptions.Singleline;

        // 依次填入上文中三个正则表达式
        private static readonly Regex TAG_REGEX = new Regex("<[^>]*>", REGEX_OPTIONS);
        private static readonly Regex VALID_TAG_REGEX = new Regex(@"^(?<begin></?)(?<tag>[a-zA-z]+)\s*(?<attr>[^>]*?)(?<end>/?>)$", REGEX_OPTIONS);
        private static readonly Regex ATTRIBUTE_REGEX = new Regex(@"(?<name>[a-zA-Z]+)\s*=\s*""(?<value>[^""]*)""", REGEX_OPTIONS);

        public HtmlFilter() : this(null) { }

        public HtmlFilter(FilterXmlConfig config) {
            this.Config = config ?? new FilterXmlConfig();
        }

        public FilterXmlConfig Config { get; private set; }

        public string Filter(string html) {
            // 对每个HTML标记进行替换?
            return TAG_REGEX.Replace(html, GetTag);
        }

        private string GetTag(Match match) {
            // 如果不是合法的HTML标记形式，则替换为空字符串
            var validTagMatch = VALID_TAG_REGEX.Match(match.Value);
            if (!validTagMatch.Success) return "";

            var tag = validTagMatch.Groups["tag"].Value;

            // 如果这个标记不在白名单中，则替换为空字符串
            TagXmlConfig tagConfig;
            if (!this.Config.TryGetValue(tag, out tagConfig)) return "";

            var begin = validTagMatch.Groups["begin"].Value;
            // 如果是闭合标记，则直接构造并返回
            if (begin == "</") {
                return String.Format("</{0}>", tag.ToLower());
            }

            // 过滤出合法的属性键值对
            var attrText = validTagMatch.Groups["attr"].Value;
            var attrMatches = ATTRIBUTE_REGEX.Matches(attrText).Cast<Match>();
            var validAttributes = attrMatches
                .Select(m => GetAttribute(m, tagConfig))
                .Where(s => !String.IsNullOrEmpty(s)).ToArray();

            var end = validTagMatch.Groups["end"].Value;
            // 如果没有合法的属性，则直接构造返回
            if (validAttributes.Length == 0) {
                return begin + tag + end;
            } else {// 否则返回带属性的HTML标记
                return String.Format(
                    "{0}{1} {2}{3}",
                    begin,
                    tag,
                    String.Join(" ", validAttributes),
                    end);
            }
        }

        private static string GetAttribute(Match attrMatch, TagXmlConfig tagConfig) {
            var name = attrMatch.Groups["name"].Value;

            Regex regex;
            if (!tagConfig.TryGetValue(name, out regex)) return "";

            var value = attrMatch.Groups["value"].Value;
            if (regex.IsMatch(value)) {
                return String.Format("{0}=\"{1}\"", name, value);
            } else {
                return "";
            }
        }
    }
}
