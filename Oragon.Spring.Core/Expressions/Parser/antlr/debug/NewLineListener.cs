namespace Oragon.Spring.Expressions.Parser.antlr.debug
{
    public interface NewLineListener : Listener
	{
		void hitNewLine(object source, NewLineEventArgs e);
	}
}