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
using Android.Views;
using Android.Graphics;
using Android.Content;
using Android.Util;
using System;
using Android.OS;
using RGBPi.Android;
using Android.Content.Res;


namespace Com.Larswerkman.HolorColorPicker{

/**
 * Displays a holo-themed color picker.
 * 
 * <p>
 * Use {@link #getColor()} to retrieve the selected color. <br>
 * Use {@link #addSVBar(SVBar)} to add a Saturation/Value Bar. <br>
 * Use {@link #addOpacityBar(OpacityBar)} to add a Opacity Bar.
 * </p>
 */
public class ColorPicker : View {
	/*
	 * Constants used to save/restore the instance state.
	 */
		private static readonly string STATE_PARENT = "parent";
		private static readonly string STATE_ANGLE = "angle";
		private static readonly string STATE_OLD_COLOR = "color";
		private static readonly string STATE_SHOW_OLD_COLOR = "showColor";

	/**
	 * Colors to construct the color wheel using {@link android.graphics.SweepGradient}.
	 */
		private static readonly int[] COLORS = new int[] { unchecked((int)0xFFFF0000), unchecked((int)0xFFFF00FF),
			unchecked((int)0xFF0000FF), unchecked((int)0xFF00FFFF), unchecked((int)0xFF00FF00), unchecked((int)0xFFFFFF00), unchecked((int)0xFFFF0000)};

	/**
	 * {@code Paint} instance used to draw the color wheel.
	 */
	private Paint mColorWheelPaint;

	/**
	 * {@code Paint} instance used to draw the pointer's "halo".
	 */
	private Paint mPointerHaloPaint;

	/**
	 * {@code Paint} instance used to draw the pointer (the selected color).
	 */
	private Paint mPointerColor;

	/**
	 * The width of the color wheel thickness.
	 */
	private int mColorWheelThickness;

	/**
	 * The radius of the color wheel.
	 */
	private int mColorWheelRadius;
	private int mPreferredColorWheelRadius;

	/**
	 * The radius of the center circle inside the color wheel.
	 */
	private int mColorCenterRadius;
	private int mPreferredColorCenterRadius;

	/**
	 * The radius of the halo of the center circle inside the color wheel.
	 */
	private int mColorCenterHaloRadius;
	private int mPreferredColorCenterHaloRadius;

	/**
	 * The radius of the pointer.
	 */
	private int mColorPointerRadius;

	/**
	 * The radius of the halo of the pointer.
	 */
	private int mColorPointerHaloRadius;

	/**
	 * The rectangle enclosing the color wheel.
	 */
	private RectF mColorWheelRectangle = new RectF();

	/**
	 * The rectangle enclosing the center inside the color wheel.
	 */
	private RectF mCenterRectangle = new RectF();

	/**
	 * {@code true} if the user clicked on the pointer to start the move mode. <br>
	 * {@code false} once the user stops touching the screen.
	 * 
	 * @see #onTouchEvent(android.view.MotionEvent)
	 */
	private bool mUserIsMovingPointer = false;

	/**
	 * The ARGB value of the currently selected color.
	 */
	private int mColor;

	/**
	 * The ARGB value of the center with the old selected color.
	 */
	private int mCenterOldColor;
	
	/**
	 * Whether to show the old color in the center or not.
	 */
	private bool mShowCenterOldColor;

	/**
	 * The ARGB value of the center with the new selected color.
	 */
	private int mCenterNewColor;

	/**
	 * Number of pixels the origin of this view is moved in X- and Y-direction.
	 * 
	 * <p>
	 * We use the center of this (quadratic) View as origin of our internal
	 * coordinate system. Android uses the upper left corner as origin for the
	 * View-specific coordinate system. So this is the value we use to translate
	 * from one coordinate system to the other.
	 * </p>
	 * 
	 * <p>
	 * Note: (Re)calculated in {@link #onMeasure(int, int)}.
	 * </p>
	 * 
	 * @see #onDraw(android.graphics.Canvas)
	 */
	private float mTranslationOffset;
	
	/**
	 * Distance between pointer and user touch in X-direction.
	 */
	private float mSlopX;
    
