using System;
using System.Drawing.Text;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using AtalLogger;

namespace AtalLoggerStoreDataApp
{
	class AtalLoggerDataRetrievalCycle
	{
		public static AtalLoggerDataRetrievalCycle Instance { get; private set; }

		private bool databaseOk;

		public void CheckDatabase()
		{
			throw new NotImplementedException();
		}

		private AtalLoggerDataRetrievalCycleState state;

		private AtalLoggerDataRetrievalCycleState State
		{
			get
			{
				lock (this) return state;
			}
			set
			{
				lock (this)
					state = value;
			}
		}


		public void Start(Control c)
		{
			lock (this)
			{
				//busy  ?
				if (State!=AtalLoggerDataRetrievalCycleState.Idle)
					throw new InvalidOperationException("Nog bezig met uitlezen logger...");
				State = AtalLoggerDataRetrievalCycleState.FindingLogger;
			}

			//database checked and ok ?
			CheckDatabase();
			if(!databaseOk)
				throw new Exception("Database niet gevonden.  Metingen kunnen niet opgeslagen worden.");

			new Thread(Cycle).Start(c);
			
			
			//next steps to be done in separate thread
			
		}

		
		private void Cycle(object o)
		{
			try
			{
				Control c = (Control) o;

				InnerCycle(c);
			}
			finally
			{
				State = AtalLoggerDataRetrievalCycleState.Idle;
			}
		}

		private AtalLogger.AtalLogger DetectLogger()
		{
			int count = 5;
			bool done;
			AtalLogger.AtalLogger logger=null;
			do
			{
				ReportProgress(c, AtalLoggerDataRetrievalCycleState.FindingLogger,
					string.Format("Detectie logger - nog {0}s", count));
				logger = new AtalLoggerFinder().FindLoggerPort(null);
				count--;
				done = logger == null && count > 0;
				if (!done)
				{
					Thread.Sleep(1000);
				}
			} while (!done);
			if(logger==null)
				throw new Exception("De logger werd niet gedecteerd !\nSteek de logger (opnieuw) in de USB poort\nen probeer opnieuw.");
			return logger;
		}

		private void InnerCycle(Control c)
		{
			//STEP 1. Look for logger - 5 sec
			var logger = DetectLogger();

			State = AtalLoggerDataRetrievalCycleState.ReadingLoggerInfo;
			ReportProgress(c,state,"Uitlezen logger : serienummer & aantal metingen.");

			//STEP 2. Get details from logger.
			var details = logger.GetDetailsFromDevice();
			ReportProgress(c,state,string.Format("Uitlezen logger : {0} heeft {1} metingen", details.SerialNumber,details.NumberOfSamples));

			if (details.NumberOfSamples == 0)
				throw new Exception("Geen metingen op de logger beschikbaar.");

			State = AtalLoggerDataRetrievalCycleState.ReadingLoggerSamples;
			Thread.Sleep(1000);
			ReportProgress(c, state, string.Format("Uitlezen logger : 0 van {0} uitgelezen", details.NumberOfSamples));

			//STEP 3. Read samples from logger.
			var totalRead = 0;
			logger.GetSamplesFromDevice(details.NumberOfSamples,
				i =>
				{
					totalRead += i;
					ReportProgress(c, state,
						string.Format("Uitlezen logger : {0} van {1} uitgelezen", totalRead, details.NumberOfSamples));
				},
				false); //TODO : support for debug dump...

			//STEP 4. Store the samples in the database.
		}

		public event EventHandler<ProgressEventArgs> OnProgress;

		private void ReportProgress(Control c, AtalLoggerDataRetrievalCycleState state, string msg)
		{
			var m = new MethodInvoker(() =>
				{
					OnProgress.Invoke(this, new ProgressEventArgs(state, msg));
				});
			c.Invoke(m);
		}

		private AtalLoggerDataRetrievalCycle()
		{
			Instance = new AtalLoggerDataRetrievalCycle();
		}
	}

	class ProgressEventArgs : EventArgs
	{
		public AtalLoggerDataRetrievalCycleState State { get; private set; }

		public string Message { get; private set; }

		public ProgressEventArgs(AtalLoggerDataRetrievalCycleState state, string msg)
		{
			State = state;
			Message = msg;
		}
	}
}
