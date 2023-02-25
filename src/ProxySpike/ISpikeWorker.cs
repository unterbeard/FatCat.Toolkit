namespace ProxySpike;

public interface ISpikeWorker<T> where T : class
{
	Task DoWork(T options);
}