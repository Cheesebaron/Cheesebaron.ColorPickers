/*
 * Derivative work of ColorPickerDialog.java from Android SDK API samples,
 * ported to Mono for Android and added .NET style event handling.
 * 
 * Copyright (C) 2007 The Android Open Source Project
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

namespace Cheesebaron.ColorPickers
{
    [Register("cheesebaron.colorpickers.RoundColorPickerDialog")]
    public class RoundColorPickerDialog : Dialog
    {
        public event ColorChangedEventHandler ColorChanged;

        private readonly string _title;

        private bool _isDisposed;
        private static Color _initialColor;
        private RoundColorPickerView _pickerView;

        public RoundColorPickerDialog(Context context, Color initialColor, string title)
            : base(context)
        {
            _initialColor = initialColor;
            _title = title;
        }

        protected RoundColorPickerDialog(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            _initialColor = Color.Black;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _pickerView = new RoundColorPickerView(Context, _initialColor);
            
            SetContentView(_pickerView, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent));
            SetTitle(_title);
        }

        private void OnColorChanged(object sender, ColorChangedEventArgs args)
        {
            ColorChanged?.Invoke(this, args);

            Dismiss();
        }

        public override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();

            _pickerView.ColorChanged += OnColorChanged;
        }

        public override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();
            
            _pickerView.ColorChanged -= OnColorChanged;
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            if (disposing)
            {
                if (_pickerView != null)
                {
                    _pickerView.ColorChanged -= OnColorChanged;
                    _pickerView.Dispose();
                }
            }

            _isDisposed = true;

            base.Dispose(disposing);
        }
    }
}