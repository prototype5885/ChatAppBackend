using IdGen;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

public static class Snowflake
{
    private static readonly IdGenerator generator = new IdGenerator(0);

    public static long New()
    {
        return generator.CreateId();
    }
}

public class SnowflakeDb : ValueGenerator<long>
{
    public override bool GeneratesTemporaryValues => false;

    public override long Next(EntityEntry entry)
    {
        return Snowflake.New();
    }
}