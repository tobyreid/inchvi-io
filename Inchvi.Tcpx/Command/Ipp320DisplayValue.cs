namespace Inchvi.Tcpx.Command
{
    class Ipp320DisplayValue
    {
        private readonly string[] _value;

        public Ipp320DisplayValue(string value, char separator)
        {
            _value = value.Split(separator);
        }

        public string Line1
        {
            get { return _value.Length > 0 ? _value[0].PadRight(16) : string.Empty.PadRight(16); }
        }

        public string Line2
        {
            get { return _value.Length > 1 ? _value[1].PadRight(16) : string.Empty.PadRight(16); }
        }

        public string Line3
        {
            get { return _value.Length > 2 ? _value[2].PadRight(16) : string.Empty.PadRight(16); }
        }

        public string Line4
        {
            get { return _value.Length > 3 ? _value[3].PadRight(16) : string.Empty.PadRight(16); }
        }
    }
}