	/**
	 * Distance between pointer and user touch in Y-direction.
	 */
    	private float mSlopY;

	/**
	 * The pointer's position expressed as angle (in rad).
	 */
	private float mAngle;

	/**
	 * {@code Paint} instance used to draw the center with the old selected
	 * color.
	 */
	private Paint mCenterOldPaint;

	/**
	 * {@code Paint} instance used to draw the center with the new selected
	 * color.
	 */
	private Paint mCenterNewPaint;

	/**
	 * {@code Paint} instance used to draw the halo of the center selected
	 * colors.
	 */
	private Paint mCenterHaloPaint;

	/**
	 * An array of floats that can be build into a {@code Color} <br>
	 * Where we can extract the Saturation and Value from.
	 */
	private float[] mHSV = new float[3];

	/**
	 * {@code SVBar} instance used to control the Saturation/Value bar.
	 */
	private SVBar mSVbar = null;

	/**
	 * {@code OpacityBar} instance used to control the Opacity bar.
	 */
	private OpacityBar mOpacityBar = null;

	/**
	 * {@code SaturationBar} instance used to control the Saturation bar.
	 */
	private SaturationBar mSaturationBar = null;

        /**
         * {@code TouchAnywhereOnColorWheelEnabled} instance used to control <br>
         * if the color wheel accepts input anywhere on the wheel or just <br>
         * on the halo.
         */
        private bool mTouchAnywhereOnColorWheelEnabled = true;

	/**
	 * {@code ValueBar} instance used to control the Value bar.
	 */
	private ValueBar mValueBar = null;

	/**
	 * {@code onColorChangedListener} instance of the onColorChangedListener
	 */
	private OnColorChangedListener onColorChangedListener;

	/**
	 * {@code onColorSelectedListener} instance of the onColorSelectedListener
	 */
	private OnColorSelectedListener onColorSelectedListener;

		public ColorPicker(Context context):base(context) {
		init(null, 0);
	}

		public ColorPicker(Context context, IAttributeSet attrs):base(context, attrs) {
		init(attrs, 0);
	}

		public ColorPicker(Context context, IAttributeSet attrs, int defStyle):base(context, attrs, defStyle) {
		init(attrs, defStyle);
	}

	/**
	 * An interface that is called whenever the color is changed. Currently it
	 * is always called when the color is changes.
	 * 
	 * @author lars
	 * 
	 */
	public interface OnColorChangedListener {
		void onColorChanged(int color);
	}

	/**
	 * An interface that is called whenever a new color has been selected.
	 * Currently it is always called when the color wheel has been released.
	 * 
	 */
	public interface OnColorSelectedListener {
		void onColorSelected(int color);
	}

	/**
	 * Set a onColorChangedListener
	 * 
	 * @param {@code OnColorChangedListener}
	 */
	public void setOnColorChangedListener(OnColorChangedListener listener) {
		this.onColorChangedListener = listener;
	}

	/**
	 * Gets the onColorChangedListener
	 * 
	 * @return {@code OnColorChangedListener}
	 */
	public OnColorChangedListener getOnColorChangedListener() {
		return this.onColorChangedListener;
	}

	/**
	 * Set a onColorSelectedListener
	 * 
	 * @param {@code OnColorSelectedListener}
	 */
	public void setOnColorSelectedListener(OnColorSelectedListener listener) {
		this.onColorSelectedListener = listener;
	}

	/**
	 * Gets the onColorSelectedListener
	 * 
	 * @return {@code OnColorSelectedListener}
	 */
	public OnColorSelectedListener getOnColorSelectedListener() {
		return this.onColorSelectedListener;
	}
	
	/**
	 * Color of the latest entry of the onColorChangedListener.
	 */
	private int oldChangedListenerColor;
	
	/**
	 * Color of the latest entry of the onColorSelectedListener.
	 */
	private int oldSelectedListenerColor;

