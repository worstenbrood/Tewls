using System;
using System.Runtime.InteropServices;
using static Tewls.Windows.WNet;

namespace Tewls.Windows.PE
{
    public enum ImageSignature : ushort
    {
        Dos = 0x5A4D
    }

    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public class ImageDosHeader
    {
        public ImageSignature e_magic;
        public ushort e_cblp;
        public ushort e_cp;
        public ushort e_crlc;
        public ushort e_cparhdr;
        public ushort e_minalloc;
        public ushort e_maxalloc;
        public ushort e_ss;
        public ushort e_sp;
        public ushort e_csum;
        public ushort e_ip;
        public ushort e_cs;
        public ushort e_lfarlc;
        public ushort e_ovno;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public ushort []e_res;
        public ushort e_oemid;
        public ushort e_oeminfo;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public ushort []e_res2;
        public int e_lfanew;
    };

    public enum ImageFileMachine : ushort
    {
        I386 = 0x014c,
        R3000 = 0x160,
        R4000 = 0x0166,
        R10000 = 0x0168,
        Wcemipsv2 = 0x0169,
        Alpha = 0x0184,
        Sh3 = 0x01a2,
        Sh3dsp = 0x01a3,
        Sh3e = 0x01a4,
        Sh4 = 0x01a6,
        Sh5 = 0x01a8,
        Arm = 0x01c0,
        Thumb = 0x01c2,
        Armnt = 0x01c4,
        Am33 = 0x01d3,
        Powerpc = 0x01F0,
        Powerpcfp = 0x01f1,
        Ia64 = 0x0200,
        Mips16 = 0x0266,
        Alpha64 = 0x0284,
        Mipsfpu = 0x0366,
        Mipsfpu16 = 0x0466,
        Tricore = 0x0520,
        Cef = 0x0CEF,
        Ebc = 0x0EBC,
        Amd64 = 0x8664,
        M32r = 0x9041,
        Cee = 0xC0EE,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct ImageFileHeader
    {
        public ImageFileMachine Machine;
        public ushort NumberOfSections;
        public uint TimeDateStamp;
        public uint PointerToSymbolTable;
        public uint NumberOfSymbols;
        public ushort SizeOfOptionalHeader;
        public ushort Characteristics;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct ImageDataDirectory
    {
        public uint VirtualAddress;
        public uint Size;
    };

    [Flags]
    public enum DllCharacteristics : ushort
    {
        HighEntropyVA = 0x0020,
        DynamicBase = 0x0040,
        ForceIntegrity = 0x0080,
        NxCompat = 0x0100,
        NoIsolation = 0x0200,
        NoSeh = 0x0400,
        NoBind = 0x0800,
        AppContainer = 0x1000,
        WdmDriver = 0x2000,
        GuardFc = 0x4000,
        TerminalServerAware = 0x8000
    }

    public enum ImageSubsystem : ushort
    {
        Unknown,		
        Natvie,			
        WindowsGui,	
        WindowsCui,		
        Os2Cui = 5,			
        PosixCui = 7,		
        NativeWindows,	
        WindowsCegui,	
        EfiApplication,	
        EfiBootServiceDriver,
        EfiRuntimeDriver,
        EfiRom,
        Xbox,	
        WindowsBootApplication = 16
    }

    public static class ImageDirectoryEntry
    {
        public const int Export = 0;
        public const int Import = 1;
        public const int Resource = 2;
        public const int Exception = 3;
        public const int Security = 4;
        public const int BaseReloc = 5;
        public const int Debug = 6;
        public const int Copyright = 7;
        public const int GlobalPtr = 8;
        public const int Tls = 9;
        public const int LoadConfig = 10;
        public const int BoundImport = 11;
        public const int Iat = 12;
        public const int DelayImport = 13;
        public const int ComDescriptor = 14;
    }

    public enum ImageNtOptional : ushort
    {
        Hdr32Magic = 0x10b,
        Hdr64Magic = 0x20b,
        HdrMagic =  0x107
    }

    public enum ImageNt : uint
    {
        Signature = 0x00004550
    }

    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct ImageOptionalHeader
    {
        public ImageNtOptional Magic;
        public byte MajorLinkerVersion;
        public byte MinorLinkerVersion;
        public uint SizeOfCode;
        public uint SizeOfInitializedData;
        public uint SizeOfUninitializedData;
        public uint AddressOfEntryPoint;
        public uint BaseOfCode;
        public ulong ImageBase;
        public uint SectionAlignment;
        public uint FileAlignment;
        public ushort MajorOperatingSystemVersion;
        public ushort MinorOperatingSystemVersion;
        public ushort MajorImageVersion;
        public ushort MinorImageVersion;
        public ushort MajorSubsystemVersion;
        public ushort MinorSubsystemVersion;
        public uint Win32VersionValue;
        public uint SizeOfImage;
        public uint SizeOfHeaders;
        public uint CheckSum;
        public ImageSubsystem Subsystem;
        public DllCharacteristics DllCharacteristics;
        public IntPtr SizeOfStackReserve;
        public IntPtr SizeOfStackCommit;
        public IntPtr SizeOfHeapReserve;
        public IntPtr SizeOfHeapCommit;
        public uint LoaderFlags;
        public uint NumberOfRvaAndSizes;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public ImageDataDirectory []DataDirectory;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct ImageOptionalHeader32
    {
        public ImageNtOptional Magic;
        public byte MajorLinkerVersion;
        public byte MinorLinkerVersion;
        public uint SizeOfCode;
        public uint SizeOfInitializedData;
        public uint SizeOfUninitializedData;
        public uint AddressOfEntryPoint;
        public uint BaseOfCode;
        public uint BaseOfData;
        public uint ImageBase;
        public uint SectionAlignment;
        public uint FileAlignment;
        public ushort MajorOperatingSystemVersion;
        public ushort MinorOperatingSystemVersion;
        public ushort MajorImageVersion;
        public ushort MinorImageVersion;
        public ushort MajorSubsystemVersion;
        public ushort MinorSubsystemVersion;
        public uint Win32VersionValue;
        public uint SizeOfImage;
        public uint SizeOfHeaders;
        public uint CheckSum;
        public ImageSubsystem Subsystem;
        public DllCharacteristics DllCharacteristics;
        public uint SizeOfStackReserve;
        public uint SizeOfStackCommit;
        public uint SizeOfHeapReserve;
        public uint SizeOfHeapCommit;
        public uint LoaderFlags;
        public uint NumberOfRvaAndSizes;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public ImageDataDirectory[] DataDirectory;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public class ImageNtHeaders
    {
        public ImageNt Signature;
        public ImageFileHeader FileHeader;
        public ImageOptionalHeader OptionalHeader;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public class ImageNtHeaders32
    {
        public ImageNt Signature;
        public ImageFileHeader FileHeader;
        public ImageOptionalHeader32 OptionalHeader;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public class ImageExportDirectory
    {
        public uint Characteristics;
        public uint TimeDateStamp;
        public ushort MajorVersion;
        public ushort MinorVersion;
        public uint Name;
        public uint Base;
        public uint NumberOfFunctions;
        public uint NumberOfNames;
        public uint AddressOfFunctions;
        public uint AddressOfNames;
        public uint AddressOfNameOrdinals;
    };
}
