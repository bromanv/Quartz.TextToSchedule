using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Quartz.TextToSchedule.Util;
using Quartz.TextToSchedule.Grammars;
using Quartz.TextToSchedule.Spanish;
namespace Quartz.TextToSchedule.Grammars
{
    public class SpanishGrammar : IGrammar
    {
       //public static readonly string SPACE = @"\ ";
        public static readonly string LIST_SEPARATOR = @"( ?, ?| ?; ?| ?\/ ?| ?\\ ?| ?\  ?|\ ?,?\ ?y\ ?|\ ?,?\ ?&\ ?)";
        public static readonly string RANGE_SEPARATOR = "( hasta  | ?- ?)";
        public static readonly string AMOUNT = @"(?<AMOUNT>\d+)";

        #region Patterns - INTERVAL UNITS

        //INTERVALUNIT
        //regular time value is (seconds|minutes|hours)
        public static readonly string INTERVALUNIT_SECOND = "(s|seg|segs|segundo|segundos)";
        public static readonly string INTERVALUNIT_MINUTE = "(m|min|mins|minuto|minutos)";
        public static readonly string INTERVALUNIT_HOUR = "(h|hr|hrs|hora|horas)";

        public static readonly string INTERVALUNIT_DAY = "(d|dia|día|días|dias)";
        public static readonly string INTERVALUNIT_WEEK = "(sm|semana|semanas)";
        public static readonly string INTERVALUNIT_MONTH = "(ms|mes|meses)";
        public static readonly string INTERVALUNIT_YEAR = "(a|ao|año|años)";

        public static readonly string INTERVALUNIT_TIME = RegexHelper.Builder_GroupOf("INTERVALUNIT", new string[] { INTERVALUNIT_SECOND, INTERVALUNIT_MINUTE, INTERVALUNIT_HOUR });
        public static readonly string INTERVALUNIT_DATE = RegexHelper.Builder_GroupOf("INTERVALUNIT", new string[] { INTERVALUNIT_DAY, INTERVALUNIT_WEEK, INTERVALUNIT_MONTH, INTERVALUNIT_YEAR });
        public static readonly string INTERVALUNIT_ALL = RegexHelper.Builder_GroupOf("INTERVALUNIT", new string[] { INTERVALUNIT_SECOND, INTERVALUNIT_MINUTE, INTERVALUNIT_HOUR, INTERVALUNIT_DAY, INTERVALUNIT_WEEK, INTERVALUNIT_MONTH, INTERVALUNIT_YEAR });

        #endregion

        #region Patterns - DAY OF WEEK

        //DAYOFWEEK
        public static readonly string MONDAY = @"(lun|lunes)";
        public static readonly string TUESDAY = @"(mar|martes)";
        public static readonly string WEDNESDAY = @"(mie|miercoles|miércoles)";
        public static readonly string THURSDAY = @"(jue|jueves)";
        public static readonly string FRIDAY = @"(vie|viernes)";
        public static readonly string SATURDAY = @"(sab|sábado|sabado)";
        public static readonly string SUNDAY = @"(dom|domingo)";

        public static readonly string DAYOFWEEK_ONE = RegexHelper.Builder_GroupOf(new string[] { MONDAY, TUESDAY, WEDNESDAY, THURSDAY, FRIDAY, SATURDAY, SUNDAY });
        public static readonly string DAYOFWEEK_RANGE = RegexHelper.Builder_Range(DAYOFWEEK_ONE, RANGE_SEPARATOR);
        public static readonly string DAYOFWEEK = RegexHelper.Builder_GroupOf("DAYOFWEEK", new string[] { DAYOFWEEK_ONE, DAYOFWEEK_RANGE });

        public static readonly string DAYOFWEEK_SPEC = RegexHelper.Builder_ListOf(DAYOFWEEK, LIST_SEPARATOR);

        #endregion

        #region Patterns - MONTHS

        //MONTHS
        public static readonly string JANUARY = "(ene|enero)";
        public static readonly string FEBRUARY = "(feb|febrero)";
        public static readonly string MARCH = "(mar|marzo)";
        public static readonly string APRIL = "(abr|abril)";
        public static readonly string MAY = "(may|mayo)";
        public static readonly string JUNE = "(jun|junio)";
        public static readonly string JULY = "(jul|julio)";
        public static readonly string AUGUST = "(aug|augosto)";
        public static readonly string SEPTEMBER = "(sep|sept|septiember)";
        public static readonly string OCTOBER = "(oct|octubre)";
        public static readonly string NOVEMBER = "(nov|noviembre)";
        public static readonly string DECEMBER = "(dic|decimbre)";

