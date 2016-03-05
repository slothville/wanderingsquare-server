using System;
using System.Collections;
using System.Collections.Generic;

namespace Personal.Collections.Generic
{
	public class MovingAverage
	{
		private RingBuffer<float> buffer;

		public MovingAverage (int order)
		{
			buffer = new RingBuffer<float>(order);
		}

		public void Add(float value)
		{
			buffer.Enqueue (value);
		}

		public int GetAverage()
		{
			float sum = 0;

			for(int index=0;index<buffer.Count;index++)
			{
				sum += buffer [index];
			}

			return (int)(sum / buffer.Capacity);
		}
	}
}

