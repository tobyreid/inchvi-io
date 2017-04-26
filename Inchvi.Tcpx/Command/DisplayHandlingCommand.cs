namespace Inchvi.Tcpx.Command
{
    public class DisplayHandlingCommand : TcpxStxEtxCommand
    {
        private readonly Ipp320DisplayValue _display;

        internal DisplayHandlingCommand(Ipp320DisplayValue display)
        {
            _display = display;
        }

        public override string RequestCommand
        {
            get { return "58.0041" + _display.Line1 + _display.Line2 + _display.Line3 + _display.Line4; }
        }

        protected override bool TryParseCommandResponse(byte[] data)
        {
            return true;
        }

    }
}