        public static readonly string MONTH_ONE = RegexHelper.Builder_GroupOf(new string[] { JANUARY, FEBRUARY, MARCH, APRIL, MAY, JUNE, JULY, AUGUST, SEPTEMBER, OCTOBER, NOVEMBER, DECEMBER });

        public static readonly string MONTH_RANGE = RegexHelper.Builder_Range(MONTH_ONE, RANGE_SEPARATOR);
        public static readonly string MONTH = RegexHelper.Builder_GroupOf("MONTH", new string[] { MONTH_ONE, MONTH_RANGE });

        public static readonly string MONTH_SINGLE = RegexHelper.Builder_Capture("MONTH", MONTH_ONE);
        public static readonly string MONTH_SPEC = RegexHelper.Builder_ListOf(MONTH, LIST_SEPARATOR);

        #endregion

        #region Patterns - ORDINAL

        //ORDINAL
        public static readonly string ORDINAL_FIRST = "(primero|1ero|1ro|1er)";
        public static readonly string ORDINAL_SECOND = "(segundo)";
        public static readonly string ORDINAL_THIRD = "(tercero|3ero|3er)";
        public static readonly string ORDINAL_FOURTH = "(cuarto)";
        public static readonly string ORDINAL_FIFTH = "(quinto)";
        public static readonly string ORDINAL_LAST = "(ultimo|último)";

        public static readonly string ORDINAL_ALL = RegexHelper.Builder_GroupOf("ORDINAL", new string[] { ORDINAL_FIRST, ORDINAL_SECOND, ORDINAL_THIRD, ORDINAL_FOURTH, ORDINAL_FIFTH, ORDINAL_LAST });
        public static readonly string ORDINAL = RegexHelper.Builder_ListOf(ORDINAL_ALL, LIST_SEPARATOR);

        #endregion

        #region Patterns - TIME

        //TIME
        public static readonly string TIME_HOUR = @"(?<HOUR>\d|[0-1]\d|2[0-3])";
        public static readonly string TIME_MINUTE = @"(?<MINUTE>[0-5]\d)";
        public static readonly string TIME_SECOND = @"(?<SECOND>[0-5]\d)";
        public static readonly string TIME_AM = @"(am|a\.m\.|a\.m|am\.)";
        public static readonly string TIME_PM = @"(pm|p\.m\.|p\.m|pm\.)";
        public static readonly string TIME_MERIDIEM = RegexHelper.Builder_GroupOf("MERIDIEM", new string[] { TIME_AM, TIME_PM });

        public static readonly string TIME = @"(?<TIME>{TIME_HOUR}((:| ){TIME_MINUTE})?((:| ){TIME_SECOND})? ?{TIME_MERIDIEM}?)";

        public static readonly string TIME_ONCE = "((a las )?{TIME})";
        public static readonly string TIME_RANGE = "((desde (?<FROMTIME>{TIME}) hasta (?<TOTIME>{TIME}))|(entre (?<FROMTIME>{TIME}) y (?<TOTIME>{TIME})))";
        public static readonly string TIME_ONCE_OR_RANGE = RegexHelper.Builder_GroupOf("TIMEONCEORRANGE", new string[] { TIME_ONCE, TIME_RANGE });

        public static readonly string TIME_LIST = "((a las )?" + RegexHelper.Builder_ListOf("{TIME}", LIST_SEPARATOR) + ")";

        #endregion

        #region Patterns - DATE

        public static readonly string DATE_DAY = @"(?<DAY>(0?[1-9])|(1[0-9])|(2[0-9])|(3[0-3]))(er|do|o|ero)?";
        public static readonly string DATE_DAY_LIST = RegexHelper.Builder_ListOf(DATE_DAY, LIST_SEPARATOR);

        public static readonly string DATE_YEAR = @"(?<YEAR>\d\d(\d\d)?)";
        public static readonly string DATE = @"({MONTH_SINGLE}( ?,? ?{DATE_DAY})?( ?,? {DATE_YEAR})?)";

        public static readonly string DATE_MONTH_NUMERIC = "(?<MONTH>(0?[1-9]|1[0-2]))";
        public static readonly string DATE2 = @"({DATE_MONTH_NUMERIC}/{DATE_DAY}(/{DATE_YEAR})?)";

        public static readonly string DATE3 = @"({DATE_DAY} of {MONTH_SINGLE}( ?,? {DATE_YEAR})?)";

        public static readonly string DATE_SPEC = RegexHelper.Builder_GroupOf("DATESPEC", new string[] { DATE, DATE2, DATE3 });
        #endregion


        // ---------------------
        // EXPRESSIONS
        // ---------------------

        //////  SYNTAX: ("every"|ordinal) (day of week) ["of" (monthspec)] (time)
        //////  SYNTAX: every [n] (second|minute|hour) "on" (day of week) of monthspec (time)
        //[every [n] (second|minute|hour)] on (datespec) (time)
        //every [n] week(s) on (day of week) (time)

