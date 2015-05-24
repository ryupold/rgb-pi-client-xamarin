using System;
using Android.Widget;
using Android.Graphics;
using Android.Views;
using Android.Util;
using Android.Content;

namespace RGBPi.Android
{
	public class VerticalSeekBar : SeekBar {

		public VerticalSeekBar(Context context):base(context) {
			
		}

		public VerticalSeekBar(Context context, IAttributeSet attrs, int defStyle):base(context, attrs, defStyle) {
			
		}

		public VerticalSeekBar(Context context, IAttributeSet attrs):base(context, attrs) {
			
		}

		protected override void OnSizeChanged(int w, int h, int oldw, int oldh) {
			base.OnSizeChanged(h, w, oldh, oldw);
		}


		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec) {
			base.OnMeasure(heightMeasureSpec, widthMeasureSpec);
			SetMeasuredDimension(MeasuredHeight, MeasuredWidth);
		}

		protected override void OnDraw(Canvas c) {
			c.Rotate(-90);
			c.Translate(-Height, 0);

			base.OnDraw(c);
		}


		public override bool OnTouchEvent(MotionEvent ev) {
			if (!Enabled) {
				return false;
			}

			switch (ev.Action) {
			case MotionEventActions.Down:
			case MotionEventActions.Move:
			case MotionEventActions.Up:
				Progress = (Max - (int) (Max * ev.GetY() / Height));
				OnSizeChanged(Width, Height, 0, 0);

				break;

			case MotionEventActions.Cancel:
				break;
			}
			return true;
		}
	}
}

