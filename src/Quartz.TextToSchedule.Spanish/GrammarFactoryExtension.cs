using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.TextToSchedule.Grammars
{
    public static class SpanishGrammarFactoryExtension
    {
        public static ITextToSchedule CreateSpanishParser(this TextToScheduleFactory factory)
        {
            return new TextToSchedule(
                new SpanishGrammar(),
                new SpanishGrammarHelper());
        }
    }
}
