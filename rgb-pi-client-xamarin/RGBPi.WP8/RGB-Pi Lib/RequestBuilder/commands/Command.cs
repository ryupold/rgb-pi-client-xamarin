package org.rgbpi.requestbuilder.commands;

import java.util.ArrayList;

import org.rgbpi.requestbuilder.attributes.Attribute;

public interface Command {
	public String toJSON();
	public boolean containsCommands();
	public boolean hasColorAttribute();
	
	
	//Getter and setter
	public String getCommandName();
	public ArrayList<Attribute> getAttributes();
}
