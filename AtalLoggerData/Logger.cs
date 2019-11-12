using System;

namespace AtalLoggerData
{
    public class Logger
    {
		internal AtalLoggerDatabase Database { get; set; }

        public int ID { get; set; }

		public string SerialNumber { get; set; }

		public DataRetrieval AddDataRetrieval()
		{
			if(Database==null)
				throw new InvalidOperationException("Database niet geopend.");
			//CREATE TABLE DataRetrieval (
			//	ID             INTEGER  PRIMARY KEY AUTOINCREMENT,
			//	Logger         INTEGER  REFERENCES Logger (ID),
			//TimeStamp      DATETIME,
			//	DataWasCleared BOOLEAN
			//	);
			var ts = DateTime.UtcNow;

			var c = Database.Connect();
			using (var cmd = c.CreateCommand())
			{
				cmd.CommandText = "INSERT INTO DataRetrieval (Logger, TimeStamp) VALUES (@logger,@ts)";
				cmd.AddParameter("@logger", ID);
				cmd.AddParameter("@ts", ts);
				cmd.ExecuteNonQuery();
				cmd.CommandText = "SELECT last_insert_rowid()";
				var id = (int)cmd.ExecuteScalar();
				return new DataRetrieval
				{
					ID = id,
					TimeStamp = ts
				};
			}
		}

		public void AddSample(DataRetrieval dr, DateTime ts, double temp)
		{
			//CREATE TABLE Sample (
			//	ID            INTEGER  PRIMARY KEY AUTOINCREMENT,
			//	Logger        INTEGER  REFERENCES Logger (ID),
			//	DataRetrieval INTEGER  REFERENCES DataRetrieval (ID),
			//	TimeStamp     DATETIME,
			//	Temperature   DOUBLE
			//	);

			var c = Database.Connect();
			using (var cmd = c.CreateCommand())
			{
				cmd.CommandText = "SELECT 1 FROM Sample WHERE Logger=@l AND TimeStamp=@ts";
				cmd.AddParameter("@l",ID);
				cmd.AddParameter("@ts",ts);
				bool exists = cmd.ExecuteScalar() != null;
				if (exists)
					return;

				cmd.CommandText =
					"INSERT INTO Sample (Logger, DataRetrieval, TimeStamp, Temperature) VALUES (@l,@dr,@ts,@tmp)";
				cmd.AddParameter("@dr",dr.ID);
				cmd.AddParameter("@tmp",temp);
				cmd.ExecuteNonQuery();
			}
		}
    }
}