	private void init(IAttributeSet attrs, int defStyle) {
			TypedArray a = Context.ObtainStyledAttributes(attrs,
				Resource.Styleable.ColorPicker, defStyle, 0);
			Resources b = Context.Resources;

		mColorWheelThickness = a.GetDimensionPixelSize(
				Resource.Styleable.ColorPicker_color_wheel_thickness,
				b.GetDimensionPixelSize(Resource.Dimension.color_wheel_thickness));
		mColorWheelRadius = a.GetDimensionPixelSize(
				Resource.Styleable.ColorPicker_color_wheel_radius,
				b.GetDimensionPixelSize(Resource.Dimension.color_wheel_radius));
		mPreferredColorWheelRadius = mColorWheelRadius;
		mColorCenterRadius = a.GetDimensionPixelSize(
				Resource.Styleable.ColorPicker_color_center_radius,
				b.GetDimensionPixelSize(Resource.Dimension.color_center_radius));
		mPreferredColorCenterRadius = mColorCenterRadius;
		mColorCenterHaloRadius = a.GetDimensionPixelSize(
				Resource.Styleable.ColorPicker_color_center_halo_radius,
				b.GetDimensionPixelSize(Resource.Dimension.color_center_halo_radius));
		mPreferredColorCenterHaloRadius = mColorCenterHaloRadius;
		mColorPointerRadius = a.GetDimensionPixelSize(
				Resource.Styleable.ColorPicker_color_pointer_radius,
				b.GetDimensionPixelSize(Resource.Dimension.color_pointer_radius));
		mColorPointerHaloRadius = a.GetDimensionPixelSize(
				Resource.Styleable.ColorPicker_color_pointer_halo_radius,
				b.GetDimensionPixelSize(Resource.Dimension.color_pointer_halo_radius));

			mShowCenterOldColor = a.GetBoolean(Resource.Styleable.ColorPicker_color_center_show_old_color, false);

		a.Recycle();

		mAngle = (float) (-Math.PI / 2);

		Shader s = new SweepGradient(0, 0, COLORS, null);

		mColorWheelPaint = new Paint(PaintFlags.AntiAlias);
		mColorWheelPaint.SetShader(s);
			mColorWheelPaint.SetStyle(Paint.Style.Stroke);
		mColorWheelPaint.StrokeWidth = (mColorWheelThickness);

			mPointerHaloPaint = new Paint(PaintFlags.AntiAlias);
			mPointerHaloPaint.Color = (Color.Black);
		mPointerHaloPaint.Alpha = (0x50);

			mPointerColor = new Paint(PaintFlags.AntiAlias);
			mPointerColor.Color = new Color(calculateColor(mAngle));

			mCenterNewPaint = new Paint(PaintFlags.AntiAlias);
			mCenterNewPaint.Color = new Color(calculateColor(mAngle));
			mCenterNewPaint.SetStyle(Paint.Style.Fill);

			mCenterOldPaint = new Paint(PaintFlags.AntiAlias);
			mCenterOldPaint.Color = new Color(calculateColor(mAngle));
			mCenterOldPaint.SetStyle(Paint.Style.Fill);

			mCenterHaloPaint = new Paint(PaintFlags.AntiAlias);
			mCenterHaloPaint.Color = (Color.Black);
		mCenterHaloPaint.Alpha = (0x00);
		
		mCenterNewColor = calculateColor(mAngle);
		mCenterOldColor = calculateColor(mAngle);
//		mShowCenterOldColor = true;
	}

	
		protected override void OnDraw(Canvas canvas) {
		// All of our positions are using our internal coordinate system.
		// Instead of translating
		// them we let Canvas do the work for us.
		canvas.Translate(mTranslationOffset, mTranslationOffset);

		// Draw the color wheel.
		canvas.DrawOval(mColorWheelRectangle, mColorWheelPaint);

		float[] pointerPosition = calculatePointerPosition(mAngle);

		// Draw the pointer's "halo"
		canvas.DrawCircle(pointerPosition[0], pointerPosition[1],
				mColorPointerHaloRadius, mPointerHaloPaint);

		// Draw the pointer (the currently selected color) slightly smaller on
		// top.
		canvas.DrawCircle(pointerPosition[0], pointerPosition[1],
				mColorPointerRadius, mPointerColor);

		// Draw the halo of the center colors.
		canvas.DrawCircle(0, 0, mColorCenterHaloRadius, mCenterHaloPaint);
		
		if (mShowCenterOldColor) {
			// Draw the old selected color in the center.
			canvas.DrawArc(mCenterRectangle, 90, 180, true, mCenterOldPaint);

			// Draw the new selected color in the center.
			canvas.DrawArc(mCenterRectangle, 270, 180, true, mCenterNewPaint);
		}
		else {
			// Draw the new selected color in the center.
			canvas.DrawArc(mCenterRectangle, 0, 360, true, mCenterNewPaint);
		}
	}

	
		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec) {
			int intrinsicSize = 2 * (mPreferredColorWheelRadius + mColorPointerHaloRadius);

			int widthMode = (int)MeasureSpec.GetMode(widthMeasureSpec);
			int widthSize = (int)MeasureSpec.GetSize(widthMeasureSpec);
			int heightMode = (int)MeasureSpec.GetMode(heightMeasureSpec);
			int heightSize = (int)MeasureSpec.GetSize(heightMeasureSpec);

		int width;
		int height;

			if (widthMode == (int)MeasureSpecMode.Exactly) {
			width = widthSize;
			} else if (widthMode == (int)MeasureSpecMode.AtMost) {
			width = Math.Min(intrinsicSize, widthSize);
		} else {
			width = intrinsicSize;
		}

			if (heightMode == (int)MeasureSpecMode.Exactly) {
			height = heightSize;
			} else if (heightMode == (int)MeasureSpecMode.AtMost) {
			height = Math.Min(intrinsicSize, heightSize);
		} else {
			height = intrinsicSize;
		}

		int min = Math.Min(width, height);
		SetMeasuredDimension(min, min);
		mTranslationOffset = min * 0.5f;

		// fill the rectangle instances.
		mColorWheelRadius = min / 2 - mColorWheelThickness - mColorPointerHaloRadius;
		mColorWheelRectangle.Set(-mColorWheelRadius, -mColorWheelRadius,
				mColorWheelRadius, mColorWheelRadius);

		mColorCenterRadius = (int) ((float) mPreferredColorCenterRadius * ((float) mColorWheelRadius / (float) mPreferredColorWheelRadius));
		mColorCenterHaloRadius = (int) ((float) mPreferredColorCenterHaloRadius * ((float) mColorWheelRadius / (float) mPreferredColorWheelRadius));
		mCenterRectangle.Set(-mColorCenterRadius, -mColorCenterRadius,
				mColorCenterRadius, mColorCenterRadius);
	}

