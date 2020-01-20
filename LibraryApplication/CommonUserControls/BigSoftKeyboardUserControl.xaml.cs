using BasicFunction.Helper;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CommonUserControls
{
    /// <summary>
    /// BigSoftKeyboardUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class BigSoftKeyboardUserControl : System.Windows.Controls.UserControl
    {
        private static Storyboard storyboard;
        public BigSoftKeyboardUserControl()
        {
            InitializeComponent();
            Loaded += BigSoftKeyboardUserControl_Loaded;
        }

        static BigSoftKeyboardUserControl()
        {
            
        }

        private void BigSoftKeyboardUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CheckBox.IsChecked = false;
            if (Win32Helper.CapsLockStatus)
            {
                InputCommand?.Execute("CapsLock");//恢复小写
            }
            if (storyboard==null)
            {
                storyboard = new Storyboard();
                var d = new DoubleAnimation() { Duration = TimeSpan.FromSeconds(0.8), EasingFunction = new CubicEase() };
                d.From = this.ActualHeight;
                d.To = 0;
                storyboard.Children.Add(d);
                Storyboard.SetTarget(d, RootGrid);
                Storyboard.SetTargetProperty(d, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)"));
            }
            storyboard?.Begin();
        }

        public ICommand InputCommand => new RelayCommand<string>(t =>
        {
            switch (t)
            {
                case "0":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.D0);
                    break;
                case "1":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.D1);
                    break;
                case "2":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.D2);
                    break;
                case "3":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.D3);
                    break;
                case "4":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.D4);
                    break;
                case "5":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.D5);
                    break;
                case "6":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.D6);
                    break;
                case "7":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.D7);
                    break;
                case "8":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.D8);
                    break;
                case "9":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.D9);
                    break;
                case "q":
                case "Q":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.Q);
                    break;
                case "w":
                case "W":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.W);
                    break;
                case "e":
                case "E":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.E);
                    break;
                case "r":
                case "R":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.R);
                    break;
                case "t":
                case "T":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.T);
                    break;
                case "y":
                case "Y":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.Y);
                    break;
                case "u":
                case "U":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.U);
                    break;
                case "i":
                case "I":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.I);
                    break;
                case "o":
                case "O":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.O);
                    break;
                case "p":
                case "P":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.P);
                    break;
                case "a":
                case "A":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.A);
                    break;
                case "s":
                case "S":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.S);
                    break;
                case "d":
                case "D":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.D);
                    break;
                case "f":
                case "F":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.F);
                    break;
                case "g":
                case "G":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.G);
                    break;
                case "h":
                case "H":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.H);
                    break;
                case "j":
                case "J":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.J);
                    break;
                case "k":
                case "K":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.K);
                    break;
                case "l":
                case "L":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.L);
                    break;
                case "z":
                case "Z":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.Z);
                    break;
                case "x":
                case "X":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.X);
                    break;
                case "c":
                case "C":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.C);
                    break;
                case "v":
                case "V":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.V);
                    break;
                case "b":
                case "B":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.B);
                    break;
                case "n":
                case "N":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.N);
                    break;
                case "m":
                case "M":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.M);
                    break;
                case "CapsLock":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.CapsLock);
                    break;
                case "Backspace":
                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.Back);
                    break;
                case "Clear":
                    Win32Helper.SendKeyPressDown(Key.LeftCtrl);//按下ctrl
                    Win32Helper.SendKeyPressDown(Key.A);//同时按下a

                    Win32Helper.SendKeyPressUp(Key.LeftCtrl);
                    Win32Helper.SendKeyPressUp(Key.A);

                    Win32Helper.SendKeyByVirtualSoftKeyboard(Key.Back);
                    break;
            }
        });
    }
}
