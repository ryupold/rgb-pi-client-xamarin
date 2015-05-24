package org.rgbpi.requestbuilder.attributes;


public class ConditionAttribute implements Attribute {
	
	String condition;
	boolean cond;
	Integer i;
	int type;	//0 boolean (while true); 1 int (i times);
	
	//Boolean
	public ConditionAttribute(boolean cond) {
		this.condition = "{b:"+((cond) ? 1 : 0)+"}";
		this.type = 0;
		this.cond = cond;
	}
	//i times
	public ConditionAttribute(int i) {
		this.condition = "{i:"+(i-1)+"}";
		this.type = 1;
		this.i = i;
	}
	
	
	
	@Override
	public String toJSON() {
		return this.condition;
	}

	@Override
	public boolean containsCommands() {
		return false;
	}
	
	public String toString() {
		String toString;
		if (i != null) {
			toString = i+"x";
		}
		else {
			toString = (cond) ? "infinite" : "never";
		}
		return toString;
	}

	@Override
	public boolean isColorAttribute() {
		// TODO Auto-generated method stub
		return false;
	}

	@Override
	public String getType() {
		return "condition";
	}

}