	private int ave(int s, int d, float p) {
			return (int)(s + Math.Round(p * (d - s)));
	}

	/**
	 * Calculate the color using the supplied angle.
	 * 
	 * @param angle
	 *            The selected color's position expressed as angle (in rad).
	 * 
	 * @return The ARGB value of the color on the color wheel at the specified
	 *         angle.
	 */
	private int calculateColor(float angle) {
		float unit = (float) (angle / (2 * Math.PI));
		if (unit < 0) {
			unit += 1;
		}

		if (unit <= 0) {
			mColor = COLORS[0];
			return COLORS[0];
		}
		if (unit >= 1) {
			mColor = COLORS[COLORS.Length - 1];
			return COLORS[COLORS.Length - 1];
		}

		float p = unit * (COLORS.Length - 1);
		int i = (int) p;
		p -= i;

		int c0 = COLORS[i];
		int c1 = COLORS[i + 1];
			int a = ave(Color.GetAlphaComponent(c0), Color.GetAlphaComponent(c1), p);
			int r = ave(Color.GetRedComponent(c0), Color.GetRedComponent(c1), p);
		int g = ave(Color.GetGreenComponent(c0), Color.GetGreenComponent(c1), p);
		int b = ave(Color.GetBlueComponent(c0), Color.GetBlueComponent(c1), p);

		mColor = Color.Argb(a, r, g, b);
			return Color.Argb(a, r, g, b);
	}

	/**
	 * Get the currently selected color.
	 * 
	 * @return The ARGB value of the currently selected color.
	 */
	public int getColor() {
		return mCenterNewColor;
	}

