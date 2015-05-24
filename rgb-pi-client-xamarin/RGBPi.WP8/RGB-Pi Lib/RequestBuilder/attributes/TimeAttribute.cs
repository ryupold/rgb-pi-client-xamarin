package org.rgbpi.requestbuilder.attributes;


public class TimeAttribute implements Attribute {

	String time;
	
	public TimeAttribute(String time) {
		this.time = time;
	}
	
	@Override
	public String toJSON() {
		return this.time;
	}

	@Override
	public boolean containsCommands() {
		return false;
	}
	
	public String toString() {
		return time;
	}

	@Override
	public boolean isColorAttribute() {
		// TODO Auto-generated method stub
		return false;
	}

	@Override
	public String getType() {
		return "time";
	}

}
