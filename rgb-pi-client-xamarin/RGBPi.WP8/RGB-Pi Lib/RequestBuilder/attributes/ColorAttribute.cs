package org.rgbpi.requestbuilder.attributes;

import android.graphics.Color;

public class ColorAttribute implements Attribute {
	
	//TODO: Allow random colors, ranges
	
	int c;
	float minR, maxR, minG, maxG, minB, maxB;
	
	//Random color
	public ColorAttribute(int minBrightness, int maxBrightness) {
		this.c = -1;
		this.minR = minBrightness/255;
		this.maxR = maxBrightness / 255;
		
		this.minG = this.minR;
		this.minB = this.minR;
		this.maxG = this.maxR;
		this.maxB = this.maxR;
	}
	
	//Random color, more specified
	public ColorAttribute(int minR, int maxR, int minG, int maxG, int minB, int maxB) {
		this.c = -1;
		this.minR = minR / 255;
		this.maxR = maxR / 255;
		this.minG = minG / 255;
		this.maxG = maxG / 255;
		this.minB = minB / 255;
		this.maxB = maxB / 255;
	}
	
	//Specific color
	public ColorAttribute(int c) {
		this.c = c;
	}

	@Override
	public String toJSON() {
		String json;
		//If random color
		if (this.c == -1) {
			json = "{r:"+minR+"-"+maxR+","+minG+"-"+maxG+","+minB+"-"+maxB+"}";
		}
		else {
			json = "{b:"+Color.red(c)+","+Color.green(c)+","+Color.blue(c)+"}";
		}
		return json;
	}
	
	public int getColor() {
		return c;
	}

	@Override
	public boolean containsCommands() {
		return false;
	}
	
	public String toString() {
		return ""+Color.red(c)+", "+Color.green(c)+", "+Color.blue(c);
	}

	@Override
	public boolean isColorAttribute() {
		// TODO Auto-generated method stub
		return true;
	}

	@Override
	public String getType() {
		return "color";
	}

}
