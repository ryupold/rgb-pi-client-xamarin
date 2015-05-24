package org.rgbpi.requestbuilder.attributes;

public interface Attribute {
	public String toJSON();
	public boolean containsCommands();
	public boolean isColorAttribute();
	public String getType();
}
