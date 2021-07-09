using U.DalilaDB;

public class MainDdbVolatileElements : DalilaDBVolatileElements<MainDdbVolatileElements>
{
    protected override int cacheSize_ => 100;
}