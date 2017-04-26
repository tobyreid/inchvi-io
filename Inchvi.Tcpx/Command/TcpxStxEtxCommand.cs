namespace Inchvi.Tcpx.Command
{
    public abstract class TcpxStxEtxCommand : TcpxBaseCommand
    {
        public override string CommandStartChar
        {
            get { return "\u0002"; } //STX
        }

        public override string CommandEndChar
        {
            get { return "\u0003"; } //ETX
        }
    }
}