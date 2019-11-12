namespace AtalLogger
{
	public interface  IAtalLogger
	{
		string SerialNumber { get; }

		AtalLoggerDetails GetDetailsFromDevice();

		AtalLoggerSample[] GetSamplesFromDevice(int numberOfSamples);

		void ClearDataOnDevice();

		void Dispose();
	}

	public delegate void SamplesReadingCallback(AtalLoggerSample[] readSamples, int totalNumberOfSamples, int offSet);

}