	/**
	 * Set the color to be highlighted by the pointer. </br> </br> If the
	 * instances {@code SVBar} and the {@code OpacityBar} aren't null the color
	 * will also be set to them
	 * 
	 * @param color
	 *            The RGB value of the color to highlight. If this is not a
	 *            color displayed on the color wheel a very simple algorithm is
	 *            used to map it to the color wheel. The resulting color often
	 *            won't look close to the original color. This is especially
	 *            true for shades of grey. You have been warned!
	 */
	public void setColor(int color) {
		mAngle = colorToAngle(color);
			mPointerColor.Color = new Android.Graphics.Color(calculateColor(mAngle));

		// check of the instance isn't null
		if (mOpacityBar != null) {
			// set the value of the opacity
			mOpacityBar.setColor(mColor);
				mOpacityBar.setOpacity(Color.GetAlphaComponent(color));
		}

		// check if the instance isn't null
		if (mSVbar != null) {
			// the array mHSV will be filled with the HSV values of the color.
				Color.ColorToHSV(new Android.Graphics.Color(color), mHSV);
			mSVbar.setColor(mColor);

			// because of the design of the Saturation/Value bar,
			// we can only use Saturation or Value every time.
			// Here will be checked which we shall use.
			if (mHSV[1] < mHSV[2]) {
				mSVbar.setSaturation(mHSV[1]);
			} else if(mHSV[1] > mHSV[2]){
				mSVbar.setValue(mHSV[2]);
			}
		}

		if (mSaturationBar != null) {
				Color.ColorToHSV(new Android.Graphics.Color(color), mHSV);
			mSaturationBar.setColor(mColor);
			mSaturationBar.setSaturation(mHSV[1]);
		}

		if (mValueBar != null && mSaturationBar == null) {
						Color.ColorToHSV(new Android.Graphics.Color(color), mHSV);
			mValueBar.setColor(mColor);
			mValueBar.setValue(mHSV[2]);
		} else if (mValueBar != null) {
								Color.ColorToHSV(new Android.Graphics.Color(color), mHSV);
			mValueBar.setValue(mHSV[2]);
		}
        setNewCenterColor(color);
	}

