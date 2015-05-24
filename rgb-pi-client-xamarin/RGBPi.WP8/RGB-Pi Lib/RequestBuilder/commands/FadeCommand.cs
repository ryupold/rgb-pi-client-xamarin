package org.rgbpi.requestbuilder.commands;

import java.util.ArrayList;

import org.rgbpi.requestbuilder.attributes.Attribute;
import org.rgbpi.requestbuilder.attributes.ColorAttribute;
import org.rgbpi.requestbuilder.attributes.TimeAttribute;

public class FadeCommand implements Command {
	
	public final static String COMMAND_NAME = "FADE";
	
	TimeAttribute time;
	ColorAttribute start;	//Optional
	ColorAttribute end;
	private ArrayList<Attribute> attributes;
	
	
	
	public FadeCommand(TimeAttribute t, ColorAttribute e) {
		this.attributes = new ArrayList<Attribute>();
		this.time = t;
		this.end = e;
		this.attributes.add(this.time);
		this.attributes.add(this.end);
	}
	
	
	
	public FadeCommand(TimeAttribute t, ColorAttribute s, ColorAttribute e) {
		this.attributes = new ArrayList<Attribute>();
		this.time = t;
		this.end = e;
		this.start = s;
		this.attributes.add(this.time);
		this.attributes.add(this.end);
		this.attributes.add(this.start);
	}

	
	
	@Override
	public String toJSON() {
		String JSON = "{ "
				+ "\"type\":\"fade\","
				+ "\"time\":\""+this.time.toJSON()+"\","
				+ "\"end\":\""+this.end.toJSON()+"\"";
		
		if (this.start != null) {
			JSON = JSON + ",\"start\":\""+this.end.toJSON()+"\"";
		}
		
		JSON = JSON + "}";
		return JSON;
	}
	
	
	
	public void setTimeAttribute(TimeAttribute t) {
		this.attributes.remove(this.time);
		this.time = t;
		this.attributes.add(this.time);
	}

	
	
	public void setStartAttribute(ColorAttribute s) {
		this.attributes.remove(this.start);
		this.start = s;
		this.attributes.add(this.start);
	}
	
	
	
	public void setEndAttribute(ColorAttribute e) {
		this.attributes.remove(this.end);
		this.end = e;
		this.attributes.add(this.end);
	}
	
	
	
	@Override
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
		return false;
	}

}
