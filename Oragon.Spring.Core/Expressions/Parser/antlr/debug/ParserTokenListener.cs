namespace Oragon.Spring.Expressions.Parser.antlr.debug
{
    public interface ParserTokenListener : Listener
	{
		void  parserConsume	(object source, TokenEventArgs e);
		void  parserLA		(object source, TokenEventArgs e);
	}
}