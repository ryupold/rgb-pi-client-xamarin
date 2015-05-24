package org.rgbpi.requestbuilder.commands;

import java.util.ArrayList;

import org.rgbpi.requestbuilder.attributes.Attribute;
import org.rgbpi.requestbuilder.attributes.ColorAttribute;
import org.rgbpi.requestbuilder.attributes.OperatorAttribute;

public class CCCommand implements Command {
	
	public final static String COMMAND_NAME = "CHANGE COLOR";
	
	private ColorAttribute c;
	private OperatorAttribute o;
	private ArrayList<Attribute> attributes;
	
	
	
	public CCCommand(ColorAttribute c) {
		this.attributes = new ArrayList<Attribute>();
		this.c = c;
		this.attributes.add(this.c);
	}
	
	
	
	public CCCommand(ColorAttribute c, OperatorAttribute o) {
		this.attributes = new ArrayList<Attribute>();
		this.c = c;
		this.o = o;
		this.attributes.add(this.c);
		this.attributes.add(this.o);
	}
	
	
	
	@Override
	public String toJSON() {
		String JSON = "{ "
				+ "\"type\":\"cc\","
				+ "\"color\":\""+this.c.toJSON()+"\"";
		
		if (this.o != null) {
			JSON = JSON + this.o.toJSON();
		}
		
		JSON = JSON	+ "}";
		return JSON;
	}
	
	
	
	public void setColorAttribute(ColorAttribute c) {
		this.attributes.remove(this.c);
		this.c = c;
		this.attributes.add(this.c);
	}
	
	
	
	public ArrayList<Attribute> getAttributes() {
		return this.attributes;
	}



	@Override
	public boolean containsCommands() {
		// TODO Auto-generated method stub
		return false;
	}



	@Override
	public String getCommandName() {
		return COMMAND_NAME;
	}



	@Override
	public boolean hasColorAttribute() {
		// TODO Auto-generated method stub
		return true;
	}
}
