using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Scrappy.Noom
{
    public abstract class NoomRouterSegment
    {
        public static NoomRouterSegment[] Parse(string path)
        {
            Regex regex = new Regex(@"^((?<root>/)|((?<segment>/[^\{\}/]+)|(?<parameter>/\{[^/]+\}))+)$");
            Match match = regex.Match(path);

            List<NoomRouterSegment> captures = new List<NoomRouterSegment>();

            captures.AddRange(match.Groups["root"].Captures.OfType<Capture>().Select(x => new RootSegment(x)));
            captures.AddRange(match.Groups["segment"].Captures.OfType<Capture>().Select(x => new ValueSegment(x)));
            captures.AddRange(match.Groups["parameter"].Captures.OfType<Capture>().Select(x => new ParameterSegment(x)));
            captures.Sort((x, y) => x.Index.CompareTo(y.Index));

            return captures.ToArray();
        }

        protected abstract int Index { get; }

        public abstract bool Accept(string value);

        public abstract void Collect(NoomParameters parameters, string value);

        private class RootSegment : NoomRouterSegment
        {
            private readonly Capture capture;

            public RootSegment(Capture capture)
            {
                this.capture = capture;
            }

            protected override int Index
            {
                get { return capture.Index; }
            }

            public override bool Accept(string value)
            {
                return value == "/";
            }

            public override void Collect(NoomParameters parameters, string value)
            {
            }
        }

        private class ValueSegment : NoomRouterSegment
        {
            private readonly Capture capture;

            public ValueSegment(Capture capture)
            {
                this.capture = capture;
            }

            protected override int Index
            {
                get { return capture.Index; }
            }

            public override bool Accept(string value)
            {
                return capture.Value == value;
            }

            public override void Collect(NoomParameters parameters, string value)
            {
            }
        }

        private class ParameterSegment : NoomRouterSegment
        {
            private readonly Capture capture;

            public ParameterSegment(Capture capture)
            {
                this.capture = capture;
            }

            protected override int Index
            {
                get { return capture.Index; }
            }

            public override bool Accept(string value)
            {
                return true;
            }

            public override void Collect(NoomParameters parameters, string value)
            {
                string name = capture.Value.Substring(2, capture.Value.Length - 3);

                parameters.Add(name, value.Substring(1));
            }
        }
    }
}