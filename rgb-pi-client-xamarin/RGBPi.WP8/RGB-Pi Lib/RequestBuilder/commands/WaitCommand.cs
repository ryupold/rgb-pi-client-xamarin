package org.rgbpi.requestbuilder.commands;

import java.util.ArrayList;

import org.rgbpi.requestbuilder.attributes.Attribute;
import org.rgbpi.requestbuilder.attributes.TimeAttribute;

public class WaitCommand implements Command {
	
	public final static String COMMAND_NAME = "WAIT";

	private TimeAttribute time;
	private ArrayList<Attribute> attributes;
	
	
	
	public WaitCommand() {
		this.attributes = new ArrayList<Attribute>();
		this.time = new TimeAttribute("5");	 				//Default wait-time 5s
		this.attributes.add(this.time);
	}
	
	
	
	public WaitCommand(TimeAttribute t) {
		this.attributes = new ArrayList<Attribute>();
		this.time = t;
		this.attributes.add(this.time);		
	}
	
	
	
	public void setTimeAttribute(TimeAttribute t) {
		this.attributes.remove(this.time);
		this.time = t;
		this.attributes.add(this.time);
	}
	
	
	
	@Override
	public String toJSON() {
		String JSON = "{ "
				+ "\"type\":\"wait\","
				+ "\"time\":\""+this.time.toJSON()+"\""
				+ "}";
		return JSON;
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
