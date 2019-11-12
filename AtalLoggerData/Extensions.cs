using System;
using System.Data.SQLite;

namespace AtalLoggerData
{
    public static class Extensions
    {
	    public static void AddParameter(this SQLiteCommand cmd, string param, object value)
	    {
		    var sn = cmd.CreateParameter();
		    sn.ParameterName = param;
		    sn.Value = value;
		    cmd.Parameters.Add(sn);
	    }

    }
}
