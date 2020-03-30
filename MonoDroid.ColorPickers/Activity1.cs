using System;
using Android.App;
using Android.Graphics;
using Android.Widget;
using Android.OS;
using Cheesebaron.ColorPickers;

namespace Cheesebaron.ColorPickersSample
{
    [Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity
    {
        private Button _btNoAlpha;
        private Button _btAlpha;
        private Button _btRound;
        private ColorPickerPanelView _panelNoAlpha;
        private ColorPickerPanelView _panelAlpha;
        private ColorPickerPanelView _panelRound;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            _btNoAlpha = FindViewById<Button>(Resource.Id.ButtonColorNoAlpha);
            _btAlpha = FindViewById<Button>(Resource.Id.ButtonColorAlpha);
            _btRound = FindViewById<Button>(Resource.Id.ButtonRoundColor);

            _btNoAlpha.Click += BtNoAlphaOnClick;
            _btAlpha.Click += BtAlphaOnClick;
            _btRound.Click += BtRoundOnClick;

            _panelNoAlpha = FindViewById<ColorPickerPanelView>(Resource.Id.PanelColorNoAlpha);
            _panelNoAlpha.Color = Color.Black;
            _panelAlpha = FindViewById<ColorPickerPanelView>(Resource.Id.PanelColorAlpha);
            _panelAlpha.Color = Color.Black;
            _panelRound = FindViewById<ColorPickerPanelView>(Resource.Id.PanelRoundColor);
            _panelRound.Color = Color.Black;
        }

        private void BtRoundOnClick(object sender, EventArgs eventArgs)
        {
            var roundColorPickerDialog = new RoundColorPickerDialog(this, _panelRound.Color);
            roundColorPickerDialog.ColorChanged += (o, args) => _panelRound.Color = args.Color;
            roundColorPickerDialog.Show();
        }

        private void BtAlphaOnClick(object sender, EventArgs eventArgs)
        {
            using (var colorPickerDialog = new ColorPickerDialog(this, _panelAlpha.Color))
            {
                colorPickerDialog.AlphaSliderVisible = true;
                colorPickerDialog.ColorChanged += (o, args) => _panelAlpha.Color = args.Color;
                colorPickerDialog.Show();
            }
        }

        private void BtNoAlphaOnClick(object sender, EventArgs eventArgs)
        {
            using (var colorPickerDialog = new ColorPickerDialog(this, _panelNoAlpha.Color))
            {
                colorPickerDialog.ColorChanged += (o, args) => _panelNoAlpha.Color = args.Color;
                colorPickerDialog.Show();
            }
        }
    }
}

