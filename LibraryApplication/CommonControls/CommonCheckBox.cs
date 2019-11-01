﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CommonControls
{
    /*
     *             <!--<commoncontrol:CommonCheckBox 
                NormalImageSource="Image/1OFF.png" 
                ImageWidth="131" 
                ImageHeight="128"
                SelectImageSource="Image/2OFF.png"
                CheckImageSource="Image/1ON.png" 
                HorizontalAlignment="Center" 
                Background="Transparent" 
                BorderThickness="0"></commoncontrol:CommonCheckBox>-->//单纯的图片切换
            <!--<commoncontrol:CommonCheckBox 
                Content="11111"
                CheckBackground="Blue"
                MouseOverBackground="Red"
                CheckForeground="White"
                CheckBorderBrush="Aqua"
                HorizontalAlignment="Center" ></commoncontrol:CommonCheckBox>-->可附带图片背景等
     */
    public class CommonCheckBox : CheckBox
    {
        #region 角度
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(CommonCheckBox), new PropertyMetadata(new CornerRadius(0)));
        #endregion

        #region 鼠标滑过颜色


        public Color? MouseOverBackground
        {
            get { return (Color?)GetValue(MouseOverColorProperty); }
            set { SetValue(MouseOverColorProperty, value); }
        }
        public static readonly DependencyProperty MouseOverColorProperty =
             DependencyProperty.Register("MouseOverBackground", typeof(Color?), typeof(CommonCheckBox), new PropertyMetadata(null));

        public Color? MouseOverBorderBrush
        {
            get { return (Color?)GetValue(MouseOverBorderBrushProperty); }
            set { SetValue(MouseOverBorderBrushProperty, value); }
        }
        public static readonly DependencyProperty MouseOverBorderBrushProperty =
             DependencyProperty.Register("MouseOverBorderBrush", typeof(Color?), typeof(CommonCheckBox), new PropertyMetadata(null));

        public Color? MouseOverForeground
        {
            get { return (Color?)GetValue(MouseOverForegroundProperty); }
            set { SetValue(MouseOverForegroundProperty, value); }
        }
        public static readonly DependencyProperty MouseOverForegroundProperty =
             DependencyProperty.Register("MouseOverForeground", typeof(Color?), typeof(CommonCheckBox), new PropertyMetadata(null));

        public Thickness? MouseOverThickness
        {
            get { return (Thickness?)GetValue(MouseOverThicknessProperty); }
            set { SetValue(MouseOverThicknessProperty, value); }
        }
        public static readonly DependencyProperty MouseOverThicknessProperty =
             DependencyProperty.Register("MouseOverThickness", typeof(Thickness?), typeof(CommonCheckBox), new PropertyMetadata(null));

        #endregion

        #region 鼠标点击颜色


        public Color? PressedBackground
        {
            get { return (Color?)GetValue(PressedBackgroundProperty); }
            set { SetValue(PressedBackgroundProperty, value); }
        }
        public static readonly DependencyProperty PressedBackgroundProperty =
             DependencyProperty.Register("PressedBackground", typeof(Color?), typeof(CommonCheckBox), new PropertyMetadata(null));

        public Color? PressedBorderBrush
        {
            get { return (Color?)GetValue(PressedBorderBrushProperty); }
            set { SetValue(PressedBorderBrushProperty, value); }
        }
        public static readonly DependencyProperty PressedBorderBrushProperty =
             DependencyProperty.Register("PressedBorderBrush", typeof(Color?), typeof(CommonCheckBox), new PropertyMetadata(null));

        public Color? PressedForeground
        {
            get { return (Color?)GetValue(PressedForegroundProperty); }
            set { SetValue(PressedForegroundProperty, value); }
        }
        public static readonly DependencyProperty PressedForegroundProperty =
             DependencyProperty.Register("PressedForeground", typeof(Color?), typeof(CommonCheckBox), new PropertyMetadata(null));

        public Thickness? PressedThickness
        {
            get { return (Thickness?)GetValue(PressedThicknessProperty); }
            set { SetValue(PressedThicknessProperty, value); }
        }
        public static readonly DependencyProperty PressedThicknessProperty =
             DependencyProperty.Register("PressedThickness", typeof(Thickness?), typeof(CommonCheckBox), new PropertyMetadata(null));

        #endregion

        #region 按钮不可用颜色
        public Color? DisabledBackground
        {
            get { return (Color?)GetValue(DisabledBackgroundProperty); }
            set { SetValue(DisabledBackgroundProperty, value); }
        }
        public static readonly DependencyProperty DisabledBackgroundProperty =
             DependencyProperty.Register("DisabledBackground", typeof(Color?), typeof(CommonCheckBox), new PropertyMetadata(null));

        public Color? DisabledBorderBrush
        {
            get { return (Color?)GetValue(DisabledBorderBrushProperty); }
            set { SetValue(DisabledBorderBrushProperty, value); }
        }
        public static readonly DependencyProperty DisabledBorderBrushProperty =
             DependencyProperty.Register("DisabledBorderBrush", typeof(Color?), typeof(CommonCheckBox), new PropertyMetadata(null));

        public Color? DisabledForeground
        {
            get { return (Color?)GetValue(DisabledForegroundProperty); }
            set { SetValue(DisabledForegroundProperty, value); }
        }
        public static readonly DependencyProperty DisabledForegroundProperty =
             DependencyProperty.Register("DisabledForeground", typeof(Color?), typeof(CommonCheckBox), new PropertyMetadata(null));

        public Thickness? DisabledThickness
        {
            get { return (Thickness?)GetValue(DisabledThicknessProperty); }
            set { SetValue(DisabledThicknessProperty, value); }
        }
        public static readonly DependencyProperty DisabledThicknessProperty =
             DependencyProperty.Register("DisabledThickness", typeof(Thickness?), typeof(CommonCheckBox), new PropertyMetadata(null));
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
            DependencyProperty.Register("StackPanelMargin", typeof(Thickness), typeof(CommonCheckBox), new PropertyMetadata(new Thickness(0)));

        public Orientation StackPanelOrientation
        {
            get { return (Orientation)GetValue(StackPanelOrientationProperty); }
            set { SetValue(StackPanelOrientationProperty, value); }
        }
        public static readonly DependencyProperty StackPanelOrientationProperty =
             DependencyProperty.Register("StackPanelOrientation", typeof(Orientation), typeof(CommonCheckBox), new PropertyMetadata(Orientation.Horizontal));

        public HorizontalAlignment StackPanelHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(StackPanelHorizontalAlignmentProperty); }
            set { SetValue(StackPanelHorizontalAlignmentProperty, value); }
        }
        public static readonly DependencyProperty StackPanelHorizontalAlignmentProperty =
             DependencyProperty.Register("StackPanelHorizontalAlignment", typeof(HorizontalAlignment), typeof(CommonCheckBox), new PropertyMetadata(HorizontalAlignment.Center));

        public VerticalAlignment StackPanelVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(StackPanelVerticalAlignmentProperty); }
            set { SetValue(StackPanelVerticalAlignmentProperty, value); }
        }
        public static readonly DependencyProperty StackPanelVerticalAlignmentProperty =
             DependencyProperty.Register("StackPanelVerticalAlignment", typeof(VerticalAlignment), typeof(CommonCheckBox), new PropertyMetadata(VerticalAlignment.Center));

        #endregion

        #region Image

        public ImageSource NormalImageSource
        {
            get { return (ImageSource)GetValue(NormalImageSourceProperty); }
            set { SetValue(NormalImageSourceProperty, value); }
        }
        public static readonly DependencyProperty NormalImageSourceProperty =
             DependencyProperty.Register("NormalImageSource", typeof(ImageSource), typeof(CommonCheckBox), new PropertyMetadata(null));

        public ImageSource SelectImageSource
        {
            get { return (ImageSource)GetValue(SelectImageSourceProperty); }
            set { SetValue(SelectImageSourceProperty, value); }
        }
        public static readonly DependencyProperty SelectImageSourceProperty =
             DependencyProperty.Register("SelectImageSource", typeof(ImageSource), typeof(CommonCheckBox), new PropertyMetadata(null));

        public ImageSource CheckImageSource
        {
            get { return (ImageSource)GetValue(CheckImageSourceProperty); }
            set { SetValue(CheckImageSourceProperty, value); }
        }
        public static readonly DependencyProperty CheckImageSourceProperty =
            DependencyProperty.Register("CheckImageSource", typeof(ImageSource), typeof(CommonCheckBox), new PropertyMetadata(null));

        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }
        public static readonly DependencyProperty ImageWidthProperty =
             DependencyProperty.Register("ImageWidth", typeof(double), typeof(CommonCheckBox), new PropertyMetadata(0.0));

        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }
        public static readonly DependencyProperty ImageHeightProperty =
             DependencyProperty.Register("ImageHeight", typeof(double), typeof(CommonCheckBox), new PropertyMetadata(0.0));

        #endregion

        #region Check后的颜色

        public Color? CheckBackground
        {
            get { return (Color?)GetValue(CheckBackgroundColorProperty); }
            set { SetValue(CheckBackgroundColorProperty, value); }
        }
        public static readonly DependencyProperty CheckBackgroundColorProperty =
            DependencyProperty.Register("CheckBackground", typeof(Color?), typeof(CommonCheckBox), new PropertyMetadata(null));

        public Color? CheckBorderBrush
        {
            get { return (Color?)GetValue(CheckBorderBrushProperty); }
            set { SetValue(CheckBorderBrushProperty, value); }
        }
        public static readonly DependencyProperty CheckBorderBrushProperty =
            DependencyProperty.Register("CheckBorderBrush", typeof(Color?), typeof(CommonCheckBox), new PropertyMetadata(null));

        public Color? CheckForeground
        {
            get { return (Color?)GetValue(CheckForegroundProperty); }
            set { SetValue(CheckForegroundProperty, value); }
        }
        public static readonly DependencyProperty CheckForegroundProperty =
            DependencyProperty.Register("CheckForeground", typeof(Color?), typeof(CommonCheckBox), new PropertyMetadata(null));

        public Thickness? CheckThickness
        {
            get { return (Thickness?)GetValue(CheckThicknessProperty); }
            set { SetValue(CheckThicknessProperty, value); }
        }
        public static readonly DependencyProperty CheckThicknessProperty =
            DependencyProperty.Register("CheckThickness", typeof(Thickness?), typeof(CommonCheckBox), new PropertyMetadata(null));



        #endregion

        static CommonCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CommonCheckBox), new FrameworkPropertyMetadata(typeof(CommonCheckBox)));
        }


    }
}
