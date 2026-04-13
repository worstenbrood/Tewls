using System;
using TewlKit.Hooking;

namespace TewlKit.Hooks
{
    public class MessageBoxHook : Hook<MessageBoxHook.MessageBox>
    {
        public delegate int MessageBox(IntPtr hwnd, string text, string caption, int type);

        /// <summary>
        /// Replacement for MessageBoxA. Modifies the caption to indicate it was hooked.
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int HookedMessageBox(IntPtr hwnd, string text, string caption, int type)
        {
            Console.WriteLine($"MessageBoxA called with text: {text}, caption: {caption}, type: {type}");
            Console.WriteLine($"Stub: {Trampoline.Stub.Address:X}");
            
            //return 0;
            return Stub(hwnd, text, "HOOKED: " + caption, type);
        }

        protected override MessageBox Replacement => HookedMessageBox;

        /// <summary>
        /// 
        /// </summary>
        public MessageBoxHook() : base("user32.dll", "MessageBoxA")
        {
           
        }
    }
}
