using System;

namespace BaseFrame
{
	public interface IChangingInfo
	{
		bool HasChanged { get; }

		void StartWatchingToChanging();
		void StopWatchingToChanging();
		void ResetWatchingToChanging();
	}
}
