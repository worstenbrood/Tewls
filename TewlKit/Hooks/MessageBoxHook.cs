using System;
using TewlKit.Core;

namespace TewlKit.Hooks
{
    public class MessageBoxHook : Hook<MessageBoxHook.MessageBox>
    {
        public delegate int MessageBox(IntPtr hwnd, string text, string caption, int type);

        public int HookedMessageBox(IntPtr hwnd, string text, string caption, int type)
        {
            return Trampoline.Original.Method.Invoke(hwnd, text, "HOOKED: " + caption, type);
        }

        public MessageBoxHook(IntPtr procAddr) : base("kernel32.dll", "OpenProcess")
        {
            Trampoline = new Trampoline<MessageBox>(procAddr, new MessageBox(HookedMessageBox));
        }
    }
}
