using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShare.Model
{
    struct TextViewPosition
    {
        public int Line { get; }
        public int Column { get; }

        public TextViewPosition(int line, int column)
        {
            Line = line;
            Column = column;
        }

        public static bool operator <(TextViewPosition a, TextViewPosition b)
        {
            if (a.Line < b.Line)
            {
                return true;
            }

            if (a.Line == b.Line)
            {
                return a.Column < b.Column;
            }

            return false;
        }

        public static bool operator >(TextViewPosition a, TextViewPosition b)
        {
            if (a.Line > b.Line)
            {
                return true;
            }

            if (a.Line == b.Line)
            {
                return a.Column > b.Column;
            }

            return false;
        }

        public static TextViewPosition Min(TextViewPosition a, TextViewPosition b)
        {
            return a > b ? b : a;
        }

        public static TextViewPosition Max(TextViewPosition a, TextViewPosition b)
        {
            return a > b ? a : b;
        }
    }
}
