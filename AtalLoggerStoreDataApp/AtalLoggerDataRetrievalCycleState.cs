namespace AtalLoggerStoreDataApp
{
	enum AtalLoggerDataRetrievalCycleState
	{
		Idle,
		FindingLogger,
		ReadingLoggerInfo,
		ReadingLoggerSamples,
		StoringLoggerSamples,
		ClearingLoggerData
	}
}
