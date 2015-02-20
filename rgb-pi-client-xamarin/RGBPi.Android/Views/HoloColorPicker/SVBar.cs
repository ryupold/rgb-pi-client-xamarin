/*
 * Copyright 2012 Lars Werkman
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using Android.Graphics;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.OS;
using Android.Content.Res;
using RGBPi.Android;
using Java.Lang;

namespace Com.Larswerkman.HolorColorPicker{

public class SVBar : View {

	/*
	 * Constants used to save/restore the instance state.
	 */
		private static readonly string STATE_PARENT = "parent";
		private static readonly string STATE_COLOR = "color";
		private static readonly string STATE_SATURATION = "saturation";
		private static readonly string STATE_VALUE = "value";
		private static readonly string STATE_ORIENTATION = "orientation";
	
	/**
	 * Constants used to identify orientation.
	 */
		private static readonly bool ORIENTATION_HORIZONTAL = true;
		private static readonly bool ORIENTATION_VERTICAL = false;
	
	/**
	 * Default orientation of the bar.
	 */
		private static readonly bool ORIENTATION_DEFAULT = ORIENTATION_HORIZONTAL;

	/**
	 * The thickness of the bar.
	 */
	private int mBarThickness;

	/**
	 * The length of the bar.
	 */
	private int mBarLength;
	private int mPreferredBarLength;

	/**
	 * The radius of the pointer.
	 */
	private int mBarPointerRadius;

	/**
	 * The radius of the halo of the pointer.
	 */
	private int mBarPointerHaloRadius;

	/**
	 * The position of the pointer on the bar.
	 */
	private int mBarPointerPosition;

	/**
	 * {@code Paint} instance used to draw the bar.
	 */
	private Paint mBarPaint;

	/**
	 * {@code Paint} instance used to draw the pointer.
	 */
	private Paint mBarPointerPaint;

	/**
	 * {@code Paint} instance used to draw the halo of the pointer.
	 */
	private Paint mBarPointerHaloPaint;

	/**
	 * The rectangle enclosing the bar.
	 */
	private RectF mBarRect = new RectF();

	/**
	 * {@code Shader} instance used to fill the shader of the paint.
	 */
	private Shader shader;

	/**
	 * {@code true} if the user clicked on the pointer to start the move mode. <br>
	 * {@code false} once the user stops touching the screen.
	 * 
	 * @see #onTouchEvent(android.view.MotionEvent)
	 */
	private bool mIsMovingPointer;

	/**
	 * The ARGB value of the currently selected color.
	 */
	private int mColor;

	/**
	 * An array of floats that can be build into a {@code Color} <br>
	 * Where we can extract the Saturation and Value from.
	 */
	private float[] mHSVColor = new float[3];

	/**
	 * Factor used to calculate the position to the Saturation/Value on the bar.
	 */
	private float mPosToSVFactor;

	/**
	 * Factor used to calculate the Saturation/Value to the postion on the bar.
	 */
	private float mSVToPosFactor;

	/**
	 * {@code ColorPicker} instance used to control the ColorPicker.
	 */
	private ColorPicker mPicker = null;

	/**
	 * Used to toggle orientation between vertical and horizontal.
	 */
		private bool mOrientation;

		public SVBar(Context context):base(context) {
		init(null, 0);
	}

		public SVBar(Context context, IAttributeSet attrs):base(context, attrs) {
		init(attrs, 0);
	}

		public SVBar(Context context, IAttributeSet attrs, int defStyle):base(context, attrs, defStyle) {
		init(attrs, defStyle);
	}

		private void init(IAttributeSet attrs, int defStyle) {
			TypedArray a = Context.ObtainStyledAttributes(attrs,
				Resource.Styleable.ColorBars, defStyle, 0);
			Resources b = Context.Resources;

		mBarThickness = a.GetDimensionPixelSize(
				Resource.Styleable.ColorBars_bar_thickness,
				b.GetDimensionPixelSize(Resource.Dimension.bar_thickness));
			mBarLength = a.GetDimensionPixelSize(Resource.Styleable.ColorBars_bar_length,
				b.GetDimensionPixelSize(Resource.Dimension.bar_length));
		mPreferredBarLength = mBarLength;
		mBarPointerRadius = a.GetDimensionPixelSize(
				Resource.Styleable.ColorBars_bar_pointer_radius,
				b.GetDimensionPixelSize(Resource.Dimension.bar_pointer_radius));
		mBarPointerHaloRadius = a.GetDimensionPixelSize(
				Resource.Styleable.ColorBars_bar_pointer_halo_radius,
				b.GetDimensionPixelSize(Resource.Dimension.bar_pointer_halo_radius));
		mOrientation = a.GetBoolean(
				Resource.Styleable.ColorBars_bar_orientation_horizontal, ORIENTATION_DEFAULT);

		a.Recycle();

			mBarPaint = new Paint(PaintFlags.AntiAlias);
		mBarPaint.SetShader(shader);

		mBarPointerPosition = (mBarLength / 2) + mBarPointerHaloRadius;

			mBarPointerHaloPaint = new Paint(PaintFlags.AntiAlias);
			mBarPointerHaloPaint.Color = (Color.Black);
		mBarPointerHaloPaint.Alpha = (0x50);

			mBarPointerPaint = new Paint(PaintFlags.AntiAlias);
			mBarPointerPaint.Color = new Android.Graphics.Color(unchecked((int)(0xff81ff00)));

		mPosToSVFactor = 1 / ((float) mBarLength / 2);
		mSVToPosFactor = ((float) mBarLength / 2) / 1;
	}

	
	protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec) {
		int intrinsicSize = mPreferredBarLength
				+ (mBarPointerHaloRadius * 2);

		// Variable orientation
		int measureSpec;
		if (mOrientation == ORIENTATION_HORIZONTAL) {
			measureSpec = widthMeasureSpec;
		}
		else {
			measureSpec = heightMeasureSpec;
		}
			int lengthMode = (int)MeasureSpec.GetMode(measureSpec);
		int lengthSize = MeasureSpec.GetSize(measureSpec);

		int length;
			if (lengthMode == (int)MeasureSpecMode.Exactly) {
			length = lengthSize;
		}
			else if (lengthMode == (int)MeasureSpecMode.AtMost) {
			length = Math.Min(intrinsicSize, lengthSize);
		}
		else {
			length = intrinsicSize;
		}

		int barPointerHaloRadiusx2 = mBarPointerHaloRadius * 2;
		mBarLength = length - barPointerHaloRadiusx2;
		if(mOrientation == ORIENTATION_VERTICAL) {
			SetMeasuredDimension(barPointerHaloRadiusx2,
			        	(mBarLength + barPointerHaloRadiusx2));
		}
		else {
			SetMeasuredDimension((mBarLength + barPointerHaloRadiusx2),
						barPointerHaloRadiusx2);
		}
	}

	
		protected override void OnSizeChanged(int w, int h, int oldw, int oldh) {
		base.OnSizeChanged(w, h, oldw, oldh);

		// Fill the rectangle instance based on orientation
		int x1, y1;
		if (mOrientation == ORIENTATION_HORIZONTAL) {
			x1 = (mBarLength + mBarPointerHaloRadius);
			y1 = mBarThickness;
			mBarLength = w - (mBarPointerHaloRadius * 2);
			mBarRect.Set(mBarPointerHaloRadius,
					(mBarPointerHaloRadius - (mBarThickness / 2)),
					(mBarLength + (mBarPointerHaloRadius)),
					(mBarPointerHaloRadius + (mBarThickness / 2)));
		}
		else {
			x1 = mBarThickness;
			y1 = (mBarLength + mBarPointerHaloRadius);
			mBarLength = h - (mBarPointerHaloRadius * 2);
			mBarRect.Set((mBarPointerHaloRadius - (mBarThickness / 2)),
					mBarPointerHaloRadius,
					(mBarPointerHaloRadius + (mBarThickness / 2)),
					(mBarLength + (mBarPointerHaloRadius)));
		}

		// Update variables that depend of mBarLength.
		if(!IsInEditMode){
			shader = new LinearGradient(mBarPointerHaloRadius, 0,
					x1, y1, new int[] {
						unchecked((int)0xffffffff), Color.HSVToColor(mHSVColor), unchecked((int)0xff000000) },
					null, Shader.TileMode.Clamp);
		} else {
			shader = new LinearGradient(mBarPointerHaloRadius, 0,
					x1, y1, new int[] {
						unchecked((int)0xffffffff), unchecked((int)0xff81ff00), unchecked((int)0xff000000) }, null,
					Shader.TileMode.Clamp);
				Color.ColorToHSV(new Android.Graphics.Color(unchecked((int)0xff81ff00)), mHSVColor);
		}
		
		mBarPaint.SetShader(shader);
		mPosToSVFactor = 1 / ((float) mBarLength / 2);
		mSVToPosFactor = ((float) mBarLength / 2) / 1;
		float[] hsvColor = new float[3];
			Color.ColorToHSV(new Android.Graphics.Color(mColor), hsvColor);
		if (hsvColor[1] < hsvColor[2]) {
			mBarPointerPosition = Math.Round((mSVToPosFactor * hsvColor[1])
					+ mBarPointerHaloRadius);
		} else {
			mBarPointerPosition = Math
					.Round((mSVToPosFactor * (1 - hsvColor[2]))
							+ mBarPointerHaloRadius + (mBarLength / 2));
		}
			if(IsInEditMode){
			mBarPointerPosition = (mBarLength / 2) + mBarPointerHaloRadius;
		}
	}

	
	protected override void OnDraw(Canvas canvas) {
		// Draw the bar.
		canvas.DrawRect(mBarRect, mBarPaint);

		// Calculate the center of the pointer.
		int cX, cY;
		if (mOrientation == ORIENTATION_HORIZONTAL) {
			cX = mBarPointerPosition;
			cY = mBarPointerHaloRadius;
		}
		else {
			cX = mBarPointerHaloRadius;
			cY = mBarPointerPosition;
		}
		
		// Draw the pointer halo.
		canvas.DrawCircle(cX, cY, mBarPointerHaloRadius, mBarPointerHaloPaint);
		// Draw the pointer.
		canvas.DrawCircle(cX, cY, mBarPointerRadius, mBarPointerPaint);
	}

	
		public override bool OnTouchEvent(MotionEvent evt) {
			Parent.RequestDisallowInterceptTouchEvent(true);

		// Convert coordinates to our internal coordinate system
		float dimen;
		if (mOrientation == ORIENTATION_HORIZONTAL) {
				dimen = evt.GetX();
		}
		else {
				dimen = evt.GetY();
		}

			switch (evt.Action) {
		case MotionEventActions.Down:
		    	mIsMovingPointer = true;
			// Check whether the user pressed on the pointer
			if (dimen >= (mBarPointerHaloRadius)
					&& dimen <= (mBarPointerHaloRadius + mBarLength)) {
				mBarPointerPosition = Math.Round(dimen);
				calculateColor(Math.Round(dimen));
					mBarPointerPaint.Color = new Android.Graphics.Color(mColor);
				Invalidate();
			}
			break;
			case MotionEventActions.Move:
			if (mIsMovingPointer) {
				// Move the the pointer on the bar.
				if (dimen >= mBarPointerHaloRadius
						&& dimen <= (mBarPointerHaloRadius + mBarLength)) {
					mBarPointerPosition = Math.Round(dimen);
					calculateColor(Math.Round(dimen));
						mBarPointerPaint.Color = new Android.Graphics.Color(mColor);
					if (mPicker != null) {
						mPicker.setNewCenterColor(mColor);
						mPicker.changeOpacityBarColor(mColor);
					}
					Invalidate();
				} else if (dimen < mBarPointerHaloRadius) {
					mBarPointerPosition = mBarPointerHaloRadius;
						mColor = Color.White;
						mBarPointerPaint.Color = new Android.Graphics.Color(mColor);
					if (mPicker != null) {
						mPicker.setNewCenterColor(mColor);
						mPicker.changeOpacityBarColor(mColor);
					}
					Invalidate();
				} else if (dimen > (mBarPointerHaloRadius + mBarLength)) {
					mBarPointerPosition = mBarPointerHaloRadius + mBarLength;
						mColor = Color.Black;
						mBarPointerPaint.Color = new Android.Graphics.Color(mColor);
					if (mPicker != null) {
						mPicker.setNewCenterColor(mColor);
						mPicker.changeOpacityBarColor(mColor);
					}
					Invalidate();
				}
			}
			break;
			case MotionEventActions.Up:
			mIsMovingPointer = false;
			break;
		}
		return true;
	}

	/**
	 * Set the pointer on the bar. With the saturation value.
	 * 
	 * @param saturation
	 *            float between 0 > 1
	 */
	public void setSaturation(float saturation) {
		mBarPointerPosition = Math.Round((mSVToPosFactor * saturation)
				+ mBarPointerHaloRadius);
		calculateColor(mBarPointerPosition);
			mBarPointerPaint.Color = new Android.Graphics.Color(mColor);
		// Check whether the Saturation/Value bar is added to the ColorPicker
		// wheel
		if (mPicker != null) {
			mPicker.setNewCenterColor(mColor);
			mPicker.changeOpacityBarColor(mColor);
		}
		Invalidate();
	}

	/**
	 * Set the pointer on the bar. With the Value value.
	 * 
	 * @param value
	 *            float between 0 > 1
	 */
	public void setValue(float value) {
		mBarPointerPosition = Math.Round((mSVToPosFactor * (1 - value))
				+ mBarPointerHaloRadius + (mBarLength / 2));
		calculateColor(mBarPointerPosition);
			mBarPointerPaint.Color = new Android.Graphics.Color(mColor);
		// Check whether the Saturation/Value bar is added to the ColorPicker
		// wheel
		if (mPicker != null) {
			mPicker.setNewCenterColor(mColor);
			mPicker.changeOpacityBarColor(mColor);
		}
		Invalidate();
	}

	/**
	 * Set the bar color. <br>
	 * <br>
	 * Its discouraged to use this method.
	 * 
	 * @param color
	 */
	public void setColor(int color) {
		int x1, y1;
		if(mOrientation) {
			x1 = (mBarLength + mBarPointerHaloRadius);
			y1 = mBarThickness;
		}        else {
			x1 = mBarThickness;
			y1 = (mBarLength + mBarPointerHaloRadius);
		}
		
			Color.ColorToHSV(new Android.Graphics.Color(color), mHSVColor);
		shader = new LinearGradient(mBarPointerHaloRadius, 0,
				x1, y1, new int[] {Color.White, color, Color.Black}, null,
				Shader.TileMode.Clamp);
			mBarPaint.SetShader(shader);
	    calculateColor(mBarPointerPosition);
			mBarPointerPaint.Color = new Android.Graphics.Color(mColor);
		if (mPicker != null) {
			mPicker.setNewCenterColor(mColor);
			if(mPicker.hasOpacityBar())
				mPicker.changeOpacityBarColor(mColor);
		}
		Invalidate();
	}

	/**
	 * Calculate the color selected by the pointer on the bar.
	 * 
	 * @param coord
	 *            Coordinate of the pointer.
	 */
	private void calculateColor(int coord) {
	    coord = coord - mBarPointerHaloRadius;
		if (coord > (mBarLength / 2) && (coord < mBarLength)) {
			mColor = Color
					.HSVToColor(new float[] {
							mHSVColor[0], 1f, 1 - (mPosToSVFactor * (coord - (mBarLength / 2)))
                    });
		} else if (coord > 0 && coord < mBarLength) {
			mColor = Color.HSVToColor(new float[]{
                    mHSVColor[0], (mPosToSVFactor * coord), 1f
            });
		} else if(coord == (mBarLength / 2)){
            mColor = Color.HSVToColor(new float[]{
                    mHSVColor[0], 1f, 1f
            });
        } else if (coord <= 0) {
				mColor = Color.White;
		} else if (coord >= mBarLength) {
				mColor = Color.Black;
		}
	}

	/**
	 * Get the currently selected color.
	 * 
	 * @return The ARGB value of the currently selected color.
	 */
	public int getColor() {
		return mColor;
	}

	/**
	 * Adds a {@code ColorPicker} instance to the bar. <br>
	 * <br>
	 * WARNING: Don't change the color picker. it is done already when the bar
	 * is added to the ColorPicker
	 * 
	 * @see ColorPicker#addSVBar(com.larswerkman.holocolorpicker.SVBar)
	 * @param picker
	 */
	public void setColorPicker(ColorPicker picker) {
		mPicker = picker;
	}

	
		protected override IParcelable OnSaveInstanceState() {
		IParcelable superState = base.OnSaveInstanceState();

		Bundle state = new Bundle();
		state.PutParcelable(STATE_PARENT, superState);
		state.PutFloatArray(STATE_COLOR, mHSVColor);
		float[] hsvColor = new float[3];
			Color.ColorToHSV(new Android.Graphics.Color(mColor), hsvColor);
		if (hsvColor[1] < hsvColor[2]) {
			state.PutFloat(STATE_SATURATION, hsvColor[1]);
		} else {
			state.PutFloat(STATE_VALUE, hsvColor[2]);
		}

		return state;
	}

	
		protected override void OnRestoreInstanceState(IParcelable state) {
		Bundle savedState = (Bundle) state;

			IParcelable superState = (IParcelable)savedState.GetParcelable(STATE_PARENT);
		base.OnRestoreInstanceState(superState);

		setColor(Color.HSVToColor(savedState.GetFloatArray(STATE_COLOR)));
		if (savedState.ContainsKey(STATE_SATURATION)) {
			setSaturation(savedState.GetFloat(STATE_SATURATION));
		} else {
			setValue(savedState.GetFloat(STATE_VALUE));
		}
	}
}
}