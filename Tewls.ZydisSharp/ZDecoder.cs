namespace Tewls.ZydisSharp
{
    public class ZDecoder
    {
        private ZydisDecoder _decoder;

        public static ZDecoder Create(ZydisMachineMode machineMode, ZydisStackWidth stackWidth) =>
            new(Zydis.CreateDecoder(machineMode, stackWidth));

        internal ZDecoder(ZydisDecoder zydisDecoder)
        {
            _decoder = zydisDecoder;
        }

        public DecodeResult DecodeFull(byte[] buffer) => Zydis.DecodeFull(ref _decoder, buffer);
    }
}
