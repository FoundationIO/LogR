using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Framework.Utilities.PocoGenerator.Utilities
{
    public class GeneratorUtils
    {
        private static Regex rxCleanUp = new Regex(@"[^\w\d_]", RegexOptions.Compiled);

        private static string[] csKeywords =
        {
            "abstract", "event", "new", "struct", "as", "explicit", "null",
            "switch", "base", "extern", "object", "this", "bool", "false", "operator", "throw",
            "break", "finally", "out", "true", "byte", "fixed", "override", "try", "case", "float",
            "params", "typeof", "catch", "for", "private", "uint", "char", "foreach", "protected",
            "ulong", "checked", "goto", "public", "unchecked", "class", "if", "readonly", "unsafe",
            "const", "implicit", "ref", "ushort", "continue", "in", "return", "using", "decimal",
            "int", "sbyte", "virtual", "default", "interface", "sealed", "volatile", "delegate",
            "internal", "short", "void", "do", "is", "sizeof", "while", "double", "lock",
            "stackalloc", "else", "long", "static", "enum", "namespace", "string",
        };

        private static Func<string, string> cleanUp = (str) =>
        {
            str = rxCleanUp.Replace(str, "_");

            if (char.IsDigit(str[0]) || csKeywords.Contains(str))
            {
                str = "@" + str;
            }

            str = str.Replace("_", string.Empty);
            return str;
        };

        private static Func<string, string> cleanUpTable = (str) =>
        {
            str = rxCleanUp.Replace(str, "_");

            if (char.IsDigit(str[0]) || csKeywords.Contains(str))
            {
                str = "@" + str;
            }

            if (str.StartsWith("vw"))
            {
                str = str.Substring(2) + "View";
            }
            else if (str.StartsWith("tbl"))
            {
                str = str.Substring(3);
            }

            str = str.Replace("_", string.Empty);
            return str;
        };

        public static string MakeClassName(string str)
        {
            return cleanUpTable(GeneratorUtils.Inflector.ToTitleCase(GeneratorUtils.Inflector.MakeSingular(str)));
        }

        public static string MakePropertyName(string str)
        {
            return cleanUpTable(GeneratorUtils.Inflector.ToTitleCase(str));
        }

        public static string GetNullableValueAsString(bool value)
        {
            if (value == true)
            {
                return "true";
            }

            return "false";
        }

        public static string ZapPassword(string connectionString)
        {
            var rx = new Regex("password=.*;", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            return rx.Replace(connectionString, "password=**zapped**;");
        }

        public static int GetDatatypePrecision(string type)
        {
            int startPos = type.IndexOf(",");
            if (startPos < 0)
            {
                return -1;
            }

            int endPos = type.IndexOf(")");
            if (endPos < 0)
            {
                return -1;
            }

            string typePrecisionStr = type.Substring(startPos + 1, endPos - startPos - 1);
            int result = -1;
            if (int.TryParse(typePrecisionStr, out result))
            {
                return result;
            }
            else
            {
                return -1;
            }
        }

        public static int GetDatatypeSize(string type)
        {
            int result = -1;
            if (int.TryParse(type, out result))
            {
                return result;
            }

            int startPos = type.IndexOf("(");
            if (startPos < 0)
            {
                return -1;
            }

            int endPos = type.IndexOf(",");
            if (endPos < 0)
            {
                endPos = type.IndexOf(")");
            }

            string typeSizeStr = type.Substring(startPos + 1, endPos - startPos - 1);
            if (int.TryParse(typeSizeStr, out result))
            {
                return result;
            }
            else
            {
                return -1;
            }
        }

        public static class Inflector
        {
            private static readonly List<InflectorRule> _plurals = new List<InflectorRule>();
            private static readonly List<InflectorRule> _singulars = new List<InflectorRule>();
            private static readonly List<string> _uncountables = new List<string>();

            static Inflector()
            {
                AddPluralRule("$", "s");
                AddPluralRule("s$", "s");
                AddPluralRule("(ax|test)is$", "$1es");
                AddPluralRule("(octop|vir)us$", "$1i");
                AddPluralRule("(alias|status)$", "$1es");
                AddPluralRule("(bu)s$", "$1ses");
                AddPluralRule("(buffal|tomat)o$", "$1oes");
                AddPluralRule("([ti])um$", "$1a");
                AddPluralRule("sis$", "ses");
                AddPluralRule("(?:([^f])fe|([lr])f)$", "$1$2ves");
                AddPluralRule("(hive)$", "$1s");
                AddPluralRule("([^aeiouy]|qu)y$", "$1ies");
                AddPluralRule("(x|ch|ss|sh)$", "$1es");
                AddPluralRule("(matr|vert|ind)ix|ex$", "$1ices");
                AddPluralRule("([m|l])ouse$", "$1ice");
                AddPluralRule("^(ox)$", "$1en");
                AddPluralRule("(quiz)$", "$1zes");

                AddSingularRule("s$", string.Empty);
                AddSingularRule("ss$", "ss");
                AddSingularRule("(n)ews$", "$1ews");
                AddSingularRule("([ti])a$", "$1um");
                AddSingularRule("((a)naly|(b)a|(d)iagno|(p)arenthe|(p)rogno|(s)ynop|(t)he)ses$", "$1$2sis");
                AddSingularRule("(^analy)ses$", "$1sis");
                AddSingularRule("([^f])ves$", "$1fe");
                AddSingularRule("(hive)s$", "$1");
                AddSingularRule("(tive)s$", "$1");
                AddSingularRule("([lr])ves$", "$1f");
                AddSingularRule("([^aeiouy]|qu)ies$", "$1y");
                AddSingularRule("(s)eries$", "$1eries");
                AddSingularRule("(m)ovies$", "$1ovie");
                AddSingularRule("(x|ch|ss|sh)es$", "$1");
                AddSingularRule("([m|l])ice$", "$1ouse");
                AddSingularRule("(bus)es$", "$1");
                AddSingularRule("(o)es$", "$1");
                AddSingularRule("(shoe)s$", "$1");
                AddSingularRule("(cris|ax|test)es$", "$1is");
                AddSingularRule("(octop|vir)i$", "$1us");
                AddSingularRule("(alias|status)$", "$1");
                AddSingularRule("(alias|status)es$", "$1");
                AddSingularRule("^(ox)en", "$1");
                AddSingularRule("(vert|ind)ices$", "$1ex");
                AddSingularRule("(matr)ices$", "$1ix");
                AddSingularRule("(quiz)zes$", "$1");

                AddIrregularRule("person", "people");
                AddIrregularRule("man", "men");
                AddIrregularRule("child", "children");
                AddIrregularRule("sex", "sexes");
                AddIrregularRule("tax", "taxes");
                AddIrregularRule("move", "moves");

                AddUnknownCountRule("equipment");
                AddUnknownCountRule("information");
                AddUnknownCountRule("rice");
                AddUnknownCountRule("money");
                AddUnknownCountRule("species");
                AddUnknownCountRule("series");
                AddUnknownCountRule("fish");
                AddUnknownCountRule("sheep");
            }

            public static string MakePlural(string word)
            {
                return ApplyRules(_plurals, word);
            }

            public static string MakeSingular(string word)
            {
                return ApplyRules(_singulars, word);
            }

            public static string ToTitleCase(string word)
            {
                return Regex.Replace(ToHumanCase(AddUnderscores(word)), @"\b([a-z])", match => { return match.Captures[0].Value.ToUpper(); });
            }

            public static string ToHumanCase(string lowercaseAndUnderscoredWord)
            {
                return MakeInitialCaps(Regex.Replace(lowercaseAndUnderscoredWord, @"_", " "));
            }

            public static string AddUnderscores(string pascalCasedWord)
            {
                return Regex.Replace(Regex.Replace(Regex.Replace(pascalCasedWord, @"([A-Z]+)([A-Z][a-z])", "$1_$2"), @"([a-z\d])([A-Z])", "$1_$2"), @"[-\s]", "_").ToLower();
            }

            public static string MakeInitialCaps(string word)
            {
                return string.Concat(word.Substring(0, 1).ToUpper(), word.Substring(1).ToLower());
            }

            public static string MakeInitialLowerCase(string word)
            {
                return string.Concat(word.Substring(0, 1).ToLower(), word.Substring(1));
            }

            public static bool IsStringNumeric(string str)
            {
                double result;
                return double.TryParse(str, NumberStyles.Float, NumberFormatInfo.CurrentInfo, out result);
            }

            public static string UppercaseFirst(string s)
            {
                if (string.IsNullOrEmpty(s))
                {
                    return string.Empty;
                }

                return char.ToUpper(s[0]) + s.Substring(1);
            }

            public static string AddOrdinalSuffix(string number)
            {
                if (IsStringNumeric(number))
                {
                    int n = int.Parse(number);
                    int nMod100 = n % 100;

                    if (nMod100 >= 11 && nMod100 <= 13)
                    {
                        return string.Concat(number, "th");
                    }

                    switch (n % 10)
                    {
                        case 1:
                            return string.Concat(number, "st");
                        case 2:
                            return string.Concat(number, "nd");
                        case 3:
                            return string.Concat(number, "rd");
                        default:
                            return string.Concat(number, "th");
                    }
                }

                return number;
            }

            public static string ConvertUnderscoresToDashes(string underscoredWord)
            {
                return underscoredWord.Replace('_', '-');
            }

            private static void AddIrregularRule(string singular, string plural)
            {
                AddPluralRule(string.Concat("(", singular[0], ")", singular.Substring(1), "$"), string.Concat("$1", plural.Substring(1)));
                AddSingularRule(string.Concat("(", plural[0], ")", plural.Substring(1), "$"), string.Concat("$1", singular.Substring(1)));
            }

            private static void AddUnknownCountRule(string word)
            {
                _uncountables.Add(word.ToLower());
            }

            private static void AddPluralRule(string rule, string replacement)
            {
                _plurals.Add(new InflectorRule(rule, replacement));
            }

            private static void AddSingularRule(string rule, string replacement)
            {
                _singulars.Add(new InflectorRule(rule, replacement));
            }

            private static string ApplyRules(IList<InflectorRule> rules, string word)
            {
                string result = word;
                if (!_uncountables.Contains(word.ToLower()))
                {
                    for (int i = rules.Count - 1; i >= 0; i--)
                    {
                        string currentPass = rules[i].Apply(word);
                        if (currentPass != null)
                        {
                            result = currentPass;
                            break;
                        }
                    }
                }

                return result;
            }

            private class InflectorRule
            {
                private readonly Regex regex;
                private readonly string replacement;

                public InflectorRule(string regexPattern, string replacementText)
                {
                    regex = new Regex(regexPattern, RegexOptions.IgnoreCase);
                    replacement = replacementText;
                }

                public string Apply(string word)
                {
                    if (!regex.IsMatch(word))
                    {
                        return null;
                    }

                    string replace = regex.Replace(word, replacement);
                    if (word == word.ToUpper())
                    {
                        replace = replace.ToUpper();
                    }

                    return replace;
                }
            }
        }
    }
}