	/**
	 * Convert a color to an angle.
	 * 
	 * @param color
	 *            The RGB value of the color to "find" on the color wheel.
	 * 
	 * @return The angle (in rad) the "normalized" color is displayed on the
	 *         color wheel.
	 */
	private float colorToAngle(int color) {
		float[] colors = new float[3];
			Color.ColorToHSV(new Android.Graphics.Color(color), colors);
		
			return (float) Math.PI * (-colors[0]) / 360f;
	}
	
	
	public override bool OnTouchEvent(MotionEvent evt) {
		Parent.RequestDisallowInterceptTouchEvent(true);

		// Convert coordinates to our internal coordinate system
			float x = evt.GetX() - mTranslationOffset;
			float y = evt.GetY() - mTranslationOffset;

			switch (evt.Action) {
			case MotionEventActions.Down:
			// Check whether the user pressed on the pointer.
			float[] pointerPosition = calculatePointerPosition(mAngle);
			if (x >= (pointerPosition[0] - mColorPointerHaloRadius)
					&& x <= (pointerPosition[0] + mColorPointerHaloRadius)
					&& y >= (pointerPosition[1] - mColorPointerHaloRadius)
					&& y <= (pointerPosition[1] + mColorPointerHaloRadius)) {
				mSlopX = x - pointerPosition[0];
				mSlopY = y - pointerPosition[1];
				mUserIsMovingPointer = true;
				Invalidate();
			}
			// Check whether the user pressed on the center.
			else if (x >= -mColorCenterRadius && x <= mColorCenterRadius
					&& y >= -mColorCenterRadius && y <= mColorCenterRadius
					&& mShowCenterOldColor) {
				mCenterHaloPaint.Alpha = (0x50);
				setColor(getOldCenterColor());
				Invalidate();
			}
                        // Check whether the user pressed anywhere on the wheel.
                        else if (Math.Sqrt(x*x + y*y)  <= mColorWheelRadius + mColorPointerHaloRadius
                                        && Math.Sqrt(x*x + y*y) >= mColorWheelRadius - mColorPointerHaloRadius
                                        && mTouchAnywhereOnColorWheelEnabled) {
                                mUserIsMovingPointer = true;
                                Invalidate();
                        }
			// If user did not press pointer or center, report event not handled
			else{
				Parent.RequestDisallowInterceptTouchEvent(false);
				return false;
			}
			break;
			case MotionEventActions.Move:
			if (mUserIsMovingPointer) {
				mAngle = (float) Math.Atan2(y - mSlopY, x - mSlopX);
					mPointerColor.Color = new Android.Graphics.Color(calculateColor(mAngle));

				setNewCenterColor(mCenterNewColor = calculateColor(mAngle));
				
				if (mOpacityBar != null) {
					mOpacityBar.setColor(mColor);
				}

				if (mValueBar != null) {
					mValueBar.setColor(mColor);
				}

				if (mSaturationBar != null) {
					mSaturationBar.setColor(mColor);
				}

				if (mSVbar != null) {
					mSVbar.setColor(mColor);
				}

				Invalidate();
			}
			// If user did not press pointer or center, report event not handled
			else{
					Parent.RequestDisallowInterceptTouchEvent(false);
				return false;
			}
			break;
		case MotionEventActions.Up:
			mUserIsMovingPointer = false;
			mCenterHaloPaint.Alpha = (0x00);
			
			if (onColorSelectedListener != null && mCenterNewColor != oldSelectedListenerColor) {
				onColorSelectedListener.onColorSelected(mCenterNewColor);
				oldSelectedListenerColor = mCenterNewColor;
			}

			Invalidate();
			break;
			case MotionEventActions.Cancel:
			if (onColorSelectedListener != null && mCenterNewColor != oldSelectedListenerColor) {
				onColorSelectedListener.onColorSelected(mCenterNewColor);
				oldSelectedListenerColor = mCenterNewColor;
			}
			break;
		}
		return true;
	}

	/**
	 * Calculate the pointer's coordinates on the color wheel using the supplied
	 * angle.
	 * 
	 * @param angle
	 *            The position of the pointer expressed as angle (in rad).
	 * 
	 * @return The coordinates of the pointer's center in our internal
	 *         coordinate system.
	 */
	private float[] calculatePointerPosition(float angle) {
		float x = (float) (mColorWheelRadius * Math.Cos(angle));
		float y = (float) (mColorWheelRadius * Math.Sin(angle));

		return new float[] { x, y };
	}

	/**
	 * Add a Saturation/Value bar to the color wheel.
	 * 
	 * @param bar
	 *            The instance of the Saturation/Value bar.
	 */
	public void addSVBar(SVBar bar) {
		mSVbar = bar;
		// Give an instance of the color picker to the Saturation/Value bar.
		mSVbar.setColorPicker(this);
		mSVbar.setColor(mColor);
	}

	/**
	 * Add a Opacity bar to the color wheel.
	 * 
	 * @param bar
	 *            The instance of the Opacity bar.
	 */
	public void addOpacityBar(OpacityBar bar) {
		mOpacityBar = bar;
		// Give an instance of the color picker to the Opacity bar.
		mOpacityBar.setColorPicker(this);
		mOpacityBar.setColor(mColor);
	}

	public void addSaturationBar(SaturationBar bar) {
		mSaturationBar = bar;
		mSaturationBar.setColorPicker(this);
		mSaturationBar.setColor(mColor);
	}

	public void addValueBar(ValueBar bar) {
		mValueBar = bar;
		mValueBar.setColorPicker(this);
		mValueBar.setColor(mColor);
	}

	/**
	 * Change the color of the center which indicates the new color.
	 * 
	 * @param color
	 *            int of the color.
	 */
	public void setNewCenterColor(int color) {
		mCenterNewColor = color;
			mCenterNewPaint.Color = new Android.Graphics.Color(color);
		if (mCenterOldColor == 0) {
			mCenterOldColor = color;
				mCenterOldPaint.Color = new Android.Graphics.Color(color);
		}
		if (onColorChangedListener != null && color != oldChangedListenerColor ) {
			onColorChangedListener.onColorChanged(color);
			oldChangedListenerColor  = color;
		}
		Invalidate();
	}

