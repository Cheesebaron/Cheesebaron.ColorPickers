using System;
using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace MonoDroid.ColorPickers
{
    [Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var colorPicker = new ColorPickerDialog(this, Color.Azure);
            colorPicker.ColorChanged += (sender, args) => Console.WriteLine("Color changed: {0}", args.Color);

            var colorPickerAlpha = new ColorPickerDialog(this, Color.Aqua);
            colorPickerAlpha.ColorChanged += (sender, args) => Console.WriteLine("Color changed: {0}", args.Color);
            colorPickerAlpha.AlphaSliderVisible = true;

            var ll = new LinearLayout(this)
                         {
                             LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.FillParent,
                                                                              ViewGroup.LayoutParams.FillParent)
                         };

            var btColorPicker = new Button(this) { Text = "Show Color Picker" };
            btColorPicker.Click += (sender, args) => colorPicker.Show();

            var btColorPickerAlpha = new Button(this) { Text = "Show Color Picker (Alpha)" };
            btColorPickerAlpha.Click += (sender, args) => colorPickerAlpha.Show();

            ll.AddView(btColorPicker);
            ll.AddView(btColorPickerAlpha);
            SetContentView(ll);
        }
    }
}