        //Expression Ideas
        //Implemented Ideas
        //1) every [n] (seconds|minutes|hours) [timespec]
        //2) ("every"|ordinal) (day of week) ["of" (monthspec)] (time)
        //3) every [n] (second|minute|hour) "on" (day of week) of monthspec (time)
        //4) every (datespec) at (time)

        //Other Ideas
        //every [n] (second|minute|hour|day) "of" monthspec (time)
        //every [n] (second|minute|hour) "on" (day of week) (time)
        //every [n] (second|minute|hour) "on" datespec (time)
        //every [n] weeks of (monthspec) (time)
        //every [n] month(s) from (month_once|datespec) (time)             every 3 months from february (3rd) at 4pm, every 3 months at 5pm
        //every [n] month(s) from (DATE_DAY) (time)                        every 3 months on the 1st at 4pm
        //every [n] year(s) on (datespec) (time)                           every 2 years on january 3rd at 4:35am

        // support, last day offsets, like second to last day, by using "L-[n]", where n is offset.
        // support nearest weekday "W".


        public static readonly string START_TIME = "(iniciando (a las|a la|el) ({DATE_SPEC}? ?{TIME_ONCE}))";


        /// <summary>
        /// Initializes the <see cref="SpanishGrammar" /> class.
        /// </summary>
        static SpanishGrammar()
        {
            //some of the magic of this class
            //is that it uses the NamedFormatHelper 
            //on all STATIC fields.

            //so that it can replace text it finds with values from other properties.

            var members = typeof(SpanishGrammar).GetMembers();

            foreach (var f in members.OfType<FieldInfo>().Where(x => x.FieldType == typeof(string)))
            {
                if (f.IsStatic) //check to see if field is static
                {
                    f.SetValue(null, NamedFormatHelper.NamedFormat(f.GetValue(null) as string, typeof(SpanishGrammar)));
                }
            }
            foreach (var p in members.OfType<PropertyInfo>().Where(x => x.PropertyType == typeof(string)))
            {
                //check to see if property has a static getter

                if (p.CanRead && p.CanWrite && p.GetSetMethod().IsStatic)
                {
                    p.SetValue(null, NamedFormatHelper.NamedFormat(p.GetValue(null, null) as string, typeof(SpanishGrammar)), null);
                }
            }
        }

        #region Expressions

        //every [n] (sec|min|hour) [on mon-fri] [of monthspec] [time]
        public static readonly string SpecialExpr1 = @"(cada( {AMOUNT})? ?{INTERVALUNIT_TIME}( los {DAYOFWEEK_SPEC})?( del {MONTH_SPEC})?( {TIME_ONCE_OR_RANGE})?)";

        //(every|ordinal) (day of week) [of (month)] [time]
        public static readonly string SpecialExpr2 = @"((cada|{ORDINAL})( (dia|{DAYOFWEEK_SPEC}))( del (mes|({MONTH_SPEC})))?( {TIME_LIST})?)";

        //[every|on] (date) [time]
        public static readonly string SpecialExpr3 = @"(((cada|el) )?{DATE_SPEC}( {TIME_LIST})?)";

        // every [n] (days|weeks|months|years) (on [day of weeks]) (from [date]) (at [time])
        public static readonly string SpecialExpr4 = @"(cada( {AMOUNT})? ?{INTERVALUNIT_ALL}( (el )?{DAYOFWEEK_SPEC})?( (desde )?{DATE_SPEC})?( {TIME_LIST})?)";

        // every [n] day of (month) [time]
        public static readonly string SpecialExpr5 = @"(((cada|el) )?{DATE_DAY_LIST}( dia)?(( del)? (mes|({MONTH_SPEC})))?( {TIME_LIST})?)";


        #endregion

        /// <summary>
        /// every [n] (sec|min|hour) [on mon-fri] [of monthspec] [time]
        /// </summary>
        public string Expression1
        {
            get { return SpecialExpr1; }
        }

        /// <summary>
        /// (every|ordinal) (day of week) [of (month)] [time]
        /// </summary>
        public string Expression2
        {
            get { return SpecialExpr2; }
        }

        /// <summary>
        /// [every|on] (date) [time]
        /// </summary>
        public string Expression3
        {
            get { return SpecialExpr3; }
        }

        /// <summary>
        /// every [n] (days|weeks|months|years) (from [date]) (at [time])
        /// </summary>
        public string Expression4
        {
            get { return SpecialExpr4; }
        }

        /// <summary>
        /// every [n] day of (month) [time]
        /// </summary>
        public string Expression5
        {
            get { return SpecialExpr5; }
        }
    }
}
