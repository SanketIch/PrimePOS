using System;
using System.Collections;
using System.Text;

namespace MMS.PAYMENTHOST
{
	/// <summary>
	/// Synchronized wrapper around the unsynchronized ArrayList class
	/// </summary>
	    public class MMSQueue
	    {
		    private Queue me;
            
            public MMSQueue()
		    {
			    //
			    // TODO: Add constructor logic here
			    //
			    me = new Queue();

		    }

		    /// <summary>
		    /// 
		    /// </summary>
		    public void Clear()
		    {
			    lock (me.SyncRoot)
			    {
				    me.Clear();
			    }
		    }

		    /// <summary>
		    /// 
		    /// </summary>
		    /// <returns></returns>
		    public int Count() 
		    {
			    lock (me.SyncRoot)
			    {
				    return me.Count;
			    }
		    }

		    /// <summary>
		    /// 
		    /// </summary>
		    /// <returns></returns>
		    public object Dequeue()
		    {
			    lock (me.SyncRoot)
			    {
				    return me.Dequeue();
			    }
		    }

		    /// <summary>
		    /// 
		    /// </summary>
		    /// <param name="obj"></param>
		    public void Enqueue(object obj)
		    {
			    lock (me.SyncRoot)
			    {
				    me.Enqueue(obj);
			    }
		    }

		    /// <summary>
		    /// 
		    /// </summary>
		    /// <returns></returns>
		    public object Peek()
		    {
			    lock (me.SyncRoot)
			    {
				    return me.Peek();
			    }
		    }
        }
    }
