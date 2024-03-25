using System;

namespace IlDasm_CSharp
{
    public class ILDasm
    {
        #region defines

        [Flags]
        enum RFlag : byte
        {
            None = 0x00,
            I8 = 0x01,
            I16 = 0x02,
            I16_I32 = 0x04,
            I16_I32_I64 = 0x08,
            Extended = 0x10,
            Relative = 0x20,
            ModRM = 0x40,
            Prefix = 0x80,
            Invalid = 0x80
        };

        [Flags]
        public enum IFlags : byte
        {
            Invalid = 0x01,
            Prefix = 0x02,
            Rex = 0x04,
            ModRM = 0x08,
            Sib = 0x10,
            Disp = 0x20,
            Imm = 0x40,
            Relative = 0x80,
        };

        #region flags_table
        static byte[] flags_table = {
            0x40,0x40,0x40,0x40,0x01,0x04,0x00,0x00,0x40,0x40,0x40,0x40,0x01,0x04,0x00,0x00,
            0x40,0x40,0x40,0x40,0x01,0x04,0x00,0x00,0x40,0x40,0x40,0x40,0x01,0x04,0x00,0x00,
            0x40,0x40,0x40,0x40,0x01,0x04,0x80,0x00,0x40,0x40,0x40,0x40,0x01,0x04,0x80,0x00,
            0x40,0x40,0x40,0x40,0x01,0x04,0x80,0x00,0x40,0x40,0x40,0x40,0x01,0x04,0x80,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x40,0x40,0x80,0x80,0x80,0x80,0x04,0x44,0x01,0x41,0x00,0x00,0x00,0x00,
            0x21,0x21,0x21,0x21,0x21,0x21,0x21,0x21,0x21,0x21,0x21,0x21,0x21,0x21,0x21,0x21,
            0x41,0x44,0x41,0x41,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x06,0x00,0x00,0x00,0x00,0x00,
            0x01,0x08,0x01,0x08,0x00,0x00,0x00,0x00,0x01,0x04,0x00,0x00,0x00,0x00,0x00,0x00,
            0x01,0x01,0x01,0x01,0x01,0x01,0x01,0x01,0x08,0x08,0x08,0x08,0x08,0x08,0x08,0x08,
            0x41,0x41,0x02,0x00,0x40,0x40,0x41,0x44,0x03,0x00,0x02,0x00,0x00,0x01,0x00,0x00,
            0x40,0x40,0x40,0x40,0x01,0x01,0x00,0x00,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,
            0x21,0x21,0x21,0x21,0x01,0x01,0x01,0x01,0x24,0x24,0x06,0x21,0x00,0x00,0x00,0x00,
            0x80,0x00,0x80,0x80,0x00,0x00,0x40,0x40,0x00,0x00,0x00,0x00,0x00,0x00,0x40,0x40,
        };
        #endregion

        #region flags_table_ex
        static byte[] flags_table_ex = {
            0x40,0x40,0x40,0x40,0x80,0x00,0x00,0x00,0x00,0x00,0x80,0x00,0x80,0x40,0x80,0x41,
            0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x80,0x80,0x80,0x80,0x80,0x80,0x00,
            0x40,0x40,0x40,0x40,0x50,0x80,0x40,0x80,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,
            0x00,0x00,0x00,0x00,0x00,0x00,0x80,0x00,0x50,0x80,0x51,0x80,0x80,0x80,0x80,0x80,
            0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,
            0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,
            0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,
            0x41,0x41,0x41,0x41,0x40,0x40,0x40,0x00,0x40,0x40,0x80,0x80,0x40,0x40,0x40,0x40,
            0x24,0x24,0x24,0x24,0x24,0x24,0x24,0x24,0x24,0x24,0x24,0x24,0x24,0x24,0x24,0x24,
            0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,
            0x00,0x00,0x00,0x40,0x41,0x40,0x80,0x80,0x00,0x00,0x00,0x40,0x41,0x40,0x40,0x40,
            0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x41,0x40,0x40,0x40,0x40,0x40,
            0x40,0x40,0x41,0x40,0x41,0x41,0x41,0x40,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,
            0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,
            0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x40,0x80,
        };
        #endregion

        #endregion

        public IFlags IFlag { get; private set; }
        public byte Rex { get; private set; }
        public byte ModRM { get; private set; }
        public byte Sib { get; private set; }

        public int Opcode { get; private set; }
        public int OpcodeOffset { get; private set; }
        public int OpcodeSize { get; private set; } = 1;

        public uint Disp { get; private set; }
        public int DispOffset { get; private set; }
        public int DispSize { get; private set; }

        public ulong Imm { get; private set; }
        public int ImmOffset { get; private set; }
        public int ImmSize { get; private set; }

        public int Length { get; private set; }

        public override string ToString()
        {
            var f = IFlag.ToString().Replace(", ", "|");
            return $"Flag: ({f}), Rex: 0x{Rex:X2}, ModRM: 0x{ModRM:X2}, Sib: 0x{Sib:X2}\r\n"
                + $"Opcode: [{OpcodeOffset}+{OpcodeSize}] 0x{Opcode:X}\r\n"
                + $"Disp:   [{DispOffset}+{DispSize}] 0x{Disp:X}\r\n"
                + $"Imm:    [{ImmOffset}+{ImmSize}] 0x{Imm:X}\r\n";
        }

