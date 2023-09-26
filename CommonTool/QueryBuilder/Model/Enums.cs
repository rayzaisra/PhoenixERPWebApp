using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTool.QueryBuilder.Model
{
    public enum EnumRuleType
    {
        STRING,
        INTEGER,
        DOUBLE,
        DATE,
        TIME,
        DATETIME,
        BOOLEAN
    }

    public enum EnumBuilderType
    {
        SQL,    // SqlBuilder
        MONGODB // MongodbBuilder
    }

    public enum EnumCondition
    {
        AND,
        OR
    }
    public enum EnumDBType
    {
        DB2,
        DERBY,
        H2,
        HSQL,
        INFORMIX,
        MS_SQL,
        MYSQL,
        ORACLE,
        POSTGRE_SQL,
        SYBASE,
        HANA
    }

    public enum EnumOperator
    {
        EQUAL,
        NOT_EQUAL,
        IN,
        NOT_IN,
        LESS,
        LESS_OR_EQUAL,
        GREATER,
        GREATER_OR_EQUAL,
        BETWEEN,
        NOT_BETWEEN,
        BEGINS_WITH,
        NOT_BEGINS_WITH,
        CONTAINS,
        NOT_CONTAINS,
        ENDS_WITH,
        NOT_ENDS_WITH,
        IS_EMPTY,
        IS_NOT_EMPTY,
        IS_NULL,
        IS_NOT_NULL
    }
}
