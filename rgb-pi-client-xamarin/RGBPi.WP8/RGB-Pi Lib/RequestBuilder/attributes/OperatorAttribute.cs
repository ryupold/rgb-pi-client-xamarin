package org.rgbpi.requestbuilder.attributes;


public class OperatorAttribute implements Attribute {
	
	private String operator;
	
	public OperatorAttribute(String o) {
		switch (o) {
		case "*":
			this.operator = "*";
			break;
		case "/":
			this.operator = "/";
			break;
		case "+":
			this.operator = "+";
			break;
		case "-":
			this.operator = "-";
			break;
		default:
			throw new IllegalArgumentException("Illegal argument "+o+". Argument must be one of * / + -");
		}
	}
	
	@Override
	public String toJSON() {
		return ",\"operator\":\""+this.operator+"\"";
	}

	@Override
	public boolean containsCommands() {
		return false;
	}
	
	public String toString() {
		return operator;
	}

	@Override
	public boolean isColorAttribute() {
		// TODO Auto-generated method stub
		return false;
	}

	@Override
	public String getType() {
		return "operator";
	}

}
