using IdGen;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace ChatAppBackend;

public static class Snowflake
{
    private static readonly IdGenerator Generator = new(0);

    public static long New()
    {
        return Generator.CreateId();
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