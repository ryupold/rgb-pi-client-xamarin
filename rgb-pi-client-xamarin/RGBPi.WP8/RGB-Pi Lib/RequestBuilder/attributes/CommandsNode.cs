package org.rgbpi.requestbuilder.attributes;

import java.util.ArrayList;
import java.util.Iterator;

import org.rgbpi.requestbuilder.commands.Command;

/**
 * A special kind of attribute. It contains commands (which contain attributes themselves)
 *
 */
public class CommandsNode implements Attribute {
	private ArrayList<Command> commands;

	
	
	public CommandsNode() {
		this.commands = new ArrayList<Command>();
	}
	
	
	
	public void addSingleCommandNode(Command node) {
		this.commands.add(node);
	}
	
	
	
	public ArrayList<Command> getCommands() {
		return this.commands;
	}
	
	
	
	public String toJSON() {
		String JSON = "";
		if (this.hasNodes()) {
			Iterator<Command> i = this.commands.iterator();
			JSON = "\"commands\": "
					+ "[ ";
			
			while (i.hasNext()) {
				JSON = JSON + i.next().toJSON();
				if (i.hasNext()) {
					JSON = JSON+",";
				}
			}
			
			JSON = JSON+"]";
		}
		else {
			JSON = "Invalid commandtree. commandsNode is empty (try adding commands via addSingleCommandNode())";
		}
		return JSON;
	}
	
	
	
	public boolean hasNodes() {
		return !this.commands.isEmpty();
	}



	@Override
	public boolean containsCommands() {
		return true;
	}



	@Override
	public boolean isColorAttribute() {
		// TODO Auto-generated method stub
		return false;
	}



	@Override
	public String getType() {
		return "commands";
	}
}
