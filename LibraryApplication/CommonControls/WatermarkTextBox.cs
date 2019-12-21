using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CommonControls
{
    public class WatermarkTextBox : TextBox
    {
        public static readonly DependencyProperty CornerRadiusProperty;
        public static readonly DependencyProperty IsFocusProperty;
        public static readonly DependencyProperty WaterMarkProperty;
        public static readonly DependencyProperty ValueRuleProperty;

        static WatermarkTextBox() 
        {
            IsFocusProperty= DependencyProperty.Register("IsFocus", typeof(bool), typeof(WatermarkTextBox), new PropertyMetadata(false, IsFocusCallBack));
            CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WatermarkTextBox), new PropertyMetadata(new CornerRadius(0)));
            WaterMarkProperty = DependencyProperty.Register("WaterMark", typeof(string), typeof(WatermarkTextBox), new PropertyMetadata(string.Empty));
            ValueRuleProperty = DependencyProperty.Register("ValueRule", typeof(string), typeof(WatermarkTextBox), new PropertyMetadata(string.Empty, ValueRuleCallBack));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WatermarkTextBox), new FrameworkPropertyMetadata(typeof(WatermarkTextBox)));
        }

        public WatermarkTextBox()
        {
            var dic = new ResourceDictionary { Source = new Uri("/CommonControls;component/AllCommonControlStyle.xaml", UriKind.RelativeOrAbsolute) };
            Style = dic["CommonTextBoxStyle"] as Style;
        }

        private static void IsFocusCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WatermarkTextBox;
            control?.Dispatcher?.Invoke(() =>
            {
                control.Focus(); //聚焦
            });
        }
        /// <summary>
        /// 设置textbox焦点
        /// </summary>
        public bool IsFocus
        {
            get { return (bool)GetValue(IsFocusProperty); }
            set { SetValue(IsFocusProperty, value); }
        }

        /// <summary>
        /// 水印
        /// </summary>
        public string WaterMark
        {
            get { return (string)GetValue(WaterMarkProperty); }
            set { SetValue(WaterMarkProperty, value); }
        }
        /// <summary>
        /// 圆角
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        

        private static void ValueRuleCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tb = d as WatermarkTextBox;
            if (tb != null)
            {
                tb.TextChanged -= TextBoxRegex_TextChanged;
                if (e.NewValue != null)
                {
                    tb.TextChanged += TextBoxRegex_TextChanged;
                }
            }
        }

        private static void TextBoxRegex_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as WatermarkTextBox;
            if (textBox == null || string.IsNullOrWhiteSpace(textBox.ValueRule)) return;
            var change = new TextChange[e.Changes.Count];
            e.Changes.CopyTo(change, 0);
            var offset = change[0].Offset;
            if (change[0].AddedLength <= 0) return;
            if (Regex.IsMatch(textBox.Text, textBox.ValueRule, RegexOptions.IgnoreCase)) return;
            //设置光标位置
            textBox.Text = textBox.Text.Remove(offset, change[0].AddedLength);
            textBox.Select(offset, 0);
        }

        //^[0-9]*$ 只有数字
        //^[a-zA-Z0-9_\u4e00-\u9fa5]{1,30}$ 字母大小写 下划线 文字 字数限定为1-30个
        //^[\u4e00-\u9fa5]$ 只能输入中文
        //^1[34578]\\d{9}$ 验证国内手机号码

        /// <summary>
        /// 正则表达的规则
        /// </summary>
        public string ValueRule
        {
            get { return (string)GetValue(ValueRuleProperty); }
            set { SetValue(ValueRuleProperty, value); }
        }
    }
}
