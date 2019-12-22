using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CommonControls.Attach
{
    /*用法： <PasswordBox MaxLength="10" 
local:PasswordBoxHelper.Attach="True" 
local:PasswordBoxHelper.Password="{Binding ElementName=window, Path=Code, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
local:PasswordBoxHelper.IsFocus="{Binding ElementName=window, Path=MyFocus}">
<PasswordBox.InputBindings>
<KeyBinding Key = "Enter" Command="{Binding LoginCommand}"/>
</PasswordBox.InputBindings>
</PasswordBox>*/

    /// <summary>
    /// Password控件是seal的，无法重写用依赖性属性 所以采用附加的方式赋值 
    /// </summary>
    public class MyPasswordBoxHelper
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password",
            typeof(string), typeof(MyPasswordBoxHelper),
            new PropertyMetadata(string.Empty, PasswordPropertyChanged));
        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached("Attach",
            typeof(bool), typeof(MyPasswordBoxHelper), new PropertyMetadata(false, Attach));
        public static readonly DependencyProperty IsFocusProperty =
            DependencyProperty.RegisterAttached("IsFocus",
                typeof(bool), typeof(MyPasswordBoxHelper),
                new FrameworkPropertyMetadata(false, OnIsFocusPropertyChanged));

        private static void OnIsFocusPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as FrameworkElement;
            control?.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Render, new Action(() => control.Focus()));//很奇怪的聚焦 非得用本身相互关联的线程才可以聚焦
        }

        public static bool GetIsFocus(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsFocusProperty);
        }

        public static void SetIsFocus(DependencyObject dp, bool value)
        {
            dp.SetValue(IsFocusProperty, value);
        }

        public static void SetAttach(DependencyObject dp, bool value)
        {
            dp.SetValue(AttachProperty, value);
        }
        public static bool GetAttach(DependencyObject dp)
        {
            return (bool)dp.GetValue(AttachProperty);
        }
        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }
        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }

        private static void PasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = d as PasswordBox;
            if (e.NewValue==null||string.IsNullOrWhiteSpace(e.NewValue.ToString()))
            {
                passwordBox.Password = "";
            }
        }
        private static void Attach(DependencyObject sender,
             DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            if (passwordBox == null)
                return;
            if ((bool)e.OldValue)
            {
                passwordBox.PasswordChanged -= PasswordChanged;
            }
            if ((bool)e.NewValue)
            {
                passwordBox.PasswordChanged += PasswordChanged;
                var dic = new ResourceDictionary { Source = new Uri("/CommonControls;component/AllCommonControlStyle.xaml", UriKind.RelativeOrAbsolute) };
                passwordBox.Style = dic["PasswordBoxWithWaterMark"] as Style;               
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            SetPassword(passwordBox, passwordBox.Password);
        }
    }
}
