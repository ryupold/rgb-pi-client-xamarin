package org.rgbpi.requestbuilder;

import org.rgbpi.requestbuilder.attributes.CommandsNode;
import org.rgbpi.requestbuilder.commands.Command;


public class RequestBuilder {
	private CommandsNode commands;
	
	public RequestBuilder() {
		this.commands = new CommandsNode();
	}
	
	public CommandsNode getCommandsNode() {
		return this.commands;
	}
	
	public String toJSON() {
		String JSON = "{";
		JSON = JSON+this.commands.toJSON();
		JSON = JSON+"}";
		return JSON;
	}
	
	public void add(Command cmd) {
		this.getCommandsNode().addSingleCommandNode(cmd);
	}
	
}
