﻿// Christopher Braun 2016

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FMA.View.Common
{
    public partial class NumericUpDown : INotifyPropertyChanged
    {
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof (int), typeof (NumericUpDown), new PropertyMetadata(10000));

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof (int), typeof (NumericUpDown), new PropertyMetadata(-10000));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof (int), typeof (NumericUpDown),
                new FrameworkPropertyMetadata(0, ValueChanged) {BindsTwoWayByDefault = true});

        public static readonly DependencyProperty PressedBrushProperty = DependencyProperty.Register("PressedBrush",
            typeof (Brush), typeof (NumericUpDown),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty HoverBrushProperty = DependencyProperty.Register("HoverBrush",
            typeof (Brush), typeof (NumericUpDown),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty DefaultBrushProperty =
            DependencyProperty.Register("DefaultBrushProperty", typeof (Brush), typeof (NumericUpDown),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        private string lastValue = string.Empty;

        public NumericUpDown()
        {
            InitializeComponent();

            MinWidth = 52;

            PressedBrush = Brushes.Black;
            HoverBrush = new SolidColorBrush(SystemColors.HighlightColor);
            DefaultBrush = new SolidColorBrush(SystemColors.ControlColor);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetStaticBrushColor("PressedBrush", PressedBrush);
            SetStaticBrushColor("StandardBrush", DefaultBrush);
            SetStaticBrushColor("MouseOverBrush", HoverBrush);
        }

        private void SetStaticBrushColor(string resourceKey, Brush sourceBrush)
        {
            Resources[resourceKey] = sourceBrush.Clone();
        }

        public Brush PressedBrush
        {
            get { return (Brush) GetValue(PressedBrushProperty); }
            set { SetValue(PressedBrushProperty, value); }
        }

        public Brush HoverBrush
        {
            get { return (Brush) GetValue(HoverBrushProperty); }
            set { SetValue(HoverBrushProperty, value); }
        }

        public Brush DefaultBrush
        {
            get { return (Brush) GetValue(DefaultBrushProperty); }
            set { SetValue(DefaultBrushProperty, value); }
        }

        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as NumericUpDown;

            if (control != null && e.NewValue is int)
            {
                control.SetValue((int) e.NewValue);
            }
        }

        private void SetValue(int value)
        {
            var textBox = TextBox;
            if (textBox == null)
            {
                return;
            }

            var lastindex = false;
            var index = textBox.CaretIndex;
            if (index == textBox.Text.Length)
            {
                lastindex = true;
            }

            var text = value.ToString(CultureInfo.InvariantCulture);

            textBox.Text = text.Replace(",", ".");

            textBox.CaretIndex = lastindex ? textBox.Text.Length : index;
        }

        public int Maximum
        {
            get { return (int) GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public int Minimum
        {
            get { return (int) GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public int Value
        {
            get { return (int) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private void textBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var validInput = IsValidInput(e.Text);
            if (validInput == false)
            {
                e.Handled = true;
                return;
            }

            var textBox = sender as TextBox;
            if (textBox == null)
            {
                e.Handled = true;
                return;
            }

            textBox.SelectedText = string.Empty;

            var text = textBox.Text;

            var caretIndex = textBox.CaretIndex;

            switch (e.Text)
            {
                case "-":
                    if (ContainsSign(textBox.Text))
                    {
                        textBox.Text = text.Replace("-", "");
                        textBox.CaretIndex = caretIndex - 1;
                    }
                    else
                    {
                        textBox.Text = textBox.Text.Insert(0, e.Text);
                        textBox.CaretIndex = caretIndex + 1;
                    }
                    e.Handled = true;
                    return;
                case "+":
                    if (ContainsSign(textBox.Text))
                    {
                        textBox.Text = text.Replace("-", "");
                        textBox.CaretIndex = caretIndex - 1;
                    }
                    e.Handled = true;
                    return;
                default:
                    // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                    textBox.Text.Insert(textBox.CaretIndex, e.Text);
                    break;
            }

            if (ContainsSeperator(e.Text))
            {
                var containsSeperator = ContainsSeperator(textBox.Text);
                {
                    e.Handled = containsSeperator;
                    if (containsSeperator)
                    {
                        return;
                    }
                }
            }

            e.Handled = false;
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null)
            {
                return;
            }

            try
            {
                if (lastValue == textBox.Text)
                {
                    return;
                }

                lastValue = textBox.Text;

                if (textBox.Text.Contains(","))
                {
                    var index = textBox.CaretIndex;
                    textBox.Text = textBox.Text.Replace(",", ".");
                    textBox.CaretIndex = index;
                }

                if (textBox.Text.Contains("0"))
                {
                    var hasSign = false;

                    var actualText = textBox.Text;

                    if (ContainsSign(textBox.Text))
                    {
                        hasSign = true;
                        actualText = actualText.Replace("-", "");
                    }

                    var count = 0;

                    foreach (var nullChar in actualText)
                    {
                        if (nullChar == '0')
                        {
                            count++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (count > 1)
                    {
                        actualText = actualText.Remove(0, count - 1);
                        if (hasSign)
                        {
                            actualText = actualText.Insert(0, "-");
                        }

                        textBox.Text = actualText;
                        textBox.CaretIndex = 1;
                    }
                }

                int value;

                if (TryParseint(textBox.Text, out value))
                {
                    if (value < Minimum)
                    {
                        value = Minimum;
                        SetValue(value);
                    }

                    if (value > Maximum)
                    {
                        value = Maximum;
                        SetValue(value);
                    }

                    Value = value;
                    OnPropertyChanged("Value");
                }

                lastValue = textBox.Text;
            }
            catch (Exception)
            {
                lastValue = "0";
                textBox.Text = "0";
            }
        }

        private static bool TryParseint(string text, out int value)
        {
            return int.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
        }

        private void textBox1_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                IncreaseValue(-1);
            }
            else
            {
                IncreaseValue(1);
            }
        }

        private static bool ContainsSeperator(string text)
        {
            return text.Contains(".") || text.Contains(",");
        }

        private static bool ContainsSign(string text)
        {
            return text.Contains("-");
        }

        private static bool IsValidInput(string input)
        {
            var firstLetter = input[0];

            if (firstLetter.Equals('-'))
            {
                return true;
            }
            if (firstLetter.Equals('.'))
            {
                return true;
            }
            if (firstLetter.Equals(','))
            {
                return true;
            }
            if (char.IsNumber(firstLetter))
            {
                return true;
            }

            return false;
        }

        private void IncreaseValue(int delta)
        {
            var textBox = TextBox;
            if (textBox == null)
            {
                return;
            }

            int value;

            if (!TryParseint(textBox.Text, out value))
            {
                return;
            }
            var index = textBox.CaretIndex;
            value += delta;
            SetValue(value);
            textBox.CaretIndex = index;
        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
            IncreaseValue(-1);
        }

        private void Up_Click(object sender, RoutedEventArgs e)
        {
            IncreaseValue(1);
        }

        private void textBox1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    IncreaseValue(1);
                    break;
                case Key.Down:
                    IncreaseValue(-1);
                    break;
            }
        }
    }
}