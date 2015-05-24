package org.rgbpi.requestbuilder.commands;

import java.util.ArrayList;

import org.rgbpi.requestbuilder.attributes.Attribute;
import org.rgbpi.requestbuilder.attributes.CommandsNode;
import org.rgbpi.requestbuilder.attributes.ConditionAttribute;

public class LoopCommand implements Command {
	
	public static final String COMMAND_NAME = "LOOP";

	private ConditionAttribute condition;
	private CommandsNode commands;
	private ArrayList<Attribute> attributes;
	
	public void add(Command cmd) {
		this.getCommandsNode().addSingleCommandNode(cmd);
	}

	
	
	//Default ctor. Condition: while true
	public LoopCommand() {
		this.attributes = new ArrayList<Attribute>();
		this.commands = new CommandsNode();
		this.condition = new ConditionAttribute(true);
		this.attributes.add(this.condition);
	}
	
	
	
	//Custom ctor. Condition given per parameter
	public LoopCommand(ConditionAttribute c) {
		this.attributes = new ArrayList<Attribute>();
		this.commands = new CommandsNode();
		this.condition = c;
		this.attributes.add(this.condition);
	}

	
	
	@Override
	public String toJSON() {
		String JSON = "{"
				+ "\"type\":\"loop\","
				+ "\"condition\":\""+this.condition.toJSON()+"\","
				+ this.commands.toJSON()
				+ "}";
		
		return JSON;
	}



	public CommandsNode getCommandsNode() {
		return commands;
	}



	public void setCondition(ConditionAttribute condition) {
		this.attributes.remove(this.condition);
		this.condition = condition;
		this.attributes.add(this.condition);
	}

	
	
	@Override
	public ArrayList<Attribute> getAttributes() {
		return this.attributes;
	}



	@Override
	public boolean containsCommands() {
		return true;
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