	/**
	 * Change the color of the center which indicates the old color.
	 * 
	 * @param color
	 *            int of the color.
	 */
	public void setOldCenterColor(int color) {
		mCenterOldColor = color;
			mCenterOldPaint.Color = new Android.Graphics.Color(color);
		Invalidate();
	}

	public int getOldCenterColor() {
		return mCenterOldColor;
	}
	
	/**
	 * Set whether the old color is to be shown in the center or not
	 * 
	 * @param show true if the old color is to be shown, false otherwise
	 */
	public void setShowOldCenterColor(bool show) {
		mShowCenterOldColor = show;
			Invalidate ();
	}
	
	public bool getShowOldCenterColor() {
		return mShowCenterOldColor;
	}

	/**
	 * Used to change the color of the {@code OpacityBar} used by the
	 * {@code SVBar} if there is an change in color.
	 * 
	 * @param color
	 *            int of the color used to change the opacity bar color.
	 */
	public void changeOpacityBarColor(int color) {
		if (mOpacityBar != null) {
			mOpacityBar.setColor(color);
		}
	}

	/**
	 * Used to change the color of the {@code SaturationBar}.
	 * 
	 * @param color
	 *            int of the color used to change the opacity bar color.
	 */
	public void changeSaturationBarColor(int color) {
		if (mSaturationBar != null) {
			mSaturationBar.setColor(color);
		}
	}

	/**
	 * Used to change the color of the {@code ValueBar}.
	 * 
	 * @param color
	 *            int of the color used to change the opacity bar color.
	 */
	public void changeValueBarColor(int color) {
		if (mValueBar != null) {
			mValueBar.setColor(color);
		}
	}
	
	/**
	 * Checks if there is an {@code OpacityBar} connected.
	 * 
	 * @return true or false.
	 */
	public bool hasOpacityBar(){
		return mOpacityBar != null;
	}
	
	/**
	 * Checks if there is a {@code ValueBar} connected.
	 * 
	 * @return true or false.
	 */
	public bool hasValueBar(){
		return mValueBar != null;
	}
	
	/**
	 * Checks if there is a {@code SaturationBar} connected.
	 * 
	 * @return true or false.
	 */
	public bool hasSaturationBar(){
		return mSaturationBar != null;
	}
	
	/**
	 * Checks if there is a {@code SVBar} connected.
	 * 
	 * @return true or false.
	 */
	public bool hasSVBar(){
		return mSVbar != null;
	}

	
		protected override IParcelable OnSaveInstanceState() {
		IParcelable superState = base.OnSaveInstanceState();

		Bundle state = new Bundle();
		state.PutParcelable(STATE_PARENT, superState);
		state.PutFloat(STATE_ANGLE, mAngle);
		state.PutInt(STATE_OLD_COLOR, mCenterOldColor);
		state.PutBoolean(STATE_SHOW_OLD_COLOR, mShowCenterOldColor);

		return state;
	}

	
		protected override void OnRestoreInstanceState(IParcelable state) {
		Bundle savedState = (Bundle) state;

			IParcelable superState = (IParcelable)savedState.GetParcelable(STATE_PARENT);
		base.OnRestoreInstanceState(superState);

		mAngle = savedState.GetFloat(STATE_ANGLE);
		setOldCenterColor(savedState.GetInt(STATE_OLD_COLOR));
		mShowCenterOldColor = savedState.GetBoolean(STATE_SHOW_OLD_COLOR);
		int currentColor = calculateColor(mAngle);
			mPointerColor.Color = new Android.Graphics.Color(currentColor);
		setNewCenterColor(currentColor);
	}

        public void setTouchAnywhereOnColorWheelEnabled(bool TouchAnywhereOnColorWheelEnabled){
                mTouchAnywhereOnColorWheelEnabled = TouchAnywhereOnColorWheelEnabled;
        }

        public bool getTouchAnywhereOnColorWheel(){
                return mTouchAnywhereOnColorWheelEnabled;
        }
}

}