        public ILDasm(byte[] buffer, bool is64, int index = 0)
        {
            bool pr_67 = false;
            bool pr_66 = false;
            RFlag flag = RFlag.None;

            #region phase 1: parse prefixies

            while (((RFlag)flags_table[buffer[index]] & RFlag.Prefix) != 0)
            {
                if (buffer[index] == 0x66)
                    pr_66 = true;
                if (buffer[index] == 0x67)
                    pr_67 = true;

                index++;
                Length++;
                IFlag |= IFlags.Prefix;

                if (Length == 15)
                {
                    IFlag |= IFlags.Invalid;
                    return;
                }
            }


            // parse REX prefix
            byte rexw = 0;
            if (is64 && buffer[index] >> 4 == 4)
            {
                Rex = buffer[index];
                rexw = (byte)((Rex >> 3) & 1);
                IFlag |= IFlags.Rex;

                index++;
                Length++;
            }

            // can be only one REX prefix
            if (is64 && buffer[index] >> 4 == 4)
            {
                IFlag |= IFlags.Invalid;
                Length++;
                return;
            }

            #endregion

            #region phase 2: parse opcode

            OpcodeOffset = index;
            Length++;

            var op = buffer[index++];

            // is 2 byte opcode?
            if (op == 0x0F)
            {
                op = buffer[index++];
                Length++;
                OpcodeSize++;
                flag = (RFlag)flags_table_ex[op];
                Opcode |= op << 8;

                if ((flag & RFlag.Invalid) != 0)
                {
                    IFlag |= IFlags.Invalid;
                    return;
                }

                // for SSE instructions
                if ((flag & RFlag.Extended) != 0)
                {
                    op = buffer[index++];
                    Length++;
                    OpcodeSize++;
                    Opcode |= op << 16;
                }
            }
            else
            {
                flag = (RFlag)flags_table[op];
                // pr_66 = pr_67 for opcodes A0-A3
                if (op >= 0xA0 && op <= 0xA3)
                    pr_66 = pr_67;

                Opcode = op;
            }

            #endregion

            #region phase 3: parse ModR/M, SIB and DISP

            if ((flag & RFlag.ModRM) != 0)
            {
                byte mod = (byte)(buffer[index] >> 6);
                byte ro = (byte)((buffer[index] & 0x38) >> 3);
                byte rm = (byte)(buffer[index] & 7);

                ModRM = buffer[index++];
                Length++;
                IFlag |= IFlags.ModRM;

                // in F6,F7 opcodes immediate data present if R/O == 0
                if (op == 0xF6 && (ro == 0 || ro == 1))
                    flag |= RFlag.I8;

                if (op == 0xF7 && (ro == 0 || ro == 1))
                    flag |= RFlag.I16_I32_I64;

                // is Sib byte exist?
                if (mod != 3 && rm == 4 && !(!is64 && pr_67))
                {
                    Sib = buffer[index++];
                    Length++;
                    IFlag |= IFlags.Sib;

                    // if base == 5 and mod == 0
                    if ((Sib & 7) == 5 && mod == 0)
                    {
                        DispSize = 4;
                    }
                }

                switch (mod)
                {
                    case 0:
                        if (is64)
                        {
                            if (rm == 5)
                            {
                                DispSize = 4;
                                if (is64)
                                    IFlag |= IFlags.Relative;
                            }
                        }
                        else if (pr_67)
                        {
                            if (rm == 6)
                                DispSize = 2;
                        }
                        else
                        {
                            if (rm == 5)
                                DispSize = 4;
                        }
                        break;
                    case 1:
                        DispSize = 1;
                        break;
                    case 2:
                        if (is64)
                            DispSize = 4;
                        else if (pr_67)
                            DispSize = 2;
                        else
                            DispSize = 4;
                        break;
                }

                if (DispSize != 0)
                {
                    DispOffset = index;
                    index += DispSize;
                    Length += DispSize;
                    IFlag |= IFlags.Disp;

                    for (int i = 0; i < DispSize; ++i)
                        Disp |= (uint)buffer[DispOffset + i] << (i * 8);
                }
            }

            #endregion

            #region phase 4: parse immediate data

            if (rexw != 0 && (flag & RFlag.I16_I32_I64) != 0)
                ImmSize = 8;
            else if ((flag & RFlag.I16_I32) != 0 || (flag & RFlag.I16_I32_I64) != 0)
                ImmSize = (byte)(pr_66 ? 2 : 4);

            // if exist, add I16 and I8 size
            ImmSize += (byte)((byte)flag & 3);

            if (ImmSize != 0)
            {
                Length += ImmSize;
                ImmOffset = index;
                IFlag |= IFlags.Imm;

                if ((flag & RFlag.Relative) != 0)
                    IFlag |= IFlags.Relative;

                for (int i = 0; i < ImmSize; ++i)
                    Imm |= ((uint)buffer[ImmOffset + i] << (i * 8));
            }

            #endregion

            // instruction is too long
            if (Length > 15)
                IFlag |= IFlags.Invalid;
        }
    }

    public static class ILDasmExtensions
    {
        public static ILDasm GetILDasm(this byte[] b, bool is64Bit, int index = 0)
        {
            return new ILDasm(b, is64Bit, index);
        }
    }
}