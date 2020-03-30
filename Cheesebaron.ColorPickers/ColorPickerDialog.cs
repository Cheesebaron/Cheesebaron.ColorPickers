/*
 * Direct port to Mono for Android of https://github.com/attenzione/android-ColorPickerPreference
 * by Tomasz Cielecki <tomasz@ostebaronen.dk>
 * Whose license is:
 * 
 * Copyright (C) 2010 Daniel Nilsson
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Cheesebaron.ColorPickers
{
    [Register("cheesebaron.colorpickers.ColorPickerDialog")]
    public class ColorPickerDialog: Dialog, View.IOnClickListener
    {
        public event ColorChangedEventHandler ColorChanged;
        
        private bool _isDisposed;
        private ColorPickerView _colorPicker;

        private ColorPickerPanelView _oldColor;
        private ColorPickerPanelView _newColor;

        public Color Color => _colorPicker.Color;

        public bool AlphaSliderVisible
        {
            get => _colorPicker.AlphaSliderVisible;
            set => _colorPicker.AlphaSliderVisible = value;
        }

        protected ColorPickerDialog(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Init(Color.BlanchedAlmond);
        }

        public ColorPickerDialog(Context context, Color initialColor) : base(context)
        {
            Init(initialColor);
        }

        private void Init(Color color)
        {
            // To fight color banding.
            Window.SetFormat(Format.Rgba8888);

            SetUp(color);
        }

        private void SetUp(Color color)
        {
            var inflater = LayoutInflater.FromContext(Context);

		    var layout = inflater.Inflate(Resource.Layout.dialog_color_picker, null);

		    SetContentView(layout);

		    SetTitle("Press Color to Apply");

            _colorPicker = layout.FindViewById<ColorPickerView>(Resource.Id.color_picker_view);
            _oldColor = layout.FindViewById<ColorPickerPanelView>(Resource.Id.old_color_panel);
            _newColor = layout.FindViewById<ColorPickerPanelView>(Resource.Id.new_color_panel);

            ((LinearLayout)_oldColor.Parent).SetPadding(
                (int) Math.Round(_colorPicker.DrawingOffset), 
			    0,
                (int) Math.Round(_colorPicker.DrawingOffset), 
			    0
		    );

            _oldColor.Color = color;
            _colorPicker.Color = color;
        }

        public override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();

            _oldColor.SetOnClickListener(this);
            _newColor.SetOnClickListener(this);
            _colorPicker.ColorChanged += OnColorChanged;
        }

        public override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();

            _oldColor.SetOnClickListener(null);
            _newColor.SetOnClickListener(null);
            _colorPicker.ColorChanged -= OnColorChanged;
        }

        private void OnColorChanged(object sender, ColorChangedEventArgs args)
        {
            _newColor.Color = args.Color;
            ColorChanged?.Invoke(this, new ColorChangedEventArgs { Color = _newColor.Color });
        }

        public void OnClick(View v)
        {
            if (v.Id == Resource.Id.new_color_panel)
            {
                ColorChanged?.Invoke(this, new ColorChangedEventArgs { Color = _newColor.Color });
            }
            else if (v.Id == Resource.Id.old_color_panel)
            {
                ColorChanged?.Invoke(this, new ColorChangedEventArgs { Color = _oldColor.Color });
            }

            Dismiss();
        }

        public override Bundle OnSaveInstanceState()
        {
            var state = base.OnSaveInstanceState();
            state.PutInt("old_color", _oldColor.Color);
            state.PutInt("new_color", _newColor.Color);
            return state;
        }

        public override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            _oldColor.Color = new Color(savedInstanceState.GetInt("old_color"));
            _colorPicker.Color = new Color(savedInstanceState.GetInt("new_color"));
            base.OnRestoreInstanceState(savedInstanceState);
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            if (disposing)
            {
                if (_colorPicker != null)
                {
                    _colorPicker.ColorChanged -= OnColorChanged;
                    _colorPicker.Dispose();
                }
                
                if (_oldColor != null)
                {
                    _oldColor.SetOnClickListener(null);
                    _oldColor.Dispose();
                }
                
                if (_newColor != null)
                {
                    _newColor.SetOnClickListener(null);
                    _newColor.Dispose();
                }
            }

            _isDisposed = true;

            base.Dispose(disposing);
        }
    }
}