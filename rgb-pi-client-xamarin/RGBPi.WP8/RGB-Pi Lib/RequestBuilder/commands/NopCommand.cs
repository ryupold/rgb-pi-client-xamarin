package org.rgbpi.requestbuilder.commands;

import java.util.ArrayList;

import org.rgbpi.requestbuilder.attributes.Attribute;


public class NopCommand implements Command {
	
	private final String COMMAND_NAME = "do nothing";
	
	@Override
	public String toJSON() {
		String JSON = "{ "
				+ "\"type\":\"nop\""
				+ "}";
		return JSON;
	}

	@Override
	public ArrayList<Attribute> getAttributes() {
		return null;
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
