grammar Grammar;

/*
 * Parser Rules
 */

 instructions : (controller | model)+;

controller : CONTROLLER name=CAPTION LBRACE (methodDefinition)+ RBRACE;

model : MODEL name=CAPTION LBRACE (propertyDefenition';')+ RBRACE;

methodDefinition : (attributes)? VERB name=CAPTION LPAREN (parameters)? RPAREN RETURN returnedType=CAPTION ';';

attributes : LSQUAREPAREN attribute (','attribute)* RSQUAREPAREN;
attribute : key=CAPTION EQUAL value=VALUE;

parameters : (bodyParameter | queryParameter | bodyParameter ';' queryParameter);
bodyParameter : FROMBODY ':' propertyDefenition;
queryParameter : FROMQUERY ':' propertyDefenition (',' propertyDefenition);

propertyDefenition: type=CAPTION name=CAPTION;

/*
 * Lexer Rules
 */

MODEL : 'model';

CONTROLLER : 'cntrl';

RETURN : 'return';

FROMBODY : 'fromBody';
FROMQUERY : 'fromQuery';

VERB : 'get' | 'post' | 'put' | 'delete';

LBRACE : '{';
RBRACE : '}';

LPAREN : '(';
RPAREN : ')';

LSQUAREPAREN : '[';
RSQUAREPAREN : ']';

EQUAL : '=';

CAPTION : [A-Za-z]+([A-Za-z] | [0-9])*;

VALUE : (CAPTION | '-' | '_')+;

WS : [ \t\r\n] -> channel(HIDDEN);