using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nazeka
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void SetCursorPos(int X, int Y);

        RamGecTools.MouseHook mouseHook = new RamGecTools.MouseHook();
        RamGecTools.KeyboardHook keyboardHook = new RamGecTools.KeyboardHook();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //mouseHook.LeftButtonDown += new RamGecTools.MouseHook.MouseHookCallback(mouseHook_LeftButtonDown);
            //mouseHook.RightButtonDown += new RamGecTools.MouseHook.MouseHookCallback(mouseHook_RightButtonDown);
            mouseHook.MouseMove += new RamGecTools.MouseHook.MouseHookCallback(mouseHook_MouseMove);
            mouseHook.Install();

            keyboardHook.KeyDown += new RamGecTools.KeyboardHook.KeyboardHookCallback(keyboardHook_KeyDown);
            keyboardHook.KeyUp += new RamGecTools.KeyboardHook.KeyboardHookCallback(keyboardHook_KeyUp);
            keyboardHook.Install();
        }

        public System.Windows.Point GetMousePosition()
        {
            System.Drawing.Point point = System.Windows.Forms.Control.MousePosition;
            return new System.Windows.Point(point.X, point.Y);
        }

        private Boolean pushedState = false;
        private int keepMouseX;
        private int keepMouseY;

        private void keyboardHook_KeyUp(RamGecTools.KeyboardHook.VKeys key)
        {
            if (RamGecTools.KeyboardHook.VKeys.LCONTROL == key)
            {
                Debug.WriteLine("key up=CONTROL");
                pushedState = false;
            }

        }

        private void keyboardHook_KeyDown(RamGecTools.KeyboardHook.VKeys key)
        {
            if (RamGecTools.KeyboardHook.VKeys.LCONTROL == key)
            {
                Debug.WriteLine("key down=CONTROL x={0}, y={0}", keepMouseX, keepMouseY);
                keepMouseX = (int)GetMousePosition().X;
                keepMouseY = (int)GetMousePosition().Y;
                pushedState = true;
            }
        }

        bool mouseHook_MouseMove(RamGecTools.MouseHook.MSLLHOOKSTRUCT mouseStruct)
        {
            Debug.WriteLine(mouseStruct.pt.x.ToString());
            Debug.WriteLine(mouseStruct.pt.y.ToString());

            // MSLLHOOKSTRUCT structure
            // https://msdn.microsoft.com/ja-jp/library/windows/desktop/ms644970%28v=vs.85%29.aspx

            // Message 構造体
            // https://msdn.microsoft.com/ja-jp/library/system.windows.forms.message(v=vs.110).aspx

            if (pushedState)
            {
                if ((bool)RadioVertical.IsChecked)
                {
                    SetCursorPos(mouseStruct.pt.x, keepMouseY);
                }
                else
                {
                    SetCursorPos(keepMouseX, mouseStruct.pt.y);
                }

                return true;
            }

            return false;
        }

        private bool mouseHook_LeftButtonDown(RamGecTools.MouseHook.MSLLHOOKSTRUCT mouseStruct)
        {
            Debug.WriteLine("mouse left button down");

            return false;
        }

        bool mouseHook_RightButtonDown(RamGecTools.MouseHook.MSLLHOOKSTRUCT mouseStruct)
        {
            Debug.WriteLine("mouse right button down");

            return false;
        }
    }
}
