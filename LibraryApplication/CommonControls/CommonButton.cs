﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CommonControls
{
    ///使用方法
    /// xmlns:commoncontrol="clr-namespace:PCL.CommonControl;assembly=PCL.CommonControl"
    /*        <commoncontrol:CommonButton Content="111" Padding="0,0,8,0"
                                                        Background="Transparent"
                                                        BorderThickness="1"
                                                        NormalImageSource="Images/K1.png"
                                                        StackPanelOrientation="Horizontal"
                                                        StackPanelMargin="8,0,8,0"
                                                        ImageWidth="50"
                                                        ImageHeight="20"
                                                        SelectImageSource="Images/compareBtn2.png"
                                                        Cursor="Hand"
                                                        MouseOverForeground="White"
                                                        CornerRadius="4"
                                                        Opacity="0.5"                                                      
                        ></commoncontrol:CommonButton>*/

    /// <summary>
    /// 文字按钮 可改变字体，背景，边框颜色,添加图片等
    /// </summary>
    public class CommonButton : Button
    {
        #region 角度
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(CommonButton), new PropertyMetadata(new CornerRadius(0)));
        #endregion

        #region 鼠标滑过颜色


        public Color? MouseOverBackground
        {
            get { return (Color?)GetValue(MouseOverColorProperty); }
            set { SetValue(MouseOverColorProperty, value); }
        }
        public static readonly DependencyProperty MouseOverColorProperty =
             DependencyProperty.Register("MouseOverBackground", typeof(Color?), typeof(CommonButton), new PropertyMetadata(null));

        public Color? MouseOverBorderBrush
        {
            get { return (Color?)GetValue(MouseOverBorderBrushProperty); }
            set { SetValue(MouseOverBorderBrushProperty, value); }
        }
        public static readonly DependencyProperty MouseOverBorderBrushProperty =
             DependencyProperty.Register("MouseOverBorderBrush", typeof(Color?), typeof(CommonButton), new PropertyMetadata(null));

        public Color? MouseOverForeground
        {
            get { return (Color?)GetValue(MouseOverForegroundProperty); }
            set { SetValue(MouseOverForegroundProperty, value); }
        }
        public static readonly DependencyProperty MouseOverForegroundProperty =
             DependencyProperty.Register("MouseOverForeground", typeof(Color?), typeof(CommonButton), new PropertyMetadata(null));

        public Thickness? MouseOverThickness
        {
            get { return (Thickness?)GetValue(MouseOverThicknessProperty); }
            set { SetValue(MouseOverThicknessProperty, value); }
        }
        public static readonly DependencyProperty MouseOverThicknessProperty =
             DependencyProperty.Register("MouseOverThickness", typeof(Thickness?), typeof(CommonButton), new PropertyMetadata(null));

        #endregion

        #region 鼠标点击颜色


        public Color? PressedBackground
        {
            get { return (Color?)GetValue(PressedBackgroundProperty); }
            set { SetValue(PressedBackgroundProperty, value); }
        }
        public static readonly DependencyProperty PressedBackgroundProperty =
             DependencyProperty.Register("PressedBackground", typeof(Color?), typeof(CommonButton), new PropertyMetadata(null));

        public Color? PressedBorderBrush
        {
            get { return (Color?)GetValue(PressedBorderBrushProperty); }
            set { SetValue(PressedBorderBrushProperty, value); }
        }
        public static readonly DependencyProperty PressedBorderBrushProperty =
             DependencyProperty.Register("PressedBorderBrush", typeof(Color?), typeof(CommonButton), new PropertyMetadata(null));

        public Color? PressedForeground
        {
            get { return (Color?)GetValue(PressedForegroundProperty); }
            set { SetValue(PressedForegroundProperty, value); }
        }
        public static readonly DependencyProperty PressedForegroundProperty =
             DependencyProperty.Register("PressedForeground", typeof(Color?), typeof(CommonButton), new PropertyMetadata(null));

        public Thickness? PressedThickness
        {
            get { return (Thickness?)GetValue(PressedThicknessProperty); }
            set { SetValue(PressedThicknessProperty, value); }
        }
        public static readonly DependencyProperty PressedThicknessProperty =
             DependencyProperty.Register("PressedThickness", typeof(Thickness?), typeof(CommonButton), new PropertyMetadata(null));

        #endregion

        #region 按钮不可用颜色
        public Color? DisabledBackground
        {
            get { return (Color?)GetValue(DisabledBackgroundProperty); }
            set { SetValue(DisabledBackgroundProperty, value); }
        }
        public static readonly DependencyProperty DisabledBackgroundProperty =
             DependencyProperty.Register("DisabledBackground", typeof(Color?), typeof(CommonButton), new PropertyMetadata(null));

        public Color? DisabledBorderBrush
        {
            get { return (Color?)GetValue(DisabledBorderBrushProperty); }
            set { SetValue(DisabledBorderBrushProperty, value); }
        }
        public static readonly DependencyProperty DisabledBorderBrushProperty =
             DependencyProperty.Register("DisabledBorderBrush", typeof(Color?), typeof(CommonButton), new PropertyMetadata(null));

        public Color? DisabledForeground
        {
            get { return (Color?)GetValue(DisabledForegroundProperty); }
            set { SetValue(DisabledForegroundProperty, value); }
        }
        public static readonly DependencyProperty DisabledForegroundProperty =
             DependencyProperty.Register("DisabledForeground", typeof(Color?), typeof(CommonButton), new PropertyMetadata(null));

        public Thickness? DisabledThickness
        {
            get { return (Thickness?)GetValue(DisabledThicknessProperty); }
            set { SetValue(DisabledThicknessProperty, value); }
        }
        public static readonly DependencyProperty DisabledThicknessProperty =
             DependencyProperty.Register("DisabledThickness", typeof(Thickness?), typeof(CommonButton), new PropertyMetadata(null));
        #endregion

        #region StackPanel

        /// <summary>
        /// 当有图片时文字的margin
        /// </summary>
        public Thickness StackPanelMargin
        {
            get { return (Thickness)GetValue(StackPanelMarginProperty); }
            set { SetValue(StackPanelMarginProperty, value); }
        }
        public static readonly DependencyProperty StackPanelMarginProperty =
            DependencyProperty.Register("StackPanelMargin", typeof(Thickness), typeof(CommonButton), new PropertyMetadata(new Thickness(0)));

        public Orientation StackPanelOrientation
        {
            get { return (Orientation)GetValue(StackPanelOrientationProperty); }
            set { SetValue(StackPanelOrientationProperty, value); }
        }
        public static readonly DependencyProperty StackPanelOrientationProperty =
             DependencyProperty.Register("StackPanelOrientation", typeof(Orientation), typeof(CommonButton), new PropertyMetadata(Orientation.Horizontal));

        public HorizontalAlignment StackPanelHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(StackPanelHorizontalAlignmentProperty); }
            set { SetValue(StackPanelHorizontalAlignmentProperty, value); }
        }
        public static readonly DependencyProperty StackPanelHorizontalAlignmentProperty =
             DependencyProperty.Register("StackPanelHorizontalAlignment", typeof(HorizontalAlignment), typeof(CommonButton), new PropertyMetadata(HorizontalAlignment.Center));

        public VerticalAlignment StackPanelVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(StackPanelVerticalAlignmentProperty); }
            set { SetValue(StackPanelVerticalAlignmentProperty, value); }
        }
        public static readonly DependencyProperty StackPanelVerticalAlignmentProperty =
             DependencyProperty.Register("StackPanelVerticalAlignment", typeof(VerticalAlignment), typeof(CommonButton), new PropertyMetadata(VerticalAlignment.Center));

        #endregion

        #region Image

        public ImageSource NormalImageSource
        {
            get { return (ImageSource)GetValue(NormalImageSourceProperty); }
            set { SetValue(NormalImageSourceProperty, value); }
        }
        public static readonly DependencyProperty NormalImageSourceProperty =
             DependencyProperty.Register("NormalImageSource", typeof(ImageSource), typeof(CommonButton), new PropertyMetadata(null));

        public ImageSource SelectImageSource
        {
            get { return (ImageSource)GetValue(SelectImageSourceProperty); }
            set { SetValue(SelectImageSourceProperty, value); }
        }
        public static readonly DependencyProperty SelectImageSourceProperty =
             DependencyProperty.Register("SelectImageSource", typeof(ImageSource), typeof(CommonButton), new PropertyMetadata(null));

        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }
        public static readonly DependencyProperty ImageWidthProperty =
             DependencyProperty.Register("ImageWidth", typeof(double), typeof(CommonButton), new PropertyMetadata(double.NaN));

        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }
        public static readonly DependencyProperty ImageHeightProperty =
             DependencyProperty.Register("ImageHeight", typeof(double), typeof(CommonButton), new PropertyMetadata(double.NaN));

        #endregion

        public CommonButton()
        {
            var dic = new ResourceDictionary { Source = new Uri("/CommonControls;component/AllCommonControlStyle.xaml", UriKind.RelativeOrAbsolute) };
            Style = dic["CommonButtonStyle"] as Style;
        }

        static CommonButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CommonButton), new FrameworkPropertyMetadata(typeof(CommonButton)));
        }
    }
}
