using System;
using System.Runtime.InteropServices;

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
        HIGH_ENTROPY_VA = 0x0020,
        DYNAMIC_BASE = 0x0040,
        FORCE_INTEGRITY = 0x0080,
        NX_COMPAT = 0x0100,
        NO_ISOLATION = 0x0200,
        NO_SEH = 0x0400,
        NO_BIND = 0x0800,
        APPCONTAINER = 0x1000,
        WDM_DRIVER = 0x2000,
        GUARD_CF = 0x4000,
        TERMINAL_SERVER_AWARE = 0x8000
    }

    public enum ImageSubsystem : ushort
    {
        UNKNOWN,		
        NATIVE,			
        WINDOWS_GUI,	
        WINDOWS_CUI,		
        OS2_CUI = 5,			
        POSIX_CUI = 7,		
        NATIVE_WINDOWS,	
        WINDOWS_CE_GUI,	
        EFI_APPLICATION,	
        EFI_BOOT_SERVICE_DRIVER,
        EFI_RUNTIME_DRIVER,
        EFI_ROM,
        XBOX,	
        WINDOWS_BOOT_APPLICATION = 16
    }

    public enum ImageDirectoryEntry : int
    {
        EXPORT,		
        IMPORT,		
        RESOURCE,	
        EXCEPTION,	
        SECURITY,	
        BASERELOC,	
        DEBUG,		
        COPYRIGHT,	
        GLOBALPTR,	 
        TLS,		
        LOAD_CONFIG,
        BOUND_IMPORT,	
        IAT,	
        DELAY_IMPORT,
        COM_DESCRIPTOR,
    }

    public enum ImageNtOptional : ushort
    {
        HDR32_MAGIC = 0x10b,
        HDR64_MAGIC = 0x20b,
        HDR_MAGIC =  0x107
    }

    public enum ImageNtSignature : uint
    {
        PE32 = 0x10b,
        PE64 = 0x20b,
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
        public IntPtr ImageBase;
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
    public class ImageNtHeaders
    {
        public ImageNtSignature Signature;
        public ImageFileHeader FileHeader;
        public ImageOptionalHeader OptionalHeader;
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
