using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace AtalLoggerData
{
    public class AtalLoggerDatabase
    {
	    private string fileName;
	    private bool _checked;

	    private void VerifySchema(SQLiteConnection c)
	    {
			//verify schema.... ?
	    }

	    private SQLiteConnection _connection;

	    internal SQLiteConnection Connect()
	    {
		    if (_connection != null)
		    {
			    return _connection;
		    }
			    
		    var csb = new SQLiteConnectionStringBuilder();
		    csb.DataSource = fileName;
		    var c = new SQLiteConnection(csb.FullUri);
			c.Open();
			_connection = c;
			c.StateChange += C_StateChange;
			return c;
	    }

		private void C_StateChange(object sender, StateChangeEventArgs e)
		{
			if (e.CurrentState == ConnectionState.Closed)
				_connection = null;
		}

		public bool Check()
	    {
		    if (!File.Exists(fileName))
			    return false;
			try
			{
				using (var c = Connect())
				{
					VerifySchema(c);
				}
				_checked = true;
				return true;
			} catch {}

			return false;
	    }

	    private Logger GetLogger(SQLiteCommand cmd, string serialNumber)
	    {
		    cmd.CommandText = "SELECT * FROM Logger WHERE SerialNumber=@sn";
		    cmd.AddParameter("@sn",serialNumber);
		    using (var r = cmd.ExecuteReader())
		    {
			    if (r.HasRows)
			    {
				    r.Read();
				    return new Logger
				    {
					    ID = r.GetInt32(0),
					    SerialNumber = serialNumber,
						Database = this
				    };
			    }
		    }
		    return null;
	    }

	    private void AddLogger(SQLiteCommand cmd, string serialNumber)
	    {
		    cmd.CommandText = "INSERT INTO Logger (SerialNumber) VALUES (@sn)";
		    cmd.AddParameter("@sn",serialNumber);
		    cmd.ExecuteNonQuery();
	    }

	    public Logger GetOrAddLogger(string serialNumber)
	    {
		    if (!_checked)
			    throw new InvalidOperationException(
				    "De database is niet ok : de gegevens kunnen niet worden gestockeerd.");

		    using (var cmd = Connect().CreateCommand())
		    {
			    //CREATE TABLE Logger (
			    //	ID           INTEGER PRIMARY KEY AUTOINCREMENT,
			    //	SerialNumber STRING  UNIQUE
			    //	);

			    var result = GetLogger(cmd, serialNumber);
			    if (result == null)
			    {
				    AddLogger(cmd, serialNumber);
				    result = GetLogger(cmd, serialNumber);
				    return result;
			    }
		    }
			throw  new Exception("Ophalen/creëren van de logger in de database heeft gefaald.");
	    }

	   

	    public AtalLoggerDatabase(string databaseFileName)
        {
	        fileName = databaseFileName;
        }
    }
}
