using System;
using TewlKit.Hooking;

namespace TewlKit.Hooks
{
    /// <summary>
    /// MessageBoxA hook. For testing.
    /// </summary>
    public class MessageBoxHook : Hook<MessageBoxHook.MessageBox>
    {
        /// <summary>
        /// Delegate for MessageBoxA. Matches the signature of the original function.
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <param name="type"></param>
        /// <returns></returns>
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
            Console.WriteLine($"Stub: {Trampoline.Stub.Address:X8}");
            
            //return 0;
            return Stub(hwnd, text, "HOOKED: " + caption, type);
        }

        /// <inheritdoc/>
        protected override MessageBox Replacement => HookedMessageBox;

        /// <summary>
        /// MessageBox hook constructor.
        /// </summary>
        public MessageBoxHook() : base("user32.dll", "MessageBoxA")
        {
        }
    }